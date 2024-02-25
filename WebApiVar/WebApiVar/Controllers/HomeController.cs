using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using WebApiVar.Application.Models;
using WebApiVar.Application.Services;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly VAREntities _ctx;
        public HomeController(VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        [Authorize]
        [Route("ObterCursos/")]
        public IActionResult ObterCursos()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var curso = new CursoService(_ctx);
            return Ok(curso.ObterCursos(Convert.ToInt32(unidadecodigo)));
        }

        [HttpGet]
        [Authorize]
        [Route("ObterProgresso/")]
        public IActionResult ObterProgresso()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var curso = new CursoService(_ctx);
            var cursos = curso.ObterCursos(Convert.ToInt32(unidadecodigo));
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            int[] percentualcurso = new int[cursos.Count()];
            var c = 0;
            foreach (var item in cursos)
            {
                percentualcurso[c] = curso.ObterPercentualConclusao(item.codigo, Convert.ToInt32(usuarioId)).evolucao;
                c++;

            }

            return Ok(percentualcurso);
        }

        [HttpGet]
        [Authorize]
        [Route("ObterAvisos")]
        public IActionResult ObterAvisos()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status == "2")
                return Unauthorized();

            var avisoService = new AvisosService(_ctx);
            return Ok(avisoService.ObterAvisos(Convert.ToInt32(unidadecodigo)));
        }

        [HttpGet]
        [Authorize]
        [Route("obterPerguntas")]
        public IActionResult ObterPerguntas()
        {
            var identitdade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identitdade.FindFirst("status").Value;
            var unidadecodigo = identitdade.FindFirst("unidadeId").Value;
            var usuarioCodigo = identitdade.FindFirst("usuarioId").Value;
            if ((status != "1" && status != "5") || unidadecodigo != "1")
                return Ok(new { mensagem = "Nenhuma Pergunta Disponível" });

            var avisoService = new AvisosService(_ctx);
            return Ok(avisoService.ObterPerguntas(Convert.ToInt32(usuarioCodigo)));
        }
        [HttpPost]
        [Authorize]
        [Route("MarcarPerguntas")]
        public IActionResult MarcarPerguntas([FromBody] RespostaPesquisaRequest request)
        {

            var identitdade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identitdade.FindFirst("status").Value;
            var unidadecodigo = identitdade.FindFirst("unidadeId").Value;
            var usuarioCodigo = identitdade.FindFirst("usuarioId").Value;
            if ((status != "1" && status != "5") || unidadecodigo != "1") 
                return Unauthorized();

            var avisoService = new AvisosService(_ctx);
            if (avisoService.MarcarPerguntas(request, Convert.ToInt32(usuarioCodigo)))
                return NoContent();
            else
                return BadRequest();

        }

    }
}
