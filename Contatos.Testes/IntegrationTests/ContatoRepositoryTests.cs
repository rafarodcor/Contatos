using Contatos.Dados.Banco;
using Contatos.Dados.Repositories;
using Contatos.Modelos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Contatos.Testes.IntegrationTests;
public class ContatoRepositoryTests
{
    #region Properties

    private readonly ContatoRepository _repository;
    private readonly TestDbContext _context;

    #endregion

    #region Constructors

    public ContatoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ContatosContext>()
            .UseInMemoryDatabase(databaseName: "TestIntegrationDatabase")
            .Options;

        _context = new TestDbContext(options);
        _repository = new ContatoRepository(_context);
    }

    #endregion

    #region Methods

    [Fact]
    public async Task IncluirContatoAsync_DeveAdicionarContato()
    {
        // Arrange
        var contato = new Contato
        {
            Id = 1,
            Nome = "Teste1",
            Email = "teste1@teste.com",
            Telefone = new Telefone { DDD = "011", Numero = "123456789" }
        };

        // Act
        await _repository.IncluirContatoAsync(contato);
        var result = await _context.Contatos.FindAsync(contato.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contato.Nome, result.Nome);
    }

    [Fact]
    public async Task RecuperarContatosAsync_DeveRetornarContatos()
    {
        // Arrange
        var contato1 = new Contato
        {
            Id = 2,
            Nome = "Teste2",
            Email = "teste2@teste.com",
            Telefone = new Telefone { DDD = "016", Numero = "943218765" }
        };

        var contato2 = new Contato
        {
            Id = 3,
            Nome = "Teste3",
            Email = "teste2@teste.com",
            Telefone = new Telefone { DDD = "017", Numero = "987654321" }
        };

        await _repository.IncluirContatoAsync(contato1);
        await _repository.IncluirContatoAsync(contato2);

        // Act
        var result = await _repository.RecuperarContatosAsync(null, 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task DeletarContatoAsync_DeveRemoverContato()
    {
        // Arrange
        var contato = new Contato
        {
            Id = 4,
            Nome = "Teste4",
            Email = "teste4@teste.com",
            Telefone = new Telefone { DDD = "013", Numero = "912345678" }
        };
        await _repository.IncluirContatoAsync(contato);

        // Act
        await _repository.DeletarContatoAsync(contato);
        var result = await _context.Contatos.FindAsync(contato.Id);

        // Assert
        Assert.Null(result);
    }

    #endregion
}