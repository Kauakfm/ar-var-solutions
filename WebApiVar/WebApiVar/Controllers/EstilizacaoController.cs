using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.MetaData.Profiles.Icc;
using System.Security.Claims;
using WebApiVar.Application.Models;
using WebApiVar.Application.Services;
using WebApiVar.Repository;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstilizacaoController : ControllerBase
    {
        private readonly VAREntities _ctx;
        public EstilizacaoController(VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        //[Authorize]
        [Route("ObterInicio")]
        public IActionResult ObterInicio()
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            //  var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            //var status = identidade.FindFirst("status").Value;
            //if (status != "5")
            //  return Unauthorized();
            var sucesso = estilizacaoService.ObterTelaInicio();
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
        [Route("ObterHomeAluno")]
        public IActionResult ObterHomeAluno()
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var sucesso = estilizacaoService.ObterHomeAluno();
            if (sucesso.mensagem == "success")
            {
                return Ok(sucesso.response);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Authorize]
        [Route("AtualizarHomeAluno")]
        public IActionResult AtualizarHomeAluno([FromBody] HomeAlunoRequest request)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var sucesso = estilizacaoService.AtualizarHomeAluno(request);
            if (sucesso)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Authorize]
        [Route("EstilizarInicio")]

        public IActionResult atualizarInicio([FromForm] List<IFormFile> files, [FromForm] string urlVideo)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var estilizacaoService = new EstilizacaoService(_ctx);
            try
            {
                var fileModels = new List<FileUploadModel>();

                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        fileModels.Add(new FileUploadModel
                        {
                            FileName = formFile.FileName,
                            Data = memoryStream.ToArray()
                        });
                    }
                }

                var sucesso = estilizacaoService.atualizarInicio(fileModels, urlVideo);
                return Ok(sucesso);

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
        [Authorize]
        [Route("EstilizarQuemSomos")]
        public IActionResult atualizarQuemSomos([FromBody] QuemSomosRequest request)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            return Ok(estilizacaoService.atualizarQuemSomos(request));

        }
        [HttpPut]
        [Authorize]
        [Route("isAtivarQuemSomos")]
        public IActionResult isAtivarQuemSomos([FromBody] IsAtivoSecaoRequest request)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            return Ok(estilizacaoService.isAtivar(request));
        }

        [HttpPut]
        [Authorize]
        [Route("EstilizarNossoProposito")]
        public IActionResult atualizarNossoProposito([FromBody] NossoPropositoRequest request)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            return Ok(estilizacaoService.atualizarNossoProposito(request));

        }
        [HttpPut]
        [Authorize]
        [Route("isAtivarNossoProposito")]
        public IActionResult isAtivarNossoProposito([FromBody] IsAtivoSecaoRequest request)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            return Ok(estilizacaoService.isAtivarNossoProposito(request));
        }
        [HttpPut]
        [Authorize]
        [Route("EstilizarNossaHistoria")]
        public IActionResult atualizarNossaHistoria([FromBody] NossaHistoriaRequest request)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            return Ok(estilizacaoService.atualizarNossaHistoria(request));
        }

        [HttpPut]
        [Authorize]
        [Route("isAtivarNossaHistoria")]
        public IActionResult isAtivarNossaHistoria([FromBody] IsAtivoSecaoRequest request)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            return Ok(estilizacaoService.isAtivarNossaHistoria(request));
        }
        [HttpPut]
        [Authorize]
        [Route("AtualizarImagem1")]
        public IActionResult atualizarImagem1NossaHistoria(List<IFormFile> files)
        {

            var estilizacaoService = new EstilizacaoService(_ctx);
            try
            {
                var fileModels = new List<FileUploadModel>();

                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        fileModels.Add(new FileUploadModel
                        {
                            FileName = formFile.FileName,
                            Data = memoryStream.ToArray()
                        });
                    }
                }
                var identidade = (ClaimsIdentity)HttpContext.User.Identity;
                var status = identidade.FindFirst("status").Value;
                if (status != "5")
                    return Unauthorized();
                var sucesso = estilizacaoService.atualizarImagem1NossaHistoria(fileModels);
                return Ok(sucesso);

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
        [Authorize]
        [Route("atualizarImagem2")]
        public IActionResult atualizarImagem2NossaHistoria(List<IFormFile> files)
        {

            var estilizacaoService = new EstilizacaoService(_ctx);
            try
            {
                var fileModels = new List<FileUploadModel>();

                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        fileModels.Add(new FileUploadModel
                        {
                            FileName = formFile.FileName,
                            Data = memoryStream.ToArray()
                        });
                    }
                }
                var identidade = (ClaimsIdentity)HttpContext.User.Identity;
                var status = identidade.FindFirst("status").Value;
                if (status != "5")
                    return Unauthorized();
                var sucesso = estilizacaoService.atualizarImagem2NossaHistoria(fileModels);
                return Ok(sucesso);

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
        [Authorize]
        [Route("AtualizarImagem3")]
        public IActionResult atualizarImagem3NossaHistoria(List<IFormFile> files)
        {

            var estilizacaoService = new EstilizacaoService(_ctx);
            try
            {
                var fileModels = new List<FileUploadModel>();

                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        fileModels.Add(new FileUploadModel
                        {
                            FileName = formFile.FileName,
                            Data = memoryStream.ToArray()
                        });
                    }
                }
                var identidade = (ClaimsIdentity)HttpContext.User.Identity;
                var status = identidade.FindFirst("status").Value;
                if (status != "5")
                    return Unauthorized();
                var sucesso = estilizacaoService.atualizarImagem3NossaHistoria(fileModels);
                return Ok(sucesso);

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
        [Authorize]
        [Route("AtualizarImagem4")]
        public IActionResult atualizarImagem4NossaHistoria(List<IFormFile> files)
        {

            var estilizacaoService = new EstilizacaoService(_ctx);
            try
            {
                var fileModels = new List<FileUploadModel>();

                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        fileModels.Add(new FileUploadModel
                        {
                            FileName = formFile.FileName,
                            Data = memoryStream.ToArray()
                        });
                    }
                }
                var identidade = (ClaimsIdentity)HttpContext.User.Identity;
                var status = identidade.FindFirst("status").Value;
                if (status != "5")
                    return Unauthorized();
                var sucesso = estilizacaoService.atualizarImagem4NossaHistoria(fileModels);
                return Ok(sucesso);

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
        [Authorize]
        [Route("AtualizarCasos")]
        public IActionResult atualizarFotos(List<IFormFile>? files, [FromForm] string nome, [FromForm] string cargo, [FromForm] string idade, [FromForm] string linkedin, [FromForm] string texto, [FromForm] string numero)
        {
            var estilizacaoService = new EstilizacaoService(_ctx);
            try
            {
                var fileModels = new List<FileUploadModel>();

                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        fileModels.Add(new FileUploadModel
                        {
                            FileName = formFile.FileName,
                            Data = memoryStream.ToArray()
                        });
                    }
                }
                var identidade = (ClaimsIdentity)HttpContext.User.Identity;
                var status = identidade.FindFirst("status").Value;
                if (status != "5")
                    return Unauthorized();
                var sucesso = estilizacaoService.atualizarFotos(fileModels, new CasosSucessoRequest { nome = nome, cargo = cargo, idade = idade, linkedin = linkedin, numero = numero, texto = texto });
                return Ok(sucesso);

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
        [HttpGet]
        [Route("ObterCSS")]
        public IActionResult ObterCSS()
        {
            
            var estilizacaoService = new EstilizacaoService(_ctx);
            var sucesso = estilizacaoService.ObterCSS();

            if (sucesso.mensagem == "success")
                return Ok(sucesso.response);
            else
                return BadRequest();

        }

        [HttpPut]
        [Route("AlterarLayoutFront/{id}")]
        public IActionResult AlterarLayoutFront(int id)
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var estilizacaoService = new EstilizacaoService(_ctx);
            var sucesso = estilizacaoService.AlterarLayoutFront(id);
            if (sucesso == true)
                return NoContent();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("ObterTodosEstilosCSS")]
        public IActionResult ObterTodosEstilosCSS()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            if (status != "5")
                return Unauthorized();
            var estilizacaoService = new EstilizacaoService(_ctx);
            var estilos = estilizacaoService.ObterTodosEstilosCSS();
            if (estilos != null)
                return Ok(estilos);
            else
                return BadRequest();
        }
    }
}
