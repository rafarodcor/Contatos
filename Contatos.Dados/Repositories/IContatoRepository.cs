using Contatos.Modelos.Modelos;

namespace Contatos.Dados.Repositories;

public interface IContatoRepository
{
    #region Methods

    Task<IEnumerable<Contato>> RecuperarContatosAsync(string? ddd, int pagina, int tamanhoPagina);
    Task<Contato?> RecuperarContatoPorIdAsync(int id);
    Task IncluirContatoAsync(Contato contato);
    Task AtualizarContatoAsync(Contato contato);
    Task DeletarContatoAsync(Contato contato);

    #endregion
}