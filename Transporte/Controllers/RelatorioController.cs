using Microsoft.AspNetCore.Mvc;
using System.Net;
using TransporteApi.Models;
using TransporteApi.Models.DTO;
using TransporteApi.Models.Interfaces;

namespace TransporteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EntregaController> _logger;
        private readonly IRelatorioService _service;

        public RelatorioController(AppDbContext context, ILogger<EntregaController> logger, IRelatorioService service)
        {
            _context = context;
            _logger = logger;
            _service = service;
        }

        [HttpGet("total-por-status")]
        public async Task<ActionResult<List<IndiceStatusDto>>> ObterQuatidadePorStatus()
        {
            try
            {

                var result = await _service.ObterQuatidadePorStatus();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("total-atrasadas")]
        public async Task<ActionResult<int>> ObterTotalEntregasAtrasadas()
        {
            try
            {
                var result = await _service.ObterTotalEntregasAtrasadas();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("quantidade-entrega-por-dia")]
        public async Task<ActionResult<List<QuantidadeEntregaPorDia>>> ObterQuantidadeEntregaPorDia(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim
        )
        {
            try
            {
                var result = await _service.ObterQuantidadeEntregaPorDia(dataInicio, dataFim);
                return Ok(result);
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
    }
}
