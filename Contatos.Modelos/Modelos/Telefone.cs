using System.ComponentModel.DataAnnotations;

namespace Contatos.Modelos.Modelos;

public class Telefone
{
    #region Constructors

    public Telefone() { }

    public Telefone(string dDD, string numero)
    {
        DDD = dDD;
        Numero = numero;
    }

    #endregion

    #region Properties

    [Required]
    [MaxLength(3)]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "O DDD deve conter exatamente 3 dígitos numéricos")]
    public string DDD { get; set; }

    [Required]
    [MaxLength(10)]
    [RegularExpression(@"^\d{8,10}$", ErrorMessage = "O número deve conter entre 8 e 10 dígitos numéricos")]
    public string Numero { get; set; }

    #endregion
}