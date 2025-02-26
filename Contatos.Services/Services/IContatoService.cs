using Contatos.Modelos.Modelos;

namespace Contatos.Services.Services;

public interface IContatoService
{
    Task<List<Contato>> RecuperarContatos(string? ddd = null);
    Task<Contato> RecuperarContatoPorId(int id);
    Task IncluirContato(Contato contato);
    Task AtualizarContato(Contato contato);
    Task DeletarContato(Contato contato);
}