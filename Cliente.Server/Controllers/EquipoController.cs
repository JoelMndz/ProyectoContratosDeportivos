using Aplicacion.Caracteristicas.Equipo;
using Azure.Core;
using Cliente.Server.Controllers.Interfaces;
using Cliente.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cliente.Server.Controllers
{
    public class EquipoController : ApiControllerBase
    {
        private readonly ILogger<EquipoController> logger;

        public EquipoController(ILogger<EquipoController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO EQUIPOS ****");
                var resultado = await Mediator.Send(new ObtenerTodo.Consulta());
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAllByGerente(int id)
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO EQUIPOS POR GERENTE ****");
                var resultado = await Mediator.Send(new ObtenerTodoPorGerente.Comando(id));
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(InsertarEquipoDTO request)
        {
            try
            {
                logger.LogInformation("*** CREANDO EQUIPO ***");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Actualizar.Comando request)
        {
            try
            {
                logger.LogInformation("*** ACTUALIZANDO EQUIPO ***");
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
                logger.LogInformation("*** ELIMINANDO EQUIPO ***");
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
