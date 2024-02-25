using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using WebApiVar.Services;

namespace WebApiVar.Application.Services
{
    public class CertificadoService
    {
        private readonly VAREntities _ctx;

        public CertificadoService(VAREntities context)
        {
            _ctx = context;
        }

        public async Task<byte[]> GerarCertificadoPDF(int codigo)
        {
            EmailService emailService = new EmailService();
            CriarCertificado(codigo);
            string pastaDestino = @"C:/inetpub/wwwroot/certificados/arquivofrx/";
            var caminhoRelatorio = Path.Combine(pastaDestino, "ReportMvc.frx");

            var freport = new FastReport.Report();
            var certificado = PegarCertificado(codigo);
            var listar = new List<CertificadoAluno> { certificado };

            var codigoAlunoCertificado = _ctx.tabCertificadosAlunos.FirstOrDefault(x => x.codigo == codigo);
            codigoAlunoCertificado.urlCertificado = certificado.codigoCertificado;
            _ctx.tabCertificadosAlunos.Update(codigoAlunoCertificado);
            _ctx.SaveChanges();

            freport.Report.Load(caminhoRelatorio);
            freport.Dictionary.RegisterBusinessObject(listar, "listaUsuario", 10, true);
            freport.Prepare();

            var pdfExport = new PDFSimpleExport();
            using MemoryStream ms = new MemoryStream();

            pdfExport.Export(freport, ms);
            ms.Flush();

            string nomeDoArquivo = $"{certificado.codigo}_{certificado.usuarioCodigo}.pdf";

            string caminhoSalvamento = Path.Combine(pastaDestino, nomeDoArquivo);
            using (FileStream fileStream = new FileStream(caminhoSalvamento, FileMode.Create))
            {
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(fileStream);
            }
            var codUser = certificado.usuarioCodigo;
            var usuarioEmail = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == codUser);
            var caminhoAnexo = $"C:/inetpub/wwwroot/certificados/arquivofrx/{nomeDoArquivo}";

            var Template = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Certificado.html";
            var htmlTemplate = System.IO.File.ReadAllText(Template);
            var htmlArrumado = htmlTemplate;
            emailService.EnviaEmailComAnexo("Var Solutions", usuarioEmail.email, "Conclusão de Curso", htmlArrumado, caminhoAnexo);

            return ms.ToArray();
        }


        public CertificadoAluno PegarCertificado(int codigo)
        {
            try
            {
                var certificadoAluno = _ctx.tabCertificadosAlunos.FirstOrDefault(x => x.codigo == codigo);
                var user = _ctx.TabUsuario.FirstOrDefault(c => c.codigo == certificadoAluno.usuarioCodigo);
                var modulos = _ctx.tabmodulosCertificados.FirstOrDefault(x => x.codigo == certificadoAluno.modulosCerficadosCodigo);
                DateTime data = DateTime.Now;
                string dataFormatada = data.ToString("dd/MM/yyyy");
                var aluno = new CertificadoAluno
                {
                    codigo = certificadoAluno.codigo,
                    usuarioCodigo = user.codigo,
                    nomeAluno = user.nome,
                    descricao = modulos.descricao,
                    dataConclusao = dataFormatada,
                    urlAssinatura = modulos.urlAssinatura,
                    nomeAssinatura = modulos.nomeAssinatura,
                    codigoCertificado = $"https://api.varsolutions.com.br/certificados/arquivofrx/{certificadoAluno.codigo}_{user.codigo}.pdf"
                };
                return aluno;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void CriarCertificado(int codigo)
        {
            try
            {
                string pastaDestino = @"C:/inetpub/wwwroot/certificados/arquivofrx/";
                Directory.CreateDirectory(pastaDestino);
                var caminhoRelatorioo = Path.Combine(pastaDestino, "ReportMvc.frx");
                if (!File.Exists(caminhoRelatorioo))
                {
                    var freport = new FastReport.Report();
                    var certificadoService = new CertificadoService(_ctx);
                    var certificado = certificadoService.PegarCertificado(codigo);
                    var listar = new List<InfoCertificados>
                    {
                        new InfoCertificados
                        {
                            codigo = certificado.codigo,
                            codigoCertificado = certificado.codigoCertificado,
                            dataConclusao = certificado.dataConclusao,
                            descricao = certificado.descricao,
                            nomeAluno = certificado.nomeAluno,
                            nomeAssinatura = certificado.nomeAssinatura,
                            urlAssinatura = certificado.urlAssinatura,
                            usuarioCodigo = certificado.usuarioCodigo

                        }
                    };

                    freport.Dictionary.RegisterBusinessObject(listar, "listaUsuario", 10, true);
                    //freport.Report.Save(caminhoProducao);
                    freport.Report.Save(caminhoRelatorioo);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ObterCertificados> ObterCertificadosAlunos(int codigo)
        {
            try
            {
                List<ObterCertificados> certi = new List<ObterCertificados>();
                var tabCertificados = _ctx.tabCertificadosAlunos.Where(c => c.usuarioCodigo == codigo).ToList();                
                var objNome = _ctx.TabUsuario.FirstOrDefault(c => c.codigo == codigo)?.nome;
                foreach (var item in tabCertificados)
                {
                    var objModulo = _ctx.tabmodulosCertificados.FirstOrDefault(c => c.codigo == item.modulosCerficadosCodigo);
                    certi.Add(new ObterCertificados
                    {
                        nome = objNome,
                        modulo = objModulo?.descricao,      
                        nomeModulo = objModulo?.nomeModulo,                        
                        urlCertificado = item.urlCertificado,                        
                        dataConclusao = item.dataGeracao
                    });

                }
                return certi;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
