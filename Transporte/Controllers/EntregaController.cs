using TransporteApi.Models;
using TransporteApi.Models.DTO;
using TransporteApi.Models.Interfaces;
using TransporteApi.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ResponseHelperPaginado<List<EntregaDto>>>> ObterEntregas (
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
                List<EntregaDto> response = new List<EntregaDto>();

                foreach (var item in result)
                {
                    response.Add(new EntregaDto(item)); 
                }

                return Ok(new ResponseHelperPaginado<List<EntregaDto>>(response, response.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
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
