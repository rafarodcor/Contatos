using System.ComponentModel.DataAnnotations;

namespace Contatos.Modelos.Modelos;

public class Contato
{
    #region Constructors

    public Contato() { }

    public Contato(int id, string nome, string email, Telefone telefone)
    {
        Id = id;
        Nome = nome;
        Telefone = telefone;
        Email = email;
    }

    #endregion

    #region Properties

    public int Id { get; set; }

    [Required(ErrorMessage="O campo nome é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo e-mail é obrigatório")]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
    public string Email { get; set; }

    [Required]
    public Telefone Telefone { get; set; }

    #endregion

    #region Methods

    public override string ToString()
    {
        return @$"Id: {Id} - Nome: {Nome}";
    }

    #endregion
}