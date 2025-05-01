using Contatos.Modelos.Modelos;

namespace Contatos.Services.Services.Persistence;

public interface IContatoService
{
    #region Methods

    Task<IEnumerable<Contato>> RecuperarContatosAsync(string? ddd, int pagina, int tamanhoPagina);
    Task<Contato> RecuperarContatoPorIdAsync(int id);
    Task IncluirContatoAsync(Contato contato);
    Task AtualizarContatoAsync(Contato contato);
    Task DeletarContatoAsync(Contato contato);

    #endregion
}