using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiVar.Application.Models;
using WebApiVar.Application.Services;
using WebApiVar.Repository;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        private readonly VAREntities _ctx;
        public DashboardController(VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpPut]
        [Authorize]
        public IActionResult EditarDashboard([FromBody] DashboardRequest request)
        {
            var dashboardService = new DashboardService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var sucesso = dashboardService.EditarPerfil(request, Convert.ToInt32(usuarioId));

            if (sucesso == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(sucesso);
            }
        }
        [HttpPut]
        [Authorize]
        [Route("EditarSobreMim")]
        public IActionResult EditarSobreMim([FromBody] SobreMim request)
        {
            var dashboardService = new DashboardService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var status = identidade.FindFirst("status").Value;
            if (status == "2")
                return Unauthorized();
            var sucesso = dashboardService.EditarSobreMim(request, Convert.ToInt32(usuarioId));

            if (sucesso)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Authorize]
        [Route("ObterDashboard")]
        public IActionResult ObterDashboard()
        {
            var dashboardService = new DashboardService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status == "2")
                return Unauthorized();
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var sucesso = dashboardService.ObterPerfil(Convert.ToInt32(usuarioId));

            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("Obterusuario")]
        [Authorize]

        public IActionResult ObterUsuario()
        {
            var dashboardService = new DashboardService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var sucesso = dashboardService.ObterPerfil(Convert.ToInt32(usuarioId));

            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest(sucesso);
            }
        }
        //[HttpGet]
        //[Authorize]
        //public IActionResult actionResult()
        //{
        //    var identidade = (ClaimsIdentity)HttpContext.User.Identity;
        //    var status = identidade.FindFirst("status").Value;
        //    if (status == "2")
        //        return Unauthorized();
        //    DashboardService dashboard = new(_ctx);
        //    return Ok(dashboard.ObterAlunosBons());
        //}
        [HttpGet]
        [Route("ObterClassificacao")]
        [Authorize]
        public IActionResult ObterClassificacao()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status == "2")
                return Unauthorized();
            DashboardService dashboard = new(_ctx);
            return Ok(dashboard.ObterAlunosBonsNaSemana());
        }
        [HttpGet]
        [Route("ObterQtdComentarios")]
        [Authorize]
        public IActionResult ObterQtdComentarios()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;            
            DashboardService dashboard = new(_ctx);
            return Ok(new { qtd = dashboard.ObterQtdComentarios(Convert.ToInt32(usuarioId)) });
        }
    }
}
