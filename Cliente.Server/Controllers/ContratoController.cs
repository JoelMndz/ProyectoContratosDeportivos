using Aplicacion.Caracteristicas.Contrato;
using Cliente.Server.Controllers.Interfaces;
using Cliente.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cliente.Server.Controllers
{
    public class ContratoController:ApiControllerBase
    {
        private readonly ILogger<ContratoController> logger;
        public ContratoController(ILogger<ContratoController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO CONTRATOS ****");
                var resultado = await Mediator.Send(new ObtenerTodo.Comando());
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpPut("gerente")]
        public async Task<ActionResult> GetAllByGerente(ObtenerTodoPorGerente.Comando request)
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO CONTRATOS POR GERENTE ****");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpPut("representante")]
        public async Task<ActionResult> GetAllByRepresentante(ObtenerTodoPorRepresentante.Comando request)
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO CONTRATOS POR REPRESENTANTE ****");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(Insertar.Comando request)
        {
            try
            {
                logger.LogInformation("**** CREANDO CONTRATO ****");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpPut]
        public async Task<ActionResult> Actualizar(Actualizar.Comando request)
        {
            try
            {
                logger.LogInformation("**** Actualizando CONTRATO ****");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                logger.LogInformation("**** Eliminando CONTRATO ****");
                var resultado = await Mediator.Send(new Eliminar.Comando(id));
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
    }
}
