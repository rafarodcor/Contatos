using Contatos.Modelos.Modelos;
using System.ComponentModel.DataAnnotations;

namespace Contatos.Testes;

public class ContatoTests
{
    #region Methods

    [Fact]
    public void Contato_DeveSerValido_QuandoDadosCorretos()
    {
        // Arrange
        var telefone = new Telefone("011", "987654321");
        var contato = new Contato(1, "Rafael Corrêa", "rafael.correa@example.com", telefone);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(contato, new ValidationContext(contato), validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Contato_DeveSerInvalido_QuandoEmailInvalido()
    {
        // Arrange
        var telefone = new Telefone("011", "987654321");
        var contato = new Contato(1, "Rafael Corrêa", "email_invalido", telefone);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(contato, new ValidationContext(contato), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage!.Contains("Digite um e-mail válido"));
    }

    [Fact]
    public void Contato_DeveSerInvalido_QuandoNomeVazio()
    {
        // Arrange
        var telefone = new Telefone("011", "987654321");
        var contato = new Contato(1, "", "rafael.correa@example.com", telefone);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(contato, new ValidationContext(contato), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage!.Contains("O campo nome é obrigatório"));
    }

    #endregion
}