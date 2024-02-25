using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks.Dataflow;
using WebApiVar.Application.Models;
using WebApiVar.Application.Services;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CadastroController : ControllerBase
    {
        private readonly VAREntities _ctx;
        public CadastroController(VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        [Route("ObterVagas")]
        public IActionResult ObterVagas()
        {
            var cadastro = new CadastroService(_ctx);
            var response = cadastro.ObterVagas();

            return response != null ? Ok(response) : BadRequest();
        }
        /*
        [HttpGet]
        [Route("ObterEspera")]
        [Authorize]
        public IActionResult ObterListaEspera()
        {
            var cadastro = new CadastroService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status == "3")
                return Unauthorized();

            return Ok(cadastro.ObterListaEspera());
        }
        */
        [HttpGet]
        [Route("VerStatus")]
        [Authorize]
        public TabUsuario VerStatus()
        {
            var cadastro = new CadastroService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var usuario = cadastro.ObterUsuario(Convert.ToInt32(usuarioId));

            return usuario;
        }
        [HttpPost]
        [Route("SalvarInscricao")]
        public IActionResult SalvarInscricao([FromBody] InscricaoRequest request)
        {
            var cadastro = new CadastroService(_ctx);
            var sucesso = cadastro.SalvarInscricao(request);

            if (sucesso)
                return NoContent();
            else
                return BadRequest();
        }
        [HttpGet]
        [Route("ValidarEmail/{id}")]
        public IActionResult ValidarEmail([FromRoute] string id)
        {
            var cadastro = new CadastroService(_ctx);
            var autenticacao = new AutenticacaoService(_ctx);
            var usuarioid = cadastro.ValidarEmail(id);
            if (usuarioid == 0)
                return BadRequest();
            var token = autenticacao.GerarTokenporId(usuarioid);
            return Ok(token);
        }
        [HttpPost]
        [Route("CadastrarUsuario")]
        public IActionResult CadastrarUsuario([FromBody] CadastroRequest request)
        {
            var cadastroService = new CadastroService(_ctx);
            var sucesso = cadastroService.CadastroUsuario(request);
            if (sucesso)
                return NoContent();
            else
                return BadRequest();
        }
        [HttpPut]  //atualizar dados
        [Authorize]
        public IActionResult CadastroBolsa([FromBody] BolsaRequest request)
        {
            var cadastroService = new CadastroService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var status = identidade.FindFirst("status").Value;
            if (status != "4")
                return Unauthorized();
            var sucesso = cadastroService.CadastroBolsa(request, Convert.ToInt32(usuarioId));

            if (sucesso == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(sucesso);
            }
        } 
        [HttpPost]  //atualizar dados
        [Route("InscreverPresencial")]
        public IActionResult InscreverPresencial([FromBody] InscricaoPresencialRequest request)
        {
            var cadastroService = new CadastroService(_ctx);

            var sucesso = cadastroService.InscreverPresencial(request);

            if (sucesso == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(sucesso);
            }
        }
        [HttpPost]
        [Route("AlterarInscricao")]
        [Authorize]
        public IActionResult AlterarInscricao([FromBody] InscricaoRequest request)
        {
            var cadastro = new CadastroService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var sucesso = cadastro.AlterarInscricao(request, Convert.ToInt32(usuarioId));

            if (sucesso)
                return NoContent();
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("Atualizarfoto")]
        [Authorize]
        public async Task<IActionResult> InserirFoto(List<IFormFile> files)
        {         
            var cadastroService = new CadastroService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;

            try
            {
                var fileModels = new List<FileUploadModel>();

                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);
                        fileModels.Add(new FileUploadModel
                        {
                            FileName = formFile.FileName,
                            Data = memoryStream.ToArray()
                        });
                    }
                }

                var fotosSalvas = await cadastroService.InserirFoto(fileModels, Convert.ToInt32(usuarioId));
                return Ok(fotosSalvas);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao salvar as fotos.");
            }           
        }
        [HttpPost]
        [Route("EnviarEmailDeRedefinirSenha")]
        public IActionResult EnviarEmailDeRedefinirSenha([FromBody] EmailRequest request)
        {
            var cadastroService = new CadastroService(_ctx);
            var sucesso = cadastroService.EnviarEmailDeRedefinirSenha(request.Email);
            if (sucesso)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(sucesso);
            }

        }
        [HttpPost]
        [Route("EsqueciSenha/{id}")]
        public IActionResult RedefinirSenha([FromRoute] string id, [FromBody] RedefinirSenhaRequest request)
        {
            var cadastroService = new CadastroService(_ctx);
            var sucesso = cadastroService.RedefinirSenha(id, request);
            if (sucesso)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(sucesso);
            }

        }
        [HttpPut]
        [Route("mensalidade")]
        [Authorize]
        public IActionResult RendaFamiliar([FromBody] RendaRequest request)
        {
            var cadastroService = new CadastroService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var sucesso = cadastroService.RendaFamiliar(request.renda, request.pessoas, Convert.ToInt32(usuarioId));
            if (sucesso.sucesso)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest(sucesso);
            }
        }
    }
}