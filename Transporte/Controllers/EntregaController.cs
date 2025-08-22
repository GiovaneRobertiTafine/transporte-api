using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using TransporteApi.Models;
using TransporteApi.Models.DTO;
using TransporteApi.Models.Interfaces;
using TransporteApi.Models.Requests;

namespace TransporteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EntregaController> _logger;
        private readonly IEntregaService _service;

        public EntregaController(AppDbContext context, ILogger<EntregaController> logger, IEntregaService service)
        {
            _context = context;
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CriarEntrega(CriarEntregaRequest request)
        {
            try
            {
                if (request.isValid())
                {
                    await _service.CriarEntrega(request);
                    return Ok();
                } else
                {
                    return BadRequest("Data estimada de entrega menor ou igual a data atual.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ResponseHelperPaginado<List<EntregaDto>>>> ObterEntregas(
            [FromQuery] string? ClienteCodigo,
            [FromQuery] StatusEntrega? Status,
            [FromQuery] int? Page,
            [FromQuery] int? PageSize
        )
        {
            try
            {
                ObterEntregasRequest req = new ObterEntregasRequest()
                {
                    ClienteCodigo = ClienteCodigo,
                    Status = Status,
                    Page = Page ?? 1,
                    PageSize = PageSize ?? 5
                };
                var result = await _service.ObterEntregas(req);
                int totalDeEntregas = result.TotalCount;

                List<EntregaDto> response = new List<EntregaDto>();
                foreach (var item in result.Items)
                {
                    response.Add(new EntregaDto(item.Id, item.Cliente, item.DataEnvio, item.Posts));
                }

                return Ok(new ResponseHelperPaginado<List<EntregaDto>>(response, result.TotalCount));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntregaDto>> ObterEntregaPorId(string id)
        {
            try
            {
                var entrega = await _service.ObterEntregaPorId(id);

                if (entrega == null)
                {
                    return NotFound();
                }

                return Ok(new EntregaDto(entrega));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EntregaDto>> AlterarStatusEntrega(
            string id,
            [FromBody] AlterarStatusEntrega request 
        )
        {
            try
            {
                var result = await _service.AlterarStatusEntrega(id, request.Status);
                if (result is null)
                {
                    return NotFound();
                }
                return Ok(new EntregaDto(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> ExcluirTodas()
        {
            await _context.HistoricoEntregas.ExecuteDeleteAsync();
            await _context.Entregas.ExecuteDeleteAsync();
            return Ok();
        }
    }
}
