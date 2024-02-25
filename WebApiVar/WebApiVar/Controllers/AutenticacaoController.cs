using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using WebApiVar.Application.Models;
using WebApiVar.Application.Services;
using WebApiVar.Repository;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController : Controller
    {
        private readonly VAREntities _ctx;
        public AutenticacaoController(VAREntities ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var autenticacao = new AutenticacaoService(_ctx);
            var token = autenticacao.VerificarLogin(request);

            if (token == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(token);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult VerificarAcesso()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var status = identidade.FindFirst("status").Value;

            return Ok(new { usuarioId = usuarioId, status = status });
        }
        [HttpGet]
        [Authorize]
        [Route("Presenca")]
        public IActionResult MarcarPresenca()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var status = identidade.FindFirst("status").Value;
            if (status != "1")
                return NoContent();
            var autenticacao = new AutenticacaoService(_ctx);
            var token = autenticacao.MarcarPresenca(Convert.ToInt32(usuarioId));

            return Ok(token);

        }
    }
}
