using Contatos.Dados.Repositories;
using Contatos.Modelos.Modelos;
using Contatos.Services.Services.Producer;

namespace Contatos.Services.Services.Persistence;

public class ContatoService : IContatoService
{
    #region Properties

    private readonly IContatoRepository _contatoRepository;
    private readonly IIncluirContatoProducer _incluirContatoProducer;
    private readonly IAtualizarContatoProducer _atualizarContatoProducer;
    private readonly IDeletarContatoProducer _deletarContatoProducer;

    #endregion

    #region Constructors

    public ContatoService(IContatoRepository contatoRepository,
        IIncluirContatoProducer incluirContatoProducer,
        IAtualizarContatoProducer atualizarContatoProducer,
        IDeletarContatoProducer deletarContatoProducer)
    {
        _contatoRepository = contatoRepository;
        _incluirContatoProducer = incluirContatoProducer;
        _atualizarContatoProducer = atualizarContatoProducer;
        _deletarContatoProducer = deletarContatoProducer;
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
        _incluirContatoProducer.Publish(contato);
    }

    public async Task AtualizarContatoAsync(Contato contato)
    {
        _atualizarContatoProducer.Publish(contato);
    }

    public async Task DeletarContatoAsync(Contato contato)
    {
        _deletarContatoProducer.Publish(contato);
    }

    #endregion
}