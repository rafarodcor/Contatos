using Contatos.Modelos.Modelos;
using System.ComponentModel.DataAnnotations;

namespace Contatos.Testes;

public class TelefoneTests
{
    #region Methods

    [Fact]
    public void Telefone_DeveSerValido_QuandoDadosCorretos()
    {
        // Arrange
        var telefone = new Telefone("011", "987654321");

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(telefone, new ValidationContext(telefone), validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Telefone_DeveSerInvalido_QuandoDDDInvalido()
    {
        // Arrange
        var telefone = new Telefone("Aa", "987654321");

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(telefone, new ValidationContext(telefone), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage!.Contains("O DDD deve conter exatamente 3 dígitos numéricos"));
    }

    [Fact]
    public void Telefone_DeveSerInvalido_QuandoNumeroInvalido()
    {
        // Arrange
        var telefone = new Telefone("011", "98765");

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(telefone, new ValidationContext(telefone), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage!.Contains("O número deve conter entre 8 e 10 dígitos numéricos"));
    }

    #endregion
}