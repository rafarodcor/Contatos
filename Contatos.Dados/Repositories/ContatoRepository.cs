using Contatos.Dados.Banco;
using Contatos.Modelos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Contatos.Dados.Repositories;

public class ContatoRepository : IContatoRepository
{
    #region Properties

    private readonly ContatosContext _context;

    #endregion

    #region Constructors

    public ContatoRepository(ContatosContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<IEnumerable<Contato>> RecuperarContatosAsync(string? ddd, int pagina, int tamanhoPagina)
    {
        var query = _context.Contatos.AsQueryable();

        if (!string.IsNullOrEmpty(ddd))
        {
            query = query.Where(c => c.Telefone.DDD == ddd);
        }

        return await query.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync();
    }

    public async Task<Contato?> RecuperarContatoPorIdAsync(int id)
    {
        return await _context.Contatos.FindAsync(id);
    }

    public async Task IncluirContatoAsync(Contato contato)
    {
        _context.Contatos.Add(contato);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarContatoAsync(Contato contato)
    {
        _context.Contatos.Update(contato);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarContatoAsync(Contato contato)
    {
        _context.Contatos.Remove(contato);
        await _context.SaveChangesAsync();
    }

    #endregion
}