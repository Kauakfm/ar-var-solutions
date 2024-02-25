using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Xml;

using WebApiVar.Application.Models;
using WebApiVar.Application.Services;
using WebApiVar.Repository;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursoController : Controller
    {
        private readonly VAREntities _ctx;
        public CursoController(VAREntities ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("aulasCursoPorId/{id}")]
        [Authorize]
        public IActionResult ObterAulasDoCurso([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var aulas = aulaService.ObterAulasDoCurso(id);
            if (aulas == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(aulas);
            }

        }
        [HttpGet]
        [Route("obterAulaId/{id}")]
        [Authorize]
        public IActionResult ObterAula([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var aula = aulaService.ObterAula(id);
            if (aula == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(aula);
            }

        }

        [HttpGet]
        [Route("obterPrimeiraAula/{cursoId}")]
        [Authorize]
        public IActionResult ObterPrimeiraAula([FromRoute] int cursoId)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var aula = aulaService.ObterPrimeiraAula(cursoId);
            if (aula == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(aula);
            }

        }
        [HttpGet]
        [Route("obterMaterialId/{id}")]
        [Authorize]
        public IActionResult ObterMaterial([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var materialService = new MaterialService(_ctx);
            var material = materialService.ObterMaterial(id);
            if (material == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(material);
            }
        }
        [HttpGet]
        [Route("MarcarConcluido/{aulaId}")]
        [Authorize]
        public IActionResult MarcarConcluido(int aulaId)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.MarcarConcluido(aulaId, Convert.ToInt32(usuarioId));
            if (sucesso.status)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("obterConcluidos")]
        [Authorize]
        public IActionResult ObterConcluidos()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status == "2")
                return Unauthorized();

            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var aulaService = new AulaService(_ctx);
            var concluidos = aulaService.ObterConcluidos(Convert.ToInt32(usuarioId));
            if (concluidos == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(concluidos);
            }
        }
        [HttpGet]
        [Route("obterComentariosPorAula/{aulanum}")]
        [Authorize]
        public IActionResult VerComentarios([FromRoute] int aulanum)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var comentarioService = new ComentarioService(_ctx);
            var comentario = comentarioService.VerComentarios(aulanum);
            if (comentario != null)
            {
                return Ok(comentario);
            }
            else
            {

                return BadRequest();
            }
        }
        [HttpPost]
        [Route("SalvarComentarios")]
        [Authorize]
        public IActionResult SalvarComentarios([FromBody] ComentarioRequest request)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var comentarioService = new ComentarioService(_ctx);
            var comentario = comentarioService.SalvarComentario(request);
            if (comentario == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(comentario);
            }
        }
        [HttpGet]
        [Route("ObterNomeAula/{id}")]
        public IActionResult ObterNomeAula([FromRoute] int id)
        {
            var aulaService = new AulaService(_ctx);
            var cursoamostra = aulaService.ObterNomeAulaOnline(id);
            if (cursoamostra == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(cursoamostra);
            }
        }
        [HttpGet]
        [Route("obterAulaIniciada/{id}")]
        [Authorize]
        public IActionResult obterAulaIniciada([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var aulaService = new AulaService(_ctx);
            aulaService.obterAulaIniciada(Convert.ToInt32(usuarioId), id);
            return Ok();
        }
        [HttpPost]
        [Route("MarcarSegundosAssistidos")]
        [Authorize]
        public IActionResult MarcarSegundosAssistidos([FromBody] MarcarSegundosRequest request)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.MarcarSegundosAssistidos(Convert.ToInt32(usuarioId), request);
            if (sucesso)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("ObterSegundosAssistidos/{id}")]
        [Authorize]
        public IActionResult ObterSegundosAssistidos([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.ObterSegundosAssistidos(Convert.ToInt32(usuarioId), id);
            if (sucesso != null)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }

        //[HttpPost]
        //[Route("InserirAula/{id}")]
        //[Authorize]
        //public IActionResult InserirAula([FromBody] List<AulaRequest> requests, [FromRoute] int id)   
        //{
        //    var identidade = (ClaimsIdentity)HttpContext.User.Identity;
        //    var status = identidade.FindFirst("status").Value;
        //    if (status != "5")
        //        return Unauthorized();
        //   var aulaService = new AulaService(_ctx);
        //    var sucesso = aulaService.InserirAula(requests, id);
        //    if (sucesso != null)
        //    {
        //        return Ok(sucesso);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        [Route("InserirCurso")]
        [Authorize]
        public IActionResult InserirCurso([FromBody] CursoRequest request)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var cursoService = new CursoService(_ctx);
            var sucesso = cursoService.InserirCurso(request);
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
        [Route("InserirModulo")]
        [Authorize]
        public IActionResult InserirModulo([FromBody] ModuloRequest request)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.InserirModulo(request);
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
        [Route("ObterModulos")]
        [Authorize]
        public IActionResult ObterModulos()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.ObterModulos();
            if (sucesso.mensagem == "sucess")
            {
                return Ok(sucesso.response);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("ObterCursos")]
        [Authorize]
        public IActionResult ObterCursos()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var cursoService = new CursoService(_ctx);
            var sucesso = cursoService.ObterTodosCursos();
            if (sucesso.mensagem == "sucess")
            {
                return Ok(sucesso.response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("InserirMaterial/{id}")]
        [Authorize]
        public async Task<IActionResult> InserirMaterial(List<IFormFile> files, [FromRoute] int id)
        {
            var cursoService = new CursoService(_ctx);
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

                var fotosSalvas = await cursoService.InserirMaterial(fileModels, id);
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

        [HttpPut]
        [Route("InserirImgFormCurso/{id}")]
        [Authorize]
        public async Task<IActionResult> InserirImgFormCurso(List<IFormFile> files, [FromRoute] int id)
        {
            var cursoService = new CursoService(_ctx);
           
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

                var fotosSalvas = await cursoService.InserirImgFormCurso(fileModels, id);
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
        [Route("InserirAula")]
        [Authorize]
        public IActionResult InserirAula([FromBody] AulaRequest requests)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.InserirAula(requests);
            if (sucesso == true)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("VincularAulaCurso")]
        [Authorize]
        public IActionResult VincularAulaCurso([FromBody] TabCursoTabAulaRequest request)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.VincularAulaCurso(request);
            if (sucesso == true)
            {
                return Ok(sucesso);
            }
            else
            {
                return BadRequest();
            }
        }

        

        [HttpGet]
        [Route("ObterTodasAulas")]
        [Authorize]
        public IActionResult ObterTodasAulas()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.ObterTodasAulas();
            if (sucesso.mensagem == "success")
                return Ok(sucesso.response);
            else
                return BadRequest();
        }  
        [HttpGet]
        [Route("ObterQtdAulas/{id}")]
        [Authorize]
        public IActionResult ObterQtdAulas([FromRoute] int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var aulaService = new AulaService(_ctx);
            var sucesso = aulaService.ObterQtdAulas(id);
            if (sucesso.mensagem == "success")
                return Ok(new { qtdAulas = sucesso.response });
            else
                return BadRequest();
        }
    }
}
