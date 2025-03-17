using Contatos.Dados.Banco;
using Contatos.Modelos.Modelos;
using Contatos.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ContatoController(ICacheService cacheService, IContatoService contatoService, ContatosContext context) : ControllerBase
{
    #region Constants

    private static readonly List<string> LISTA_DDD_BRASIL =
    [
      "099", "098", "097", "096", "095", "094", "093", "092", "091", "089", "088", "087", "086", "085", "084", "083",
      "082", "081", "079", "077", "075", "074", "073", "071", "069", "068", "067", "066", "065", "064", "063", "062",
      "061", "055", "054", "053", "051", "049", "048", "047", "046", "045", "044", "043", "042", "041", "038", "037",
      "035", "034", "033", "032", "031", "028", "027", "024", "022", "021", "019", "018", "017", "016", "015", "014",
      "013", "012", "011"
    ];

    #endregion

    #region Properties

    private readonly ICacheService _cacheService = cacheService;
    private readonly IContatoService _contatoService = contatoService;
    private readonly ContatosContext _context = context;

    #endregion

    #region Methods

    [HttpGet("/listar")]
    public async Task<IActionResult> RecuperarContatosAsync([FromQuery] string? ddd = null, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
    {
        try
        {
            if (!string.IsNullOrEmpty(ddd))
                ddd = FormatarDDD(ddd);

            var key = $"listaContato_{pagina}_{tamanhoPagina}_{ddd}";
            var cachedContatos = _cacheService.Get(key);

            if (cachedContatos != null)
            {
                return Ok(cachedContatos);
            }

            var listaContatos = await _contatoService.RecuperarContatosAsync(ddd, pagina, tamanhoPagina);

            _cacheService.Set(key, listaContatos);

            return Ok(listaContatos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> RecuperarContatoPorIdAsync(int id)
    {
        try
        {
            var cachedContato = _cacheService.Get(id.ToString());

            if (cachedContato != null)
            {
                return Ok(cachedContato);
            }

            var contato = await _contatoService.RecuperarContatoPorIdAsync(id);

            if (contato != null)
            {
                _cacheService.Set(id.ToString(), contato);
                return Ok(contato);
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("/incluir")]
    public async Task<IActionResult> IncluirContatoAsync([FromBody] Contato contato)
    {
        try
        {
            contato.Telefone.DDD = FormatarDDD(contato.Telefone.DDD);

            if (!LISTA_DDD_BRASIL.Contains(contato.Telefone.DDD))
            {
                return BadRequest("DDD inválido.");
            }

            await _contatoService.IncluirContatoAsync(contato);
            _cacheService.Set(contato.Id.ToString(), contato);

            return Ok($"Contato com chave '{contato.Id}' incluído com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarContatoAsync(int id)
    {
        try
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato != null)
            {
                await _contatoService.DeletarContatoAsync(contato);
                _cacheService.Remove(id.ToString());

                return Ok($"Contato com chave '{id}' deletado com sucesso.");
            }

            return NotFound();

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("/atualizar")]
    public async Task<IActionResult> AtualizarContatoAsync([FromBody] Contato contato)
    {
        try
        {
            contato.Telefone.DDD = FormatarDDD(contato.Telefone.DDD);

            if (!LISTA_DDD_BRASIL.Contains(contato.Telefone.DDD))
            {
                return BadRequest("DDD inválido.");
            }

            await _contatoService.AtualizarContatoAsync(contato);
            _cacheService.Set(contato.Id.ToString(), contato);

            return Ok($"Contato com chave '{contato.Id}' atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #region Private Methods

    private string FormatarDDD(string ddd)
    {
        return ddd.Length == 2 ? $"0{ddd}" : ddd;
    }

    #endregion

    #endregion
}