using Aplicacion.Caracteristicas.Usuario;
using Azure.Core;
using Cliente.Server.Controllers.Interfaces;
using Cliente.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Throw;

namespace Cliente.Server.Controllers
{
    public class UsuarioController : ApiControllerBase
    {
        private readonly ILogger<UsuarioController> logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UsuarioDTO>>> GetAll()
        {
            try
            {
                logger.LogInformation("**** CONSULTANDO USUARIOS ****");
                var resultado = await Mediator.Send(new ObtenerTodo.Consulta());
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(InsertarComandoDTO request)
        {
            try
            {
                logger.LogInformation("**** CREANDO USUARIO ****");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error) {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> AuthLogin(LoginDTO request)
        {
            try
            {
                logger.LogInformation("********* Login ************");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpPut("send")]
        public async Task<ActionResult> SendCode(EnviarCodigoDTO request)
        {
            try
            {
                logger.LogInformation("********* Enviar codigo ************");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
        [HttpPut("recovery")]
        public async Task<ActionResult> Recovery(RecuperarPasswordDTO request)
        {
            try
            {
                logger.LogInformation("********* RECUPERAR PASSWORD ************");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }

        [HttpPut("token")]
        public async Task<ActionResult> ValidateToken(TokenDTO request)
        {
            try
            {
                logger.LogInformation("********* OBTENER USUARIOS POR EL TOKEN ************");
                var resultado = await Mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception error)
            {
                return BadRequest(new ErrorResponseDTO(error.Message));
            }
        }
    }
}
