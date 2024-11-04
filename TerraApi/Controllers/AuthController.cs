using Microsoft.AspNetCore.Mvc;
using TerraApi.Dao.Usuario;
using TerraApi.Modelos.Usuario;

namespace TerraApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDao _usuarioDao;

        public AuthController(UsuarioDao usuarioDao)
        {
            _usuarioDao = usuarioDao;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UsuarioData), 200)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody] UsuarioData request)
        {
            var usuarioData = _usuarioDao.Login(request.Usuario, request.Contraseña);
            if (usuarioData != null)
            {
                return Ok(usuarioData);
            }
            return Unauthorized();
        }
    }
}
