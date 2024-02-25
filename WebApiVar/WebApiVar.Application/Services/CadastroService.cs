using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository.Models;
using WebApiVar.Repository;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApiVar.Application.Models;
using WebApiVar.Services;
using System.Security.Cryptography;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net.Http;
using System.IO;
using teste = System.Drawing;
using System.Drawing;
using System.Globalization;

namespace WebApiVar.Application.Services
{
    public class CadastroService
    {
        private readonly decimal renda_por_pessoa_gratuito = 700;
        private readonly decimal renda_por_pessoa_nivel_2 = 900;
        private readonly decimal renda_por_pessoa_nivel_3 = 1100;
        private readonly decimal renda_por_pessoa_nivel_4 = 1300;

        private readonly VAREntities _ctx;

        public CadastroService(VAREntities context)
        {
            _ctx = context;
        }
        public bool EnviarEmailDeRedefinirSenha(string email)
        {
            try
            {
                EmailService emailService = new EmailService();
                var user = _ctx.TabUsuario.FirstOrDefault(x => x.email == email);
                if (user == null)
                    return false;


                var pathTemplate = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/RedefinirSenha.html";
                var html = System.IO.File.ReadAllText(pathTemplate);

                var htmlreformado = html.Replace("@codigo", criptografarID(user.codigo));
                htmlreformado = htmlreformado.Replace("@nome", user.nome.Split(" ")[0]);

                emailService.EnviaEmailParametrizavelInstituto("Var Solutions", user.email, "Redefinição de senha", htmlreformado);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool RedefinirSenha(string token, RedefinirSenhaRequest request)
        {
            var id = descriptografarID(token);
            try
            {
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                usuario.senha = request.senha;
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SalvarInscricao(InscricaoRequest request)
        {
            EmailService emailService = new EmailService();
            TabUsuario novousuario = new TabUsuario()
            {

                nome = request.nome,
                email = request.email,
                senha = request.senha,
                dataCriacao = DateTime.Now,
                dataNascimento = request.dataNascimento,
                cep = request.CEP,
                rua = request.rua,
                bairro = request.bairro,
                cidade = request.cidade,
                turmaCodigo = request.turmaCodigo,
                celular = request.celular,
                descricao = request.descricao,
                status = 3
            };
            try
            {
                if (_ctx.TabUsuario.Any(x => x.email == novousuario.email))
                    return false;
                var usuario = _ctx.TabUsuario.Add(novousuario);
                _ctx.SaveChanges();

                var pathTemplate = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Email.html";
                var html = System.IO.File.ReadAllText(pathTemplate);
                var htmlreformado = html.Replace("@codigo", usuario.Entity.codigo.ToString());

                emailService.EnviaEmailParametrizavelInstituto("Var Solutions", novousuario.email, "Verificação de email", html);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CadastroUsuario(CadastroRequest request)
        {
            try
            {
                EmailService emailService = new EmailService();
                TabUsuario novoUsuario = new TabUsuario()
                {
                    nome = request.nome,
                    email = request.email,
                    senha = request.senha,
                    dataCriacao = DateTime.Now,
                    isAluno = (false),
                    isEmail = (false),
                    status = 4
                };
                if (_ctx.TabUsuario.Any(x => x.email == novoUsuario.email))
                    return false;
                var usuario = _ctx.TabUsuario.Add(novoUsuario);
                _ctx.SaveChanges();

                var pathTemplate = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Email.html";
                var html = System.IO.File.ReadAllText(pathTemplate);

                var htmlreformado = html.Replace("@codigo", criptografarID(usuario.Entity.codigo));

                emailService.EnviaEmailParametrizavelInstituto("Var Solutions", novoUsuario.email, "Verificação de email", htmlreformado);


                return true;

            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo CadastroUsuario na CadastroService");

                return false;
            }
        }

        public int ValidarEmail(string token)
        {
            try
            {
                StringBuilder result = new StringBuilder();

                foreach (char c in token)
                {
                    if (!char.IsLetter(c))
                    {
                        result.Append(c);
                    }
                }

                token = result.ToString();
                var id = (Convert.ToInt32(token) + 34) / 362;
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                if (usuario.isEmail)
                    return 0;
                usuario.isEmail = true;
                _ctx.TabUsuario.Update(usuario);
                _ctx.SaveChanges();
                return usuario.codigo;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public CadastroResponse InscreverPresencial(InscricaoPresencialRequest request)
        {
            var response = new CadastroResponse();

            try
            {
                List<TabTurma> Turmas = _ctx.TabTurma.ToList();
                EmailService emailService = new EmailService();
                List<TabUsuario> usuario = _ctx.TabUsuario.ToList();
                List<TabPosicaoAluno> posicoes = _ctx.TabPosicaoAluno.ToList();
                AutenticacaoService aut = new AutenticacaoService(_ctx);
                TabUsuario codUser = new TabUsuario();

                codUser.nome = request.Username;
                codUser.senha = request.Password;
                codUser.email = request.Email;
                codUser.dataCriacao = DateTime.Now;
                codUser.inscricaoPresencial = DateTime.Now;
                codUser.dataNascimento = request.dataNascimento;
                codUser.cep = request.CEP;
                codUser.rua = request.rua;
                codUser.bairro = request.bairro;
                codUser.cidade = request.cidade;
                codUser.celular = request.celular;
                codUser.RG = request.RG;
                codUser.CPF = request.CPF;
                codUser.genero = request.genero;
                codUser.cor = request.Cor;
                codUser.UF = request.UF;
                codUser.haveNote = request.haveNote;
                codUser.haveInternetHouse = request.haveInternetHouse;
                codUser.descricao = request.descricao;
                codUser.turmaCodigo = request.turmaCodigo;
                codUser.complemento = request.complemento;
                codUser.numeroCasa = request.numeroCasa;


                if (_ctx.TabUsuario.Any(x => x.email == request.Email))
                {
                    response.sucesso = false;
                    response.mensagem = "Email já existe no banco.";
                    return response;
                }
                if (_ctx.TabUsuario.Any(x => x.CPF == request.CPF))
                {
                    response.sucesso = false;
                    response.mensagem = "CPF já existe no banco.";
                    return response;
                }


                DateTime dataAtual = DateTime.Now;
                DateTime dataNascimento = request.dataNascimento;

                int idade = dataAtual.Year - dataNascimento.Year;

                if (dataNascimento > dataAtual.AddYears(-idade))
                {
                    idade--;
                }

                bool maiorDe16Anos = idade >= 16;

                if (!maiorDe16Anos)
                {
                    response.sucesso = false;
                    response.mensagem = "A idade minima permitida é de 16 anos";
                    return response;
                }
                string rg = request.RG;
                if (rg.Length <= 4)
                {
                    response.sucesso = false;
                    response.mensagem = "Minimo de caracteres permitidos do RG é 5";
                    return response;
                }
                string celular = request.celular;
                if (celular.Length != 11)
                {
                    response.sucesso = false;
                    response.mensagem = "Minimo de caracteres permitidos do celular é 11";
                    return response;
                }
                string cpf = request.CPF;
                if (cpf.Length > 11)
                {
                    response.sucesso = false;
                    response.mensagem = "Minimo de caracteres permitidos do CPF é 11";
                    return response;

                }
                string numcasa = request.numeroCasa;
                if (numcasa.Length > 5)
                {
                    response.sucesso = false;
                    response.mensagem = "O maximo de caracteres permitidos é 5";
                }
                codUser.status = 3;
                codUser.isEmail = true;

                var user = _ctx.TabUsuario.Add(codUser);
                _ctx.SaveChanges();
                response.sucesso = true;
                response.mensagem = "Dados atualizados com sucesso.";
                RendaFamiliar(request.renda, request.pessoas, user.Entity.codigo);

                var turma = ObterVagas().FirstOrDefault(x => x.codigoTurma == request.turmaCodigo);
                string textoDisponibilidade;
                var posicao = usuario.Where(x => (x.status == 3 || x.status == 6) && x.turmaCodigo == request.turmaCodigo && (x.inscricaoPresencial <= codUser.inscricaoPresencial || x.inscricaoPresencial == null)).Count(); textoDisponibilidade = $"Você está cadastrado na nossa lista de espera. Sua posição é: {(turma.qtdEspera + turma.qtdConvocados) * 1}.";

                if (turma.Vagas > 0 && turma.qtdEspera == 0)
                {
                    /*
                    var atual = DateTime.Now.Date;
                    var diaAula = Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).diaAula;
                    var diaAulaSemana = diaAula.Split('-').First();
                    var myDay = ToDayOfWeek(diaAulaSemana);
                    int offset = CalculateOffset(atual.DayOfWeek, myDay);
                    var datainicio = atual.AddDays(offset);

                    while (FeriadosBrasileiros.IsHoliday(datainicio))
                    {
                        datainicio = datainicio.AddDays(7);
                    }
                    textoDisponibilidade = $"Sua data de inicio será: {datainicio.ToString("dd/MM/yyyy")}.";
                    */
                    var baia = usuario.Where(x => x.turmaCodigo == request.turmaCodigo && x.status == 1).Select(y => y.id_Posicao).ToList();
                    var baialivres = _ctx.TabPosicaoAluno.Where(x => !baia.Any(y => y == x.id) && x.isNoteVar == true).ToList().First();
                    codUser.id_Posicao = baialivres.id;
                    var hoje = DateTime.Now;
                    var telContat = _ctx.tabUnidade.FirstOrDefault(x => x.codigo == 1);
                    var telinst = telContat.telContato;
                    var Template = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Convocacao.html";
                    var htmlTemplate = System.IO.File.ReadAllText(Template);
                    var htmlArrumado = htmlTemplate
                    .Replace("@Nome", codUser.nome)
                    .Replace("@DataInicioPrazo", hoje.ToString("dd/MM/yyyy"))
                    .Replace("@DataLimiteApresentacao", hoje.AddDays(7).ToString("dd/MM/yyyy"))
                    .Replace("@Turma", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).descricao)
                    .Replace("@NumeroContato", telinst)
                    // .Replace("@DataInicio", textoDisponibilidade)
                    // .Replace("@Baia", baialivres.descricao);
                    //.Replace("@Horario", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).horario)
                    .Replace("@Endereco", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).localizacao);
                    emailService.EnviaEmailParametrizavelInstituto("Var Solutions", codUser.email, "Realizar Matrícula", htmlArrumado);
                    //var lugaresocupados = usuario.Where(x => x.turmaCodigo == codUser.turmaCodigo).Select(y => y.id_Posicao).ToList();
                    //var lugarvazio = posicoes.Where(x => !lugaresocupados.Any(y => y == x.id)).ToList().First().id;
                    //codUser.id_Posicao = lugarvazio;
                    codUser.status = 6;
                    codUser.unidadeCodigo = 1;
                    //InserirPlano(new PlanoRequest { codigousuario = codUser.codigo, diaPagamento = request.diaPagamento, mensalidade = Convert.ToInt32(codUser.mensalidade) });
                    codUser.dataEmailEnviado = DateTime.Now;
                    _ctx.TabUsuario.Update(codUser);
                    _ctx.SaveChanges();
                    var response1 = aut.GerarTokenporId(codUser.codigo);
                    response.nome = response1.nome;
                    response.status = response1.status;
                    response.token = response1.token;
                    response.urlFoto = response1.urlFoto;
                    response.isEmail = response1.isEmail;
                    response.usuarioid = response1.usuarioid;
                }
                else
                {
                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/InscricaoPresencial1.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    var html = htmlTemplat.Replace("@Nome", codUser.nome).Replace("@Turma", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).descricao).Replace("@Posicao", posicao.ToString());
                    emailService.EnviaEmailParametrizavelInstituto("Var Solutions", codUser.email, "Sua posição na lista de espera", html);
                    codUser.unidadeCodigo = 1;
                    _ctx.TabUsuario.Update(codUser);
                    _ctx.SaveChanges();
                    var response1 = aut.GerarTokenporId(codUser.codigo);
                    response.nome = response1.nome;
                    response.status = response1.status;
                    response.token = response1.token;
                    response.urlFoto = response1.urlFoto;
                    response.isEmail = response1.isEmail;
                    response.usuarioid = response1.usuarioid;
                }
                return response;

            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo CadastroBolsa na CadastroService");

                response.sucesso = false;
                response.mensagem = "Não foi possivel atualizar os dados.";
                return response;
            }
        }
        public CadastroResponse CadastroBolsa(BolsaRequest request, int id)
        {
            var response = new CadastroResponse();

            try
            {
                List<TabTurma> Turmas = _ctx.TabTurma.ToList();
                EmailService emailService = new EmailService();
                List<TabUsuario> usuario = _ctx.TabUsuario.ToList();
                List<TabPosicaoAluno> posicoes = _ctx.TabPosicaoAluno.ToList();
                AutenticacaoService aut = new AutenticacaoService(_ctx);

                var codUser = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                if (codUser == null)
                {
                    response.sucesso = false;
                    response.mensagem = "Id do usuario não encontrado.";
                    return response;
                }
                codUser.inscricaoPresencial = DateTime.Now;
                codUser.dataNascimento = request.dataNascimento;
                codUser.cep = request.CEP;
                codUser.rua = request.rua;
                codUser.bairro = request.bairro;
                codUser.cidade = request.cidade;
                codUser.celular = request.celular;
                codUser.RG = request.RG;
                codUser.CPF = request.CPF;
                codUser.genero = request.genero;
                codUser.cor = request.Cor;
                codUser.UF = request.UF;
                codUser.haveNote = request.haveNote;
                codUser.haveInternetHouse = request.haveInternetHouse;
                codUser.descricao = request.descricao;
                codUser.turmaCodigo = request.turmaCodigo;
                codUser.complemento = request.complemento;
                codUser.numeroCasa = request.numeroCasa;


                if (_ctx.TabUsuario.Any(x => x.codigo != id && x.CPF == request.CPF))
                {
                    response.sucesso = false;
                    response.mensagem = "CPF já existe no banco.";
                    return response;
                }


                DateTime dataAtual = DateTime.Now;
                DateTime dataNascimento = request.dataNascimento;

                int idade = dataAtual.Year - dataNascimento.Year;

                if (dataNascimento > dataAtual.AddYears(-idade))
                {
                    idade--;
                }

                bool maiorDe16Anos = idade >= 16;

                if (!maiorDe16Anos)
                {
                    response.sucesso = false;
                    response.mensagem = "A idade minima permitida é de 16 anos";
                    return response;
                }
                string rg = request.RG;
                if (rg.Length <= 4)
                {
                    response.sucesso = false;
                    response.mensagem = "Minimo de caracteres permitidos do RG é 5";
                    return response;
                }
                string celular = request.celular;
                if (celular.Length != 11)
                {
                    response.sucesso = false;
                    response.mensagem = "Minimo de caracteres permitidos do celular é 11";
                    return response;
                }
                string cpf = request.CPF;
                if (cpf.Length > 11)
                {
                    response.sucesso = false;
                    response.mensagem = "Minimo de caracteres permitidos do CPF é 11";
                    return response;

                }
                string numcasa = request.numeroCasa;
                if (numcasa.Length > 5)
                {
                    response.sucesso = false;
                    response.mensagem = "O maximo de caracteres permitidos é 5";
                }
                codUser.status = 3;

                _ctx.TabUsuario.Update(codUser);
                _ctx.SaveChanges();
                response.sucesso = true;
                response.mensagem = "Dados atualizados com sucesso.";
                RendaFamiliar(request.renda, request.pessoas, id);
                var turma = ObterVagas().FirstOrDefault(x => x.codigoTurma == request.turmaCodigo);
                string textoDisponibilidade;
                var posicao = usuario.Where(x => (x.status == 3 || x.status == 6) && x.turmaCodigo == request.turmaCodigo && (x.inscricaoPresencial <= codUser.inscricaoPresencial || x.inscricaoPresencial == null)).Count();
                textoDisponibilidade = $"Você está cadastrado na nossa lista de espera. Sua posição é: {(turma.qtdEspera + turma.qtdConvocados) * 1}.";

                if (turma.Vagas > 0 && turma.qtdEspera == 0)
                {
                    /*
                    var atual = DateTime.Now.Date;
                    var diaAula = Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).diaAula;
                    var diaAulaSemana = diaAula.Split('-').First();
                    var myDay = ToDayOfWeek(diaAulaSemana);
                    int offset = CalculateOffset(atual.DayOfWeek, myDay);
                    var datainicio = atual.AddDays(offset);

                    while (FeriadosBrasileiros.IsHoliday(datainicio))
                    {
                        datainicio = datainicio.AddDays(7);
                    }
                    textoDisponibilidade = $"Sua data de inicio será: {datainicio.ToString("dd/MM/yyyy")}.";
                    */
                    var baia = usuario.Where(x => x.turmaCodigo == request.turmaCodigo && x.status == 1).Select(y => y.id_Posicao).ToList();
                    var baialivres = _ctx.TabPosicaoAluno.Where(x => !baia.Any(y => y == x.id) && x.isNoteVar == true).ToList().First();
                    codUser.id_Posicao = baialivres.id;
                    var hoje = DateTime.Now;
                    var telContat = _ctx.tabUnidade.FirstOrDefault(x => x.codigo == 1);
                    var telinst = telContat.telContato;
                    var Template = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Convocacao.html";
                    var htmlTemplate = System.IO.File.ReadAllText(Template);
                    var htmlArrumado = htmlTemplate
                    .Replace("@Nome", codUser.nome)
                    .Replace("@DataInicioPrazo", hoje.ToString("dd/MM/yyyy"))
                    .Replace("@DataLimiteApresentacao", hoje.AddDays(7).ToString("dd/MM/yyyy"))
                    .Replace("@Turma", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).descricao)
                    .Replace("@NumeroContato", telinst)
                    // .Replace("@DataInicio", textoDisponibilidade)
                    // .Replace("@Baia", baialivres.descricao);
                    //.Replace("@Horario", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).horario)
                    .Replace("@Endereco", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).localizacao);
                    emailService.EnviaEmailParametrizavelInstituto("Var Solutions", codUser.email, "Realizar Matrícula", htmlArrumado);
                    //var lugaresocupados = usuario.Where(x => x.turmaCodigo == codUser.turmaCodigo).Select(y => y.id_Posicao).ToList();
                    //var lugarvazio = posicoes.Where(x => !lugaresocupados.Any(y => y == x.id)).ToList().First().id;
                    //codUser.id_Posicao = lugarvazio;
                    codUser.status = 6;
                    codUser.unidadeCodigo = 1;
                    //InserirPlano(new PlanoRequest { codigousuario = codUser.codigo, diaPagamento = request.diaPagamento, mensalidade = Convert.ToInt32(codUser.mensalidade) });
                    codUser.dataEmailEnviado = DateTime.Now;
                    _ctx.TabUsuario.Update(codUser);
                    _ctx.SaveChanges();
                    var response1 = aut.GerarTokenporId(codUser.codigo);
                    response.nome = response1.nome;
                    response.status = response1.status;
                    response.token = response1.token;
                    response.urlFoto = response1.urlFoto;
                    response.isEmail = response1.isEmail;
                    response.usuarioid = response1.usuarioid;
                }
                else
                {
                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/InscricaoPresencial1.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    var html = htmlTemplat.Replace("@Nome", codUser.nome).Replace("@Turma", Turmas.FirstOrDefault(x => x.codigo == request.turmaCodigo).descricao).Replace("@Posicao", posicao.ToString());
                    emailService.EnviaEmailParametrizavelInstituto("Var Solutions", codUser.email, "Sua posição na lista de espera", html);
                    codUser.unidadeCodigo = 1;
                    _ctx.TabUsuario.Update(codUser);
                    _ctx.SaveChanges();
                    var response1 = aut.GerarTokenporId(codUser.codigo);
                    response.nome = response1.nome;
                    response.status = response1.status;
                    response.token = response1.token;
                    response.urlFoto = response1.urlFoto;
                    response.isEmail = response1.isEmail;
                    response.usuarioid = response1.usuarioid;
                }
                return response;
            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo CadastroBolsa na CadastroService");

                response.sucesso = false;
                response.mensagem = "Não foi possivel atualizar os dados.";
                return response;
            }
        }

        private DayOfWeek ToDayOfWeek(string diaSemana)
        {
            switch (diaSemana)
            {
                case "Sunday":
                    return DayOfWeek.Sunday;
                case "Tuesday":
                    return DayOfWeek.Tuesday;
                case "Wednesday":
                    return DayOfWeek.Wednesday;
                case "Thursday":
                    return DayOfWeek.Thursday;
                case "Friday":
                    return DayOfWeek.Friday;
                case "Saturday":
                    return DayOfWeek.Saturday;
                default:
                    return DayOfWeek.Monday;
            }
        }

        public int CalculateOffset(DayOfWeek current, DayOfWeek desired)
        {
            int c = (int)current;
            int d = (int)desired;
            int offset = (7 - c + d) % 7;
            return offset == 0 ? 7 : offset;
        }

        public bool InserirPlano(PlanoRequest request)
        {
            try
            {

                var plano = _ctx.TabPlano.Any(x => x.usuarioCodigo == request.codigousuario);
                if (plano)
                    return false;
                var plan = new TabPlano()
                {
                    usuarioCodigo = request.codigousuario,
                    diaInicializacao = DateTime.Now,
                    dataEncerramento = DateTime.Now.AddMonths(10),
                    status = request.status
                };
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public MensalidadeResponse RendaFamiliar(decimal renda, int pessoas, int id)
        {
            var mensal = new MensalidadeResponse();
            try
            {
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                if (usuario == null)
                {
                    mensal.sucesso = false;
                    mensal.mensagem = "usuario não encontrado";
                    return mensal;
                }

                decimal rendaPorPessoa = renda / pessoas;

                if (rendaPorPessoa <= renda_por_pessoa_gratuito)
                {
                    mensal.mensalidade = 1;
                }
                else if (rendaPorPessoa <= renda_por_pessoa_nivel_2)
                {
                    mensal.mensalidade = 2;
                }
                else if (rendaPorPessoa <= renda_por_pessoa_nivel_3)
                {
                    mensal.mensalidade = 3;
                }
                else if (rendaPorPessoa <= renda_por_pessoa_nivel_4)
                {
                    mensal.mensalidade = 4;
                }
                else
                {
                    mensal.mensalidade = 5;
                }
                usuario.rendaFamiliar = renda;
                usuario.qntdPessoas = pessoas;
                usuario.mensalidade = mensal.mensalidade;
                _ctx.TabUsuario.Update(usuario);
                _ctx.SaveChanges();
                mensal.sucesso = true;
                mensal.mensagem = "Dados adionados com sucesso.";
                return mensal;

            }
            catch (Exception ex)
            {
                mensal.sucesso = false;
                mensal.mensagem = "deu errado";
                return mensal;
            }
        }
        public bool AlterarInscricao(InscricaoRequest usuarioatualizado, int id)
        {

            if (_ctx.TabUsuario.Any(x => x.email == usuarioatualizado.email))
                return false;

            var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);

            try
            {

                if (usuarioatualizado.email != null)
                    usuario.email = usuarioatualizado.email;
                if (usuarioatualizado.senha != null)
                    usuario.senha = usuarioatualizado.senha;
                if (usuarioatualizado.nome != null)
                    usuario.nome = usuarioatualizado.nome;
                if (usuarioatualizado.turmaCodigo != null)
                {
                    if (usuarioatualizado.turmaCodigo != usuario.turmaCodigo)
                    {
                        usuario.turmaCodigo = usuarioatualizado.turmaCodigo;
                        usuario.dataCriacao = DateTime.Now;
                    }
                }
                if (usuarioatualizado.dataNascimento != null)
                    usuario.dataNascimento = usuarioatualizado.dataNascimento;
                if (usuarioatualizado.CEP != null)
                    usuario.cep = usuarioatualizado.CEP;
                if (usuarioatualizado.rua != null)
                    usuario.rua = usuarioatualizado.rua;
                if (usuarioatualizado.bairro != null)
                    usuario.bairro = usuarioatualizado.bairro;
                if (usuarioatualizado.cidade != null)
                    usuario.cidade = usuarioatualizado.cidade;
                if (usuarioatualizado.celular != null)
                    usuario.celular = usuarioatualizado.celular;


                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void AprovarMatricula(int id)

        {
            var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
            usuario.isAluno = true;
            usuario.status = 1;
            _ctx.SaveChanges();
        }
        public void RejeitarMatricula(int id)
        {
            var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
            usuario.isAluno = false;
            usuario.status = 4;
            _ctx.SaveChanges();

        }
        public List<Vw_SituacaoAtualVagasPorTurma> ObterVagas()
        {
            try
            {
                return _ctx.Vw_SituacaoAtualVagasPorTurma.ToList();

                //var usuarios = _ctx.Vw_SituacaoAtualVagasPorTurma.ToList();
                //var turmas = _ctx.TabTurma.Where(x => x.isAtivo == true && x.codUnidade == 1).ToList();
                //List<VagasModel> vagas = new List<VagasModel>();
                //foreach (var item in turmas)
                //{
                //    VagasModel vaga = new VagasModel();

                //    var unidade = _ctx.tabUnidade.FirstOrDefault(x => x.codigo == item.codUnidade);
                //    var usuariosTurma = usuarios.Where(x => (x.turmaCodigo == item.codigo && x.status == 1 || x.status == 6 && x.id_Posicao != null) ||
                //    (x.turmaCodigo == item.codigo && x.status == 6 && x.id_Posicao != null)).ToList();
                //    var posicoes = _ctx.TabPosicaoAluno.Where(c => usuariosTurma.Select(y => y.id_Posicao).ToList().Contains(c.id));
                //    var qtdAtivo = posicoes.Where(X => X.isNoteVar).Count();

                //    vaga.Qtd = unidade.qtdNote - qtdAtivo;
                //    vaga.Turma = item.descricao;
                //    vaga.IdTurma = item.codigo;
                //    vaga.espera = usuarios.Where(x => x.turmaCodigo == item.codigo && x.status == 3).Count();
                //    vaga.hasnote = false;
                //    vagas.Add(vaga);
                //}
                //return vagas;
            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo ObterVagas na CadastroService");

                return null;
            }

        }

        /*
            public List<EsperaModel> ObterListaEspera()
               { 
            var usuarios = _ctx.TabUsuario.ToList();
            var turmas = _ctx.TabTurma.ToList();
            List<EsperaModel> espera = new List<EsperaModel>();
            foreach (var item in turmas)
            {
                EsperaModel esperas = new EsperaModel();
                esperas.QtdEspera = usuarios.Where(x => x.turmaCodigo == item.codigo && x.status == 3).Count();
                esperas.turma = item.descricao;
                esperas.idTurma = item.codigo;
                espera.Add(esperas);
            }
            return espera;
          
        }
        */
        public TabUsuario ObterUsuario(int id)
        {
            try
            {
                return _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string criptografarID(int id)
        {
            var id1 = ((id * 362) - 34).ToString();
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            char[] characters = id1.ToCharArray();


            var retorno = "";

            foreach (var letra in characters)
            {
                Random random = new Random();

                int index = random.Next(0, alphabet.Length);

                retorno += alphabet[index] + letra.ToString();
            }
            return retorno;
        }
        private int descriptografarID(string token)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in token)
            {
                if (!char.IsLetter(c))
                {
                    result.Append(c);
                }
            }
            token = result.ToString();
            var id = (Convert.ToInt32(token) + 34) / 362;
            return id;
        }
        public async Task<List<string>> InserirFoto(List<FileUploadModel> files, int id)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("Nenhum arquivo foi fornecido para upload.");

            var fotosSalvas = new List<string>();

            foreach (var file in files)
            {
                if (file.Data.Length > 0)
                {
                    file.FileName = id.ToString() + $".{file.FileName.Split(".")[1]}";
                    // Salvar a foto em um diretório (por exemplo, "Fotos") e obter o caminho completo do arquivo salvo
                    string filePath = "C:/inetpub/wwwroot/images/image_Aluno/" + file.FileName;

                    // Verificar se o diretório "Fotos" existe e criar se não existir
                    if (!Directory.Exists("C:/inetpub/wwwroot/images/image_Aluno"))
                    {
                        Directory.CreateDirectory("C:/inetpub/wwwroot/images/image_Aluno");
                    }

                    // Salvar o arquivo no diretório
                    await File.WriteAllBytesAsync(filePath, file.Data);
                    var user = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                    user.urlFoto = file.FileName;   
                    _ctx.SaveChanges();


                    fotosSalvas.Add(filePath);
                }
            }

            return fotosSalvas;
        }
    }
}