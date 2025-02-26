using Contatos.Dados.Banco;
using Contatos.Modelos.Modelos;
using Contatos.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContatoController(ICacheService cacheService, IContatoService contatoService, ContatosContext context) : ControllerBase
{
    #region Properties

    private readonly ICacheService _cacheService = cacheService;
    private readonly IContatoService _contatoService = contatoService;
    private readonly ContatosContext _context = context;

    #endregion

    #region Methods

    [HttpGet("/listar")]
    public async Task<IActionResult> RecuperarContatos()
    {
        try
        {
            var key = "listaContato";
            var cachedContatos = _cacheService.Get(key);

            if (cachedContatos != null)
            {
                return Ok(cachedContatos);
            }

            var listaContatos = await _contatoService.RecuperarContatos();

            _cacheService.Set(key, listaContatos);

            return Ok(listaContatos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> RecuperarContatoPorId(int id)
    {
        try
        {
            var cachedContato = _cacheService.Get(id.ToString());

            if (cachedContato != null)
            {
                return Ok(cachedContato);
            }

            var contato = await _contatoService.RecuperarContatoPorId(id);

            if(contato != null)
            {
                _cacheService.Set(id.ToString(), contato);
                return Ok(contato);
            }         

            return NotFound();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("ddd/{ddd}")]
    public async Task<IActionResult> RecuperarContatosPorDDD(string ddd)
    {
        try
        {
            var cachedListaContatosPorDDD = _cacheService.Get(ddd);

            if (cachedListaContatosPorDDD != null)
            {
                return Ok(cachedListaContatosPorDDD);
            }

            var listaContatosPorDDD = await _contatoService.RecuperarContatos(ddd);

            _cacheService.Set(ddd, listaContatosPorDDD);

            return Ok(listaContatosPorDDD);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("/incluir")]
    public async Task<IActionResult> IncluirContato([FromBody] Contato contato)
    {
        try
        {
            await _contatoService.IncluirContato(contato);
            _cacheService.Set(contato.Id.ToString(), contato);

            return Ok($"Contato com chave '{contato.Id}' incluído com sucesso.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{id}")]    
    public async Task<IActionResult> DeletarContato(int id)
    {
        try
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato != null)
            {
                await _contatoService.DeletarContato(contato);
                _cacheService.Remove(id.ToString());

                return Ok($"Contato com chave '{id}' deletado com sucesso.");
            }

            return NotFound();

        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    [Route("/atualizar")]
    public async Task<IActionResult> AtualizarContato([FromBody] Contato contato)
    {
        try
        {
            await _contatoService.AtualizarContato(contato);
            _cacheService.Set(contato.Id.ToString(), contato);

            return Ok($"Contato com chave '{contato.Id}' atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    #endregion
}