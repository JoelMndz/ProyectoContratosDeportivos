using Aplicacion.Caracteristicas.Categoria;
using Aplicacion.Helper.Comunes;
using Azure.Core;
using Cliente.Server.Controllers.Interfaces;
using Cliente.Shared;
using Cliente.Shared.Jugador;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cliente.Server.Controllers
{
    public class CategoriaController : ApiControllerBase
    {
        private readonly ILogger<CategoriaController> logger;

        public CategoriaController(ILogger<CategoriaController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO CATEGORIAS ****");
                var resultado = await Mediator.Send(new ObtenerTodo.Consulta());
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(InsertarCategoriaDTO request)
        {
            try
            {
                logger.LogInformation("**** CREANDO CATEGORIA ****");
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
        public async Task<ActionResult> Update(Actualizar.Comando request)
        {
            try
            {
                logger.LogInformation("**** ACTUALIZANDO CATEGORIA ****");
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
                logger.LogInformation("**** ELIMINADO CATEGORIA ****");
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
