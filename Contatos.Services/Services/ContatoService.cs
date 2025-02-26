using Contatos.Dados.Banco;
using Contatos.Modelos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Contatos.Services.Services;

public class ContatoService : IContatoService
{
    #region Constructors

    private readonly ContatosContext _context;

    public ContatoService(ContatosContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<List<Contato>> RecuperarContatos(string? ddd = null)
    {
        if (string.IsNullOrEmpty(ddd))
        {
            var retorno = await _context.Contatos.ToListAsync();
            return retorno;
        }

        return await _context.Contatos
            .Where(c => c.Telefone.DDD == ddd)
            .ToListAsync();
    }

    public async Task<Contato> RecuperarContatoPorId(int id)
    {
        return await _context.Contatos.FindAsync(id);
    }

    public async Task IncluirContato(Contato contato)
    {
        _context.Contatos.Add(contato);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarContato(Contato contato)
    {
        _context.Contatos.Update(contato);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarContato(Contato contato)
    {
        _context.Contatos.Remove(contato);
        await _context.SaveChangesAsync();
    }

    #endregion
}