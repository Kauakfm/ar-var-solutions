using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;


namespace WebApiVar.Services
{
    public class EmailService
    {
        public void EnviaEmailParametrizavelInstituto(string nomeRemetente, string emailDestinatario, string assuntoMensagem, string conteudoMensagem)
        {
            var porta = 587;
            var smtp = "smtp.titan.email";
            var isSSL = false;
            var usuario = "contato@instituto.varsolutions.com.br";
            var senha = "Teste01&";

            var objEmail = new MailMessage(usuario, emailDestinatario, assuntoMensagem, conteudoMensagem);

            objEmail.From = new MailAddress(nomeRemetente + "<" + usuario + ">");
            objEmail.IsBodyHtml = true;
            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            objEmail.Subject = assuntoMensagem;
            objEmail.Body = conteudoMensagem;

            using (var objSmtp = new SmtpClient(smtp, porta))
            {
                objSmtp.EnableSsl = isSSL;
                objSmtp.UseDefaultCredentials = false;
                objSmtp.Credentials = new NetworkCredential(usuario, senha);

                try
                {
                    objSmtp.Send(objEmail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO EM MANDAR EMAIL - Instituto | ERRO: "+ ex.ToString());
                }
                finally
                {
                    objEmail.Dispose();
                }
            }
        }
        public void EnviaEmailParametrizavelVar(string nomeRemetente, string emailDestinatario, string assuntoMensagem, string conteudoMensagem, string emailCopiaOculta = "sistema@varsolutions.com.br")
        {
            var porta = 587;
            var smtp = "smtp.titan.email";
            var isSSL = false;
            var usuario = "contato@varsolutions.com.br";
            var senha = "Teste01&";

            var objEmail = new MailMessage(usuario, emailDestinatario, assuntoMensagem, conteudoMensagem);

            objEmail.From = new MailAddress(nomeRemetente + "<" + usuario + ">");
            objEmail.IsBodyHtml = true;
            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            objEmail.Subject = assuntoMensagem;
            objEmail.Body = conteudoMensagem;
            objEmail.Bcc.Add(new MailAddress(emailCopiaOculta));     

            using (var objSmtp = new SmtpClient(smtp, porta))
            {
                objSmtp.EnableSsl = isSSL;
                objSmtp.UseDefaultCredentials = false;
                objSmtp.Credentials = new NetworkCredential(usuario, senha);

                try
                {
                    objSmtp.Send(objEmail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO EM MANDAR EMAIL - Var | ERRO: " + ex.ToString());
                }
                finally
                {
                    objEmail.Dispose();
                }
            }
        }

        public void EnviaEmailComAnexo(string nomeRemetente, string emailDestinatario, string assuntoMensagem, string conteudoMensagem, string caminhoAnexo)
        {
            var porta = 587;
            var smtp = "smtp.titan.email";
            var isSSL = false;
            var usuario = "contato@instituto.varsolutions.com.br";
            var senha = "Teste01&";

            var objEmail = new MailMessage(usuario, emailDestinatario, assuntoMensagem, conteudoMensagem);

            objEmail.From = new MailAddress(nomeRemetente + "<" + usuario + ">");
            objEmail.IsBodyHtml = true;
            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            objEmail.Subject = assuntoMensagem;
            objEmail.Body = conteudoMensagem;

            // Adicione o anexo
            Attachment anexo = new Attachment(caminhoAnexo);
            objEmail.Attachments.Add(anexo);

            using (var objSmtp = new SmtpClient(smtp, porta))
            {
                objSmtp.EnableSsl = isSSL;
                objSmtp.UseDefaultCredentials = false;
                objSmtp.Credentials = new NetworkCredential(usuario, senha);

                try
                {
                    objSmtp.Send(objEmail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO EM MANDAR EMAIL | ERRO: " + ex.ToString());
                }
                finally
                {
                    objEmail.Dispose();
                }
            }
        }        
    }
}