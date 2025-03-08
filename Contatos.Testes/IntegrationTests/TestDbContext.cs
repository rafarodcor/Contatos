using Castle.Core.Configuration;
using Contatos.Dados.Banco;
using Microsoft.EntityFrameworkCore;

namespace Contatos.Testes.IntegrationTests;

public class TestDbContext : ContatosContext
{
    public TestDbContext(IConfiguration configuration, DbContextOptions<ContatosContext> options) 
        : base(configuration: null, options: options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("TestIntegrationDatabase");
    }
}