using Contatos.Dados.Banco;
using Microsoft.EntityFrameworkCore;

namespace Contatos.Testes.IntegrationTests;

public class TestDbContext : ContatosContext
{
    public TestDbContext(DbContextOptions<ContatosContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("TestIntegrationDatabase");
    }
}