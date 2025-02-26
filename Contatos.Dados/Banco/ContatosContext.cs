using Contatos.Modelos.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Contatos.Dados.Banco;

public class ContatosContext : DbContext
{
    #region Constructors

    private readonly IConfiguration _configuration;

    public ContatosContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    public DbSet<Contato> Contatos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contato>(contato =>
        {
            contato.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();

            contato.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(50);

            contato.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(30)
                .HasAnnotation("RegularExpression", @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            contato.OwnsOne(c => c.Telefone, telefone =>
            {
                telefone.Property(t => t.DDD)
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasAnnotation("RegularExpression", @"^\d{3}$");

                telefone.Property(t => t.Numero)
                    .HasMaxLength(10)
                    .IsRequired()
                    .HasAnnotation("RegularExpression", @"^\d{8,10}$");
            });
        });
    }

    //private string connectionString = "Server=localhost,1433;Database=ContatosDatabase;User ID=sa;Password=sqlRRC00!;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
}