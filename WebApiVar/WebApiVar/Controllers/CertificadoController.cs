using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApiVar.Application.Models;
using WebApiVar.Application.Services;
using WebApiVar.Repository;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using WebApiVar.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CertificadoController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly VAREntities _ctx;

        public CertificadoController(IWebHostEnvironment webHostEnv, VAREntities context)
        {
            _webHostEnv = webHostEnv;
            _ctx = context;
        }
        /*[HttpPost]
        [Route("CriarRelatorio")]
        public IActionResult CriarRelatorio(int codigo)
        {
            //string filePath = "C:/inetpub/wwwroot/certificados/arquivofrx/";
            //Directory.CreateDirectory(filePath);
            //var caminhoProducao = Path.Combine(filePath, "ReportMvc.frx");

            string pastaDestino = @"C:\Users\Kaua\Documents\arquivofrx";
            Directory.CreateDirectory(pastaDestino);
            var caminhoRelatorioo = Path.Combine(pastaDestino, "ReportMvc.frx");
            if (!File.Exists(caminhoRelatorioo)) 
            { 
            var freport = new FastReport.Report();
            var certificadoService = new CertificadoService(_ctx);
            var certificado = certificadoService.PegarCertificado(codigo);
            var listar = new List<CertificadoAluno> { certificado };

            freport.Dictionary.RegisterBusinessObject(listar, "listaUsuario", 10, true);
            //freport.Report.Save(caminhoProducao);
            freport.Report.Save(caminhoRelatorioo);
            }
            return Ok($" Relatorio gerado : {caminhoRelatorioo}");
        }

         [HttpGet]
         [Route("RelatorioUsuarios")]
         public IActionResult RelatorioUsuarios(int id)
         {
             //string filePath = "C:/inetpub/wwwroot/certificados/";
             //var caminhoCerficadoProducao = Path.Combine(filePath, "ReportMvc.frx");
            // if (!Directory.Exists("C:/inetpub/wwwroot/certificados/certificado_Aluno"))
             //{
               //  Directory.CreateDirectory("C:/inetpub/wwwroot/certificados/certificado_Aluno");
             //}


             string pastaDestino = @"C:\Users\Kaua\Documents\arquivofrx";
             var caminhoRelatorio = Path.Combine(pastaDestino, "ReportMvc.frx");

             var freport = new FastReport.Report();
             var productList = new UsuarioService(_ctx);
             //var listar = productList.PegarUsuarios(id);
             var aluno = new CertificadoAluno
             {
                 codigo = 1,
                 nomeAluno = "Kaua Ferreira Martins",
                 descricao = "Full Stack com carga de 900 horas",
                 urlAssinatura = "https://www.freepnglogos.com/uploads/signature-png/signatures-download-clipart-29.png",
                 nomeAssinatura = "Pedro Alvares Cabral"
             };
             var listar = new List<CertificadoAluno>();
             listar.Add(aluno);  

             freport.Report.Load(caminhoRelatorio);
             freport.Dictionary.RegisterBusinessObject(listar, "listaUsuario", 10, true);
             freport.Prepare();

             var pdfExport = new PDFSimpleExport();
             using MemoryStream ms = new MemoryStream();

             pdfExport.Export(freport, ms);
             ms.Flush();

             string dataFormatada = DateTime.Now.ToString("yyyyMMdd_HHmmss");
             string nomeDoArquivo = $"{id}_{dataFormatada}.pdf";
            // TabCertificadosAlunos salvarCertificado = new TabCertificadosAlunos();

            // _ctx.tabCertificadosAlunos.Add(salvarCertificado);
           //  _ctx.SaveChanges();

             string caminhoSalvamento = Path.Combine(pastaDestino, nomeDoArquivo);
             using (FileStream fileStream = new FileStream(caminhoSalvamento, FileMode.Create))
             {
                 ms.Seek(0, SeekOrigin.Begin);
                 ms.CopyTo(fileStream);
             }
             return File(ms.ToArray(), "application/pdf");
         }
        */

        [HttpGet]
        [Route("RelatorioUsuarios")]
        public async Task<IActionResult> RelatorioUsuarios(int id)
        {
            var service = new CertificadoService(_ctx); 
            byte[] pdfBytes = await service.GerarCertificadoPDF(id);

            return File(pdfBytes.ToArray(), "application/pdf");
        }

        [HttpGet]
        [Authorize]
        [Route("ObterCertificados")]

        public IActionResult ObterCertificados()
        {
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var status = identidade.FindFirst("status").Value;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            if (status != "1" && status != "5")
                return Unauthorized();
            var admin = new CertificadoService(_ctx);
            var response = admin.ObterCertificadosAlunos(Convert.ToInt32(usuarioId));

            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(response);
            }
        }





    }
}
