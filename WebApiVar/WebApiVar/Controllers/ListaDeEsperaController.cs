using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiVar.Application.Services;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListaDeEsperaController : Controller
    {
        private readonly VAREntities _ctx;
        public ListaDeEsperaController(VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        [Authorize]
        [Route("ObterListaDeEspera/")]
        public IActionResult ObterListaDeEspera()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var usuarioService = new UsuarioService(_ctx);
            var listadeespera = usuarioService.ObterListaDeEspera();
            return Ok(listadeespera);
        }
        [HttpGet]
        [Route("Aprovar/{id}")]
        [Authorize]
        public IActionResult AprovarMatricula([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var usuarioService = new UsuarioService(_ctx);
            var sucesso = usuarioService.AprovarMatricula(id);
            if (sucesso)
                return NoContent();
            else
                return BadRequest();
        }
        [HttpGet]
        [Route("Rejeitar/{id}")]
        [Authorize]
        public IActionResult RejeitarMatricula([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var usuarioService = new UsuarioService(_ctx);
            var sucesso = usuarioService.RejeitarMatricula(id);
            if (sucesso)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
