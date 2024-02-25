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
    public class AdministrativoController : Controller
    {
        private readonly VAREntities _ctx;
        public AdministrativoController(VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        [Authorize]
        [Route("ObterListaEspera/{id?}")]
        public IActionResult ObterListaEspera(int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            return Ok(admin.ObterListaEspera(Convert.ToInt32(id)));
        }
        [HttpPost]
        [Authorize]
        [Route("ObterListaPresenca")]
        public IActionResult ObterListaPresenca([FromBody] ProcRequest request)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var administrativoService = new AdministrativoService(_ctx);
            return Ok(administrativoService.ObterListaPresenca(request.turmaCodigo, request.data));


        }
        [HttpPost]
        [Authorize]
        [Route("AprovarAlunoListaEspera/{id}")]

        public IActionResult AprovarMatricula(int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            var response = admin.AprovarMatricula(Convert.ToInt32(id));

            if (!response.sucesso)
            {
                return BadRequest(new { mensagem = response.mensagem });
            }

            return Ok(response);
        }
        [HttpPost]
        [Authorize]
        [Route("ReprovarAlunoListaEspera/{id}")]

        public IActionResult ReprovarMatricula(int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            var response = admin.RejeitarMatricula(Convert.ToInt32(id));

            if (!response.sucesso)
            {
                return BadRequest(new { mensagem = response.mensagem });
            }

            return Ok(response);
        }
        [HttpGet]
        [Authorize]
        [Route("ObterDetalhesAluno/{id}")]

        public IActionResult VerDetalhes(int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            return Ok(admin.DetalhesUsuario(Convert.ToInt32(id)));

        }
        [HttpPut]
        [Authorize]
        [Route("editarUsuario/{id}")]

        public IActionResult EditarUsuario([FromBody] EditarUsuarioRequest request, int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            return Ok(admin.EditarUsuario(request, Convert.ToInt32(id)));

        }
        [HttpGet]
        [Authorize]
        [Route("ObterVagasTurma/{turmaCodigo}/{userId?}")]

        public IActionResult ObterVagasTurma(int turmaCodigo, int? userId)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            return Ok(admin.ObterPosicoesDisponiveisPorTurma(Convert.ToInt32(turmaCodigo), userId));

        }
        [HttpGet]
        [Authorize]
        [Route("ObterVagas")]

        public IActionResult ObterVagas(int turmaCodigo)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            return Ok(admin.ObterVagas());
        }
        [HttpGet]
        [Authorize]
        [Route("ObterTurmas")]

        public IActionResult ObterTurmas(int turmaCodigo)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            return Ok(admin.ObterTurmas());
        }
        [HttpGet]
        [Authorize]
        [Route("ObterUser/{id}")]

        public IActionResult ObterUsuario(int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var unidadecodigo = identidade.FindFirst("unidadeId").Value;
            if (status != "5")
                return Unauthorized();
            var admin = new AdministrativoService(_ctx);
            return Ok(admin.ObterEditarUsuario(Convert.ToInt32(id)));

        }
        [HttpGet]
        [Authorize]
        [Route("ObterAdministrativo/{id}")]
        public IActionResult ObterAdministrativo(int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.ObterPerfilAdminisrtativo(Convert.ToInt32(id));
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
        [Authorize]
        [Route("ObterPrimeiroPagamento/{id}")]
        public IActionResult ObterPrimeiroPagamento(int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.ObterPrimeiroPagamento(Convert.ToInt32(id));
            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet]
        [Authorize]
        [Route("ObterConvocados/{id?}")]
        public IActionResult ObterConvocados([FromRoute] int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.ObterConvocados(id);
            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Authorize]
        [Route("VoltarParaListaEspera/{id}")]
        public IActionResult VoltarParaListaEspera([FromRoute] int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.VoltarParaListaEspera(id);
            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize]
        [Route("MensagemEnviada/{id}")]
        public IActionResult MensagemEnviada([FromRoute] int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.MensagemEnviada(id);
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
        [Authorize]
        [Route("ObterPagantes/{id?}")]
        public IActionResult ObterPagantes([FromRoute] int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.ObterPagantes(id);
            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Authorize]
        [Route("DarBaixa/{id}")]
        public IActionResult DarBaixa([FromBody] DarBaixaRequest request, int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.DarBaixa(request, id);
            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("InserirUnidade")]
        public IActionResult InserirUnidade([FromBody] UnidadeRequest request)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var unidadeAdicionada = administrativoService.InserirUnidade(request);
            if (unidadeAdicionada.status)
            {
                return Ok(unidadeAdicionada);
            }
            else
            {
                return BadRequest(unidadeAdicionada);
            }
        }

        [HttpGet]
        [Route("BuscarUnidades")]
        public IActionResult BuscarUnidades()
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var unidades = administrativoService.BuscarUnidades();
            if (unidades == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(unidades);
            }
        }

        [HttpPut]
        [Route("AtualizarUnidade/{id}")]
        public IActionResult AtualizarUnidade([FromBody] UnidadeRequest request, [FromRoute] int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var unidadeAtualizada = administrativoService.AtualizarUnidade(request, id);
            if (unidadeAtualizada.status)
            {
                return Ok(unidadeAtualizada);
            }
            else
            {
                return BadRequest(unidadeAtualizada);
            }
        }

        [HttpDelete]
        [Route("DeletarUnidade/{id}")]
        public IActionResult DeletarUnidade([FromRoute] int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var unidadeRemovida = administrativoService.DeletarUnidade(id);
            if (unidadeRemovida.status)
            {
                return Ok(unidadeRemovida);
            }
            else
            {
                return BadRequest(unidadeRemovida);
            }
        }

        [HttpPost]
        [Route("VincularCursoUnidade")]
        public IActionResult VincularCursoUnidade([FromBody] VinculoRequest request)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var unidadeVinculada = administrativoService.VincularCursoUnidade(request);
            if (unidadeVinculada == true)
            {
                return NoContent();
            }
            else
            {

                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("DesvincularCursoUnidade/{id}")]
        public IActionResult DesvincularCursoUnidade([FromRoute] int id)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var unidadeDesvinculada = administrativoService.DesvincularCursoUnidade(id);
            if (unidadeDesvinculada == true)
            {
                return NoContent();
            }
            else
            {

                return BadRequest();
            }
        }

        [HttpGet]
        [Route("BuscarUnidadesVinculadas")]
        public IActionResult BuscarUnidadesVinculadas()
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var unidadesVinculadas = administrativoService.BuscarUnidadesVinculadas();
            if (unidadesVinculadas != null)
            {
                return Ok(unidadesVinculadas);
            }
            else
            {

                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize]
        [Route("DarPresenca")]
        public IActionResult DarPresenca([FromBody] AlunoPresencaRequest request)
        {
            var administrativoService = new AdministrativoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = administrativoService.DarPresenca(request);
            if (sucesso)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
