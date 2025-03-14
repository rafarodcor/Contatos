using Contatos.Dados.Repositories;
using Contatos.Modelos.Modelos;

namespace Contatos.Services.Services;

public class ContatoService : IContatoService
{
    #region Properties

    private readonly IContatoRepository _contatoRepository;

    #endregion

    #region Constructors

    public ContatoService(IContatoRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }

    #endregion

    #region Methods

    public async Task<IEnumerable<Contato>> RecuperarContatosAsync(string? ddd, int pagina, int tamanhoPagina)
    {
        return await _contatoRepository.RecuperarContatosAsync(ddd, pagina, tamanhoPagina);
    }

    public async Task<Contato> RecuperarContatoPorIdAsync(int id)
    {
        return await _contatoRepository.RecuperarContatoPorIdAsync(id);
    }

    public async Task IncluirContatoAsync(Contato contato)
    {
        await _contatoRepository.IncluirContatoAsync(contato);
    }

    public async Task AtualizarContatoAsync(Contato contato)
    {
        await _contatoRepository.AtualizarContatoAsync(contato);
    }

    public async Task DeletarContatoAsync(Contato contato)
    {
        await _contatoRepository.DeletarContatoAsync(contato);
    }

    #endregion
}