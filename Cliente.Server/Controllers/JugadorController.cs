using Aplicacion.Caracteristicas.Jugador;
using Aplicacion.Helper.Comunes;
using Azure.Core;
using Cliente.Server.Controllers.Interfaces;
using Cliente.Shared;
using Cliente.Shared.Jugador;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf.Types;

namespace Cliente.Server.Controllers
{
    public class JugadorController : ApiControllerBase
    {
        private readonly ILogger<JugadorController> logger;
        public JugadorController(ILogger<JugadorController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO JUGADORES ****");
                var resultado = await Mediator.Send(new ObtenerTodo.Consulta());
                return Ok(resultado);
            }
            catch (ExcepcionValidacion error)
            {
                return BadRequest(error.Errors);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAllByRepresentante(int id)
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO JUGADORES POR REPRESENTANTE ****");
                var resultado = await Mediator.Send(new ObtenerTodoPorRepresentante.Comando(id));
                return Ok(resultado);
            }
            catch (ExcepcionValidacion error)
            {
                return BadRequest(error.Errors);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpPost]
        public async Task<ActionResult<JugadorDTO>> Create(JugadorDTO request)
        {
            try
            {
                logger.LogInformation("**** CREANDO JUGADOR ****");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (ExcepcionValidacion error)
            {
                return BadRequest(error.Errors);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpPut]
        public async Task<ActionResult<JugadorDTO>> Update(Actualizar.Comando request)
        {
            try
            {
                logger.LogInformation("**** ACTUALIZANDO JUGADOR ****");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<JugadorDTO>> Delete(int id)
        {
            try
            {
                logger.LogInformation("**** ELIMINANDO JUGADOR ****");
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
