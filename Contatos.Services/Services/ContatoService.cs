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

//public class ContatoService : IContatoService
//{
//    #region Constructors

//    private readonly ContatosContext _context;

//    public ContatoService(ContatosContext context)
//    {
//        _context = context;
//    }

//    #endregion

//    #region Methods

//    public async Task<List<Contato>> RecuperarContatosAsync(string? ddd, int pagina, int tamanhoPagina)
//    {
//        var query = _context.Contatos.AsQueryable();

//        if (!string.IsNullOrEmpty(ddd))
//        {
//            query = query.Where(c => c.Telefone.DDD == ddd);
//        }

//        return await query.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync();       
//    }

//    public async Task<Contato> RecuperarContatoPorIdAsync(int id)
//    {
//        return await _context.Contatos.FindAsync(id);
//    }

//    public async Task IncluirContatoAsync(Contato contato)
//    {
//        _context.Contatos.Add(contato);
//        await _context.SaveChangesAsync();
//    }

//    public async Task AtualizarContatoAsync(Contato contato)
//    {
//        _context.Contatos.Update(contato);
//        await _context.SaveChangesAsync();
//    }

//    public async Task DeletarContatoAsync(Contato contato)
//    {
//        _context.Contatos.Remove(contato);
//        await _context.SaveChangesAsync();
//    }

//    #endregion
//}