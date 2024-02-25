using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;
using WebApiVar.Services;

using static WebApiVar.Application.Models.DoacaoRequest.Payments;

namespace WebApiVar.Application.Services
{
    public class PagamentoService
    {
        private readonly VAREntities _ctx;
        public PagamentoService(VAREntities ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> RealizarDoacao(DoacaoRequest doacao)
        {
            EmailService emailService = new EmailService();
            try
            {
                var options = new RestClientOptions("https://api.pagar.me/core/v5/orders");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Basic c2tfSnJlb1lqS1RXM1N4T2FuTTo="); //Producao
                //request.AddHeader("authorization", "Basic c2tfdGVzdF9yNjRsREdaczhqSThPZHoxOg=="); // Teste
                request.AddJsonBody(JsonConvert.SerializeObject(doacao), false);

                var response = await client.PostAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = JsonConvert.DeserializeObject<DoacaoResponse>(response.Content);
                    if (content.Status != "paid")
                        return false;

                    _ctx.TabDoacao.Add(new TabDoacao
                    {
                        nome = doacao.customer.name,
                        cpf = doacao.customer.document,
                        dataTrans = DateTime.Now,
                        email = doacao.customer.email,
                        valor = doacao.items.First().amount
                    });
                    _ctx.SaveChanges();
                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Doacao.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    decimal valorFormatado = doacao.items.First().amount / 100;
                    string valorFormatadoString = valorFormatado.ToString("C");
                    var html = htmlTemplat.Replace("@Nome", doacao.customer.name).Replace("@Valor", valorFormatadoString).Replace("@FormaPag", doacao.payments.First().payment_method);
                    emailService.EnviaEmailParametrizavelInstituto("Var Solutions", doacao.customer.email, "Doação realizada com sucesso!", html);
                    return true;


                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo RealizarDoacao na pagamentoService");
                return false;
            }
        }
        public async Task<TabUsuario> ComprarCursoOnline(DoacaoRequest doacao, int id, string cupom)
        {
            EmailService emailService = new EmailService();
            try
            {
                var desconto = 0;
                if (cupom != null)
                {
                    if (_ctx.TabCupomCursoOnline.Any(x => x.cupom == cupom && x.dataVencimento >= DateTime.Now))
                        desconto = _ctx.TabCupomCursoOnline.FirstOrDefault(x => x.cupom == cupom && x.dataVencimento >= DateTime.Now).valorCupomPorcento;
                    else
                        return null;
                }


                doacao.items.First().amount -= (doacao.items.First().amount / 100) * desconto;

                var options = new RestClientOptions("https://api.pagar.me/core/v5/orders");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Basic c2tfTGRuNEpBWW9IbHVySnJFWjo="); //Producao
                //request.AddHeader("authorization", "Basic c2tfdGVzdF9xbE5uRWdFc2R1V1ZvWDJkOg=="); // Teste
                request.AddJsonBody(JsonConvert.SerializeObject(doacao), false);

                var response = await client.PostAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = JsonConvert.DeserializeObject<DoacaoResponse>(response.Content);
                    if (content.Status != "paid")
                        return null;

                    tabComprasCurso compracurso = new()
                    {
                        data = DateTime.Now,
                        usuarioCodigo = id,
                        parcelas = doacao.payments.First().credit_card.installments,
                        tipo = 1
                    };

                    _ctx.tabComprasCurso.Add(compracurso);
                    var user = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                    if (user == null)
                        return null;

                    user.status = 1;
                    user.unidadeCodigo = 3;
                    user.isEmail = true;
                    _ctx.SaveChanges();

                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/CursoOnline.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    string formpag;
                    switch (doacao.payments.First().payment_method)
                    {
                        case "credit_card":
                            formpag = "Cartão de Crédito";
                            break;
                        case "boleto":
                            formpag = "Boleto";
                            break;
                        case "voucher":
                            formpag = "Voucher";
                            break;
                        case "bank_transfer":
                            formpag = "Transferência bancária";
                            break;
                        case "pix":
                            formpag = "Pix";
                            break;
                        default:
                            formpag = " ";
                            break;
                    }
                    var html = htmlTemplat.Replace("@Nome", doacao.customer.name).Replace("@FormaPag", formpag).Replace("@tempo", "1 ano").Replace("@tabela", "");

                    emailService.EnviaEmailParametrizavelVar("Var Solutions", doacao.customer.email, "Compra realizada com sucesso!", html);
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<CursoOnlineResponse> ComprarECadastrarCursoOnline(DoacaoRequest doacao, CadastroRequest cadastroRequest, string cupom, int compraHistoricoCodigo)
        {
            EmailService emailService = new EmailService();

            try
            {
                var desconto = 0;
                if (cupom != null)
                {
                    if (_ctx.TabCupomCursoOnline.Any(x => x.cupom == cupom && x.dataVencimento >= DateTime.Now))
                    {
                        desconto = _ctx.TabCupomCursoOnline.FirstOrDefault(x => x.cupom == cupom && x.dataVencimento >= DateTime.Now).valorCupomPorcento;
                        IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Cupom validado, inserido desconto : " + desconto);

                    }
                    else
                    {
                        IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Tentou um cupom invalido: " + cupom);
                        return null;
                    }
                }

                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.email.ToLower() == cadastroRequest.email.ToLower());
                if (usuario != null && usuario.status == 1)
                    return new CursoOnlineResponse
                    {
                        mensagem = "Email já existe",
                        user = null
                    };

                doacao.items.First().amount -= (doacao.items.First().amount / 100) * desconto;

                if (doacao.items.First().amount <= 0)
                {
                    TabUsuario novoUsuario = new TabUsuario()
                    {
                        nome = cadastroRequest.nome,
                        email = cadastroRequest.email,
                        senha = cadastroRequest.senha,
                        dataCriacao = DateTime.Now,
                        isAluno = (false),
                        status = 1,
                        unidadeCodigo = 3,
                        isEmail = true,
                        codigo = usuario != null ? usuario.codigo : 0
                    };
                    IncluirDetalhesHistorico(compraHistoricoCodigo, JsonConvert.SerializeObject(novoUsuario), "Preparando o objeto para cadastro do usuário");

                    _ctx.TabUsuario.Update(novoUsuario);

                    _ctx.SaveChanges();



                    tabComprasCurso compracurso = new()
                    {
                        data = DateTime.Now,
                        usuarioCodigo = novoUsuario.codigo,
                        parcelas = doacao.payments.First().credit_card.installments,
                        tipo = 1
                    };

                    _ctx.tabComprasCurso.Add(compracurso);
                    _ctx.SaveChanges();

                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/CursoOnline.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    string formpag;
                    switch (doacao.payments.First().payment_method)
                    {
                        case "credit_card":
                            formpag = "Cartão de Crédito";
                            break;
                        case "boleto":
                            formpag = "Boleto";
                            break;
                        case "voucher":
                            formpag = "Voucher";
                            break;
                        case "bank_transfer":
                            formpag = "Transferência bancária";
                            break;
                        case "pix":
                            formpag = "Pix";
                            break;
                        default:
                            formpag = " ";
                            break;
                    }
                    var html = htmlTemplat.Replace("@Nome", doacao.customer.name).Replace("@FormaPag", formpag).Replace("@tempo", "1 ano");


                    html = html.Replace("@tabela", "<table class=\"user\">\r\n                    <tr>\r\n                        <th>Usuário</th>\r\n                        <th>Senha</th>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>@Email</td>\r\n                        <td>@Senha</td>\r\n                    </tr>\r\n                </table>")
                        .Replace("@Email", novoUsuario.email)
                        .Replace("@Senha", novoUsuario.senha);
                    emailService.EnviaEmailParametrizavelVar("Var Solutions", doacao.customer.email, "Compra realizada com sucesso!", html, "contato@instituto.varsolutions.com.br");

                    return new CursoOnlineResponse
                    {
                        mensagem = "success",
                        user = novoUsuario
                    };
                }

                var options = new RestClientOptions("https://api.pagar.me/core/v5/orders");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");                
                request.AddHeader("authorization", "Basic c2tfTGRuNEpBWW9IbHVySnJFWjo="); //Producao
                //request.AddHeader("authorization", "Basic c2tfdGVzdF9xbE5uRWdFc2R1V1ZvWDJkOg=="); // Teste
                request.AddJsonBody(JsonConvert.SerializeObject(doacao), false);

                var response = await client.PostAsync(request);

                IncluirDetalhesHistorico(compraHistoricoCodigo, response.Content, "Retorno do pagar.me com o statusCode: " + response.StatusCode);

                var content = JsonConvert.DeserializeObject<DoacaoResponse>(response.Content);
                if (content == null || content.Status != "paid")
                {
                    IncluirDetalhesHistorico(compraHistoricoCodigo, JsonConvert.SerializeObject(doacao), "Request, requisição para o pagar.me para caso queira reprocessar pois gerou erro");
                    IncluirDetalhesHistorico(compraHistoricoCodigo, content.Status, "Vai retornar um erro para o cliente pois o status foi diferente de paid");
                    emailService.EnviaEmailParametrizavelVar("Var Solutions", "contato@varsolutions.com.br", "Tentativa de compra com problemas", "Nome: " + cadastroRequest.nome + " - email: " + cadastroRequest.email, "contato@instituto.varsolutions.com.br");
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    TabUsuario novoUsuario = new TabUsuario()
                    {
                        nome = cadastroRequest.nome,
                        email = cadastroRequest.email,
                        senha = cadastroRequest.senha,
                        dataCriacao = DateTime.Now,
                        isAluno = (false),
                        status = 1,
                        unidadeCodigo = 3,
                        isEmail = true,
                        codigo = usuario != null ? usuario.codigo : 0
                    };
                    IncluirDetalhesHistorico(compraHistoricoCodigo, JsonConvert.SerializeObject(novoUsuario), "Preparando o objeto para cadastro do usuário");

                    _ctx.TabUsuario.Update(novoUsuario);

                    _ctx.SaveChanges();



                    tabComprasCurso compracurso = new()
                    {
                        data = DateTime.Now,
                        usuarioCodigo = novoUsuario.codigo,
                        parcelas = doacao.payments.First().credit_card.installments,
                        tipo = 1
                    };

                    _ctx.tabComprasCurso.Add(compracurso);
                    _ctx.SaveChanges();

                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/CursoOnline.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    string formpag;
                    switch (doacao.payments.First().payment_method)
                    {
                        case "credit_card":
                            formpag = "Cartão de Crédito";
                            break;
                        case "boleto":
                            formpag = "Boleto";
                            break;
                        case "voucher":
                            formpag = "Voucher";
                            break;
                        case "bank_transfer":
                            formpag = "Transferência bancária";
                            break;
                        case "pix":
                            formpag = "Pix";
                            break;
                        default:
                            formpag = " ";
                            break;
                    }
                    var html = htmlTemplat.Replace("@Nome", doacao.customer.name).Replace("@FormaPag", formpag).Replace("@tempo", "1 ano");


                    html = html.Replace("@tabela", "<table class=\"user\">\r\n                    <tr>\r\n                        <th>Usuário</th>\r\n                        <th>Senha</th>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>@Email</td>\r\n                        <td>@Senha</td>\r\n                    </tr>\r\n                </table>")
                        .Replace("@Email", novoUsuario.email)
                        .Replace("@Senha", novoUsuario.senha);
                    emailService.EnviaEmailParametrizavelVar("Var Solutions", doacao.customer.email, "Compra realizada com sucesso!", html, "contato@instituto.varsolutions.com.br");

                    return new CursoOnlineResponse
                    {
                        mensagem = "success",
                        user = novoUsuario
                    };
                }
                else
                {
                    IncluirDetalhesHistorico(compraHistoricoCodigo, content.Status, "Vai retornar um erro para o cliente o status foi paid mas deu merda");
                    emailService.EnviaEmailParametrizavelVar("Var Solutions", "contato@varsolutions.com.br", "Tentativa de compra com problemas", "Nome: " + cadastroRequest.nome + " - email: " + cadastroRequest.email, "contato@instituto.varsolutions.com.br");
                    return null;
                }


            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo da PagamentoService chamando ComprarECadastrarOnline");
                return null;
            }
        }
        public async Task<CursoOnlineResponse> ComprarCursoOnlineMensal(PlanoPagarMeRequest requestPagarme, CadastroRequest cadastroRequest, int compraHistoricoCodigo)
        {
            EmailService emailService = new EmailService();

            try
            {
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.email.ToLower() == cadastroRequest.email.ToLower());
                if (usuario != null && usuario.status == 1)
                    return new CursoOnlineResponse
                    {
                        mensagem = "Email já existe",
                        user = null
                    };

                //var options = new RestClientOptions("https://api.pagar.me/core/v5/orders");
                //var client = new RestClient(options);
                //var request = new RestRequest("");
                //request.AddHeader("accept", "application/json");
                ////request.AddHeader("authorization", "Basic c2tfSnJlb1lqS1RXM1N4T2FuTTo="); //Producao Instituto
                ////request.AddHeader("authorization", "Basic c2tfTGRuNEpBWW9IbHVySnJFWjo="); //Producao Var
                //request.AddHeader("authorization", "Basic c2tfdGVzdF9xbE5uRWdFc2R1V1ZvWDJkOg=="); // Teste
                //request.AddJsonBody(JsonConvert.SerializeObject(requestPagarme), false);
                //var response = await client.PostAsync(request);

                var options = new RestClientOptions("https://api.pagar.me/core/v5/subscriptions");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Basic c2tfTGRuNEpBWW9IbHVySnJFWjo="); //Producao
                //request.AddHeader("authorization", "Basic c2tfdGVzdF9xbE5uRWdFc2R1V1ZvWDJkOg=="); // Teste
                request.AddJsonBody(JsonConvert.SerializeObject(requestPagarme), false);

                var response = await client.PostAsync(request);

                IncluirDetalhesHistorico(compraHistoricoCodigo, response.Content, "Retorno do pagar.me com o statusCode: " + response.StatusCode);

                var content = JsonConvert.DeserializeObject<DoacaoResponse>(response.Content);
                if (content == null || content.Status != "active")
                {
                    IncluirDetalhesHistorico(compraHistoricoCodigo, JsonConvert.SerializeObject(requestPagarme), "Request, requisição para o pagar.me para caso queira reprocessar pois gerou erro");
                    IncluirDetalhesHistorico(compraHistoricoCodigo, content.Status, "Vai retornar um erro para o cliente pois o status foi diferente de paid");
                    emailService.EnviaEmailParametrizavelVar("Var Solutions", "contato@varsolutions.com.br", "Tentativa de compra com problemas", "Nome: " + cadastroRequest.nome + " - email: " + cadastroRequest.email, "contato@instituto.varsolutions.com.br");
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    TabUsuario novoUsuario = new TabUsuario()
                    {
                        nome = cadastroRequest.nome,
                        email = cadastroRequest.email,
                        senha = cadastroRequest.senha,
                        dataCriacao = DateTime.Now,
                        isAluno = (false),
                        status = 1,
                        unidadeCodigo = 3,
                        isEmail = true,
                        codigo = usuario != null ? usuario.codigo : 0
                    };
                    IncluirDetalhesHistorico(compraHistoricoCodigo, JsonConvert.SerializeObject(novoUsuario), "Preparando o objeto para cadastro do usuário");

                    _ctx.TabUsuario.Update(novoUsuario);

                    _ctx.SaveChanges();



                    tabComprasCurso compracurso = new()
                    {
                        data = DateTime.Now,
                        usuarioCodigo = novoUsuario.codigo,
                        parcelas = 0,
                        tipo = 2,
                        assinaturaCodigo = content.Id

                    };

                    _ctx.tabComprasCurso.Add(compracurso);
                    _ctx.SaveChanges();

                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/CursoOnline.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    string formpag;
                    switch (requestPagarme.payment_method)
                    {
                        case "credit_card":
                            formpag = "Cartão de Crédito";
                            break;
                        case "boleto":
                            formpag = "Boleto";
                            break;
                        case "voucher":
                            formpag = "Voucher";
                            break;
                        case "bank_transfer":
                            formpag = "Transferência bancária";
                            break;
                        case "pix":
                            formpag = "Pix";
                            break;
                        default:
                            formpag = " ";
                            break;
                    }
                    var html = htmlTemplat.Replace("@Nome", requestPagarme.customer.name).Replace("@FormaPag", formpag).Replace("@tempo", "tempo indeterminado.");


                    html = html.Replace("@tabela", "<table class=\"user\">\r\n                    <tr>\r\n                        <th>Usuário</th>\r\n                        <th>Senha</th>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>@Email</td>\r\n                        <td>@Senha</td>\r\n                    </tr>\r\n                </table>")
                        .Replace("@Email", novoUsuario.email)
                        .Replace("@Senha", novoUsuario.senha);
                    emailService.EnviaEmailParametrizavelVar("Var Solutions", requestPagarme.customer.email, "Compra realizada com sucesso!", html, "contato@instituto.varsolutions.com.br");

                    return new CursoOnlineResponse
                    {
                        mensagem = "success",
                        user = novoUsuario
                    };
                }
                else
                {
                    IncluirDetalhesHistorico(compraHistoricoCodigo, content.Status, "Vai retornar um erro para o cliente o status foi paid mas deu merda");
                    emailService.EnviaEmailParametrizavelVar("Var Solutions", "contato@varsolutions.com.br", "Tentativa de compra com problemas", "Nome: " + cadastroRequest.nome + " - email: " + cadastroRequest.email, "contato@instituto.varsolutions.com.br");
                    return null;
                }


            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo da PagamentoService chamando ComprarECadastrarOnline");
                return null;
            }
        }
        public async Task<bool> AdotarAluno(PlanoPagarMeRequest adotar)
        {
            EmailService emailService = new EmailService();
            try
            {
                var options = new RestClientOptions("https://api.pagar.me/core/v5/subscriptions");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Basic c2tfSnJlb1lqS1RXM1N4T2FuTTo="); //Producao
                //request.AddHeader("authorization", "Basic c2tfdGVzdF9yNjRsREdaczhqSThPZHoxOg=="); // Teste
                request.AddJsonBody(JsonConvert.SerializeObject(adotar), false);

                var response = await client.PostAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = JsonConvert.DeserializeObject<AdoteumAlunoResponse>(response.Content);
                    if (content.Status != "active")
                        return false;
                    _ctx.TabAdoteUmAluno.Add(new TabAdoteUmAluno
                    {
                        nome = adotar.customer.name,
                        cpf = adotar.customer.document,
                        cep = adotar.card.billing_address.zip_code,
                        email = adotar.customer.email,
                        dataTrans = DateTime.Now
                    });
                    _ctx.SaveChanges();
                    var Templat = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/AdotarAluno.html";
                    var htmlTemplat = System.IO.File.ReadAllText(Templat);
                    var html = htmlTemplat.Replace("@Nome", adotar.customer.name).Replace("@FormaPag", adotar.payment_method);
                    emailService.EnviaEmailParametrizavelInstituto("Var Solutions", adotar.customer.email, "Adoção efetivada. Muito obrigado!", html);
                    return true;

                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public CupomStatus ObterCupom(string cupom)
        {
            var cupomDb = _ctx.TabCupomCursoOnline.FirstOrDefault(x => x.cupom == cupom);
            if (cupomDb == null)
                return new CupomStatus { cupom = null, status = "does not exist" };
            if (cupomDb.dataVencimento < DateTime.Now)
                return new CupomStatus { cupom = null, status = "expired" };

            return new CupomStatus { cupom = cupomDb, status = "success" };
        }

        public bool ValidarPagamento(int usuarioCodigo, out int parcelasEmAtraso, out int diasEmAtraso, out DateTime? diapagamentoPresencial)
        {

            //Por padrao as variaveis nascem sem problemas para o aluno, ou seja sem atrasos
            //se der uma exception a gente vai retornar sem problemas, é melhor do que dizer 
            //que ele não deve nada do que errar dizendo que esta atrasado por um erro no codigo
            bool pagamentosEmDia = true;
            parcelasEmAtraso = 0;
            diasEmAtraso = 0;
            diapagamentoPresencial = null;
            try
            {
                //consulta o usuario
                var objUsuario = _ctx.TabUsuario.Where(c => c.codigo == usuarioCodigo).FirstOrDefault();
                //Guarda a mensalidade dele em uma variavel
                int mensalidadeCodigo = objUsuario.mensalidade ?? 0;
                //Consulta a mensalidade na tabela para saber qual o valor ele deve pagar
                var objValor = _ctx.tabMensalidade.Where(c => c.codigo == mensalidadeCodigo).FirstOrDefault();

                //Verifica se este aluno tem mensalidade e também se ela tem valor, no caso verifica
                //se não é gratuito.
                if (objValor != null && objValor.valor > 0)
                {
                    //Cria a compentencia, sempre vai ser o mes e o ano em que o metodo foi acionado
                    //por exemplo, se o metodo for chamado no dia 06/09/2023 a competencia vai ficar
                    //como 09/2023
                    var competencia = DateTime.Now.Month.ToString().PadLeft(2, '0') + '/' + DateTime.Now.Year.ToString();

                    //Consulta se este aluno tem um pagamento registrado para esta competencia
                    //Neste momento não estamos consultando se esta atrasado
                    var objPagamento = _ctx.tabPagamentoPresencial.
                                        Where(c => c.usuarioCodigo == usuarioCodigo
                                               && c.competencia == competencia).FirstOrDefault();

                    //Caso ele não tenha pagamento vamos gerar um pagamento pra ele
                    if (objPagamento == null)
                    {
                        //FK da tabela de pagamentos, se for null

                        int diaPagamento = objUsuario.dataPagamento ?? 0;
                        //consulta o dia que ele escolheu pra pagamento
                        var objDiaPagamento = _ctx.tabDiaPagamento.Where(c => c.codigo == diaPagamento).FirstOrDefault();

                        //Cria a data de vencimento, por exemplo se ele escolhou dia 10
                        //Neste se o metodo esta sendo executado no dia 06/09/2023 ele vai gerar um 
                        //pagamento para a data 10/10/2023 (sempre o dia que ele escolho e mais um mes pra 
                        //frente da compentencia que ele esta
                        if (objDiaPagamento != null)
                        {
                            DateTime dataVencimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, objDiaPagamento.diaMes);
                            dataVencimento = dataVencimento.AddMonths(1);
                            var usre = _ctx.tabPagamentoPresencial.Where(c => c.usuarioCodigo == usuarioCodigo).OrderByDescending(x => x.dataVencimento).FirstOrDefault();
                            diapagamentoPresencial = Convert.ToDateTime(usre.dataVencimento);
                            //Cria o objeto e salva inserindo o pagamento no banco
                            tabPagamentoPresencial objPag = new tabPagamentoPresencial
                            {
                                usuarioCodigo = usuarioCodigo,
                                competencia = competencia,
                                valor = objValor.valor,
                                dataVencimento = dataVencimento
                            };
                            _ctx.tabPagamentoPresencial.Add(objPag);
                            _ctx.SaveChanges();
                        }
                    }
                    //Consultar todos os registros que ainda não foram baixados e a data de vencimento já passou, ou seja é menor do que a data atual
                    var objPagamentosAtrasados = _ctx.tabPagamentoPresencial.Where(c => c.usuarioCodigo == usuarioCodigo && c.dataLiquidacao == null && c.dataVencimento < DateTime.Now).ToList();
                    var usree = _ctx.tabPagamentoPresencial.Where(c => c.usuarioCodigo == usuarioCodigo).OrderByDescending(x => x.dataVencimento).FirstOrDefault();
                    if(usree != null) { 
                    diapagamentoPresencial = Convert.ToDateTime(usree.dataVencimento);
                    }
                    //Pega quantas parcelas em atraso foram localizadas
                    parcelasEmAtraso = objPagamentosAtrasados.Count;
                    
                    //Verificar sem pagamentos Atrasados
                    if (parcelasEmAtraso > 0)
                    {
                        //Varivavel de primeiroVencimentoAtr{
                        DateTime primeiroVencimentoAtrasado = objPagamentosAtrasados.OrderBy(c => c.dataVencimento).FirstOrDefault().dataVencimento;
                        //Calcula o total de dias em atraso
                        diasEmAtraso = (primeiroVencimentoAtrasado - DateTime.Now).Days;
                        pagamentosEmDia = false;
                    }
                   
                }
            }
            catch (Exception)
            {
                pagamentosEmDia = true;
            }
            diapagamentoPresencial = diapagamentoPresencial;
            return pagamentosEmDia;
        }
        public bool ValidarPagamentoOnline(int usuarioCodigo,out DateTime? diaPag, out bool isCancel)
        {
            bool pagamentosEmDia = true;

            tabComprasCurso assinatura = _ctx.tabComprasCurso.FirstOrDefault(x => x.usuarioCodigo == usuarioCodigo);

            string url = $"https://api.pagar.me/core/v5/invoices?subscription_id={assinatura.assinaturaCodigo}";

            var options = new RestClientOptions(url);
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Basic c2tfTGRuNEpBWW9IbHVySnJFWjo="); //Producao
            //request.AddHeader("authorization", "Basic c2tfdGVzdF9xbE5uRWdFc2R1V1ZvWDJkOg==");// Teste            

            var response = client.Get(request);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content;
                
                var responsePagarme = JsonConvert.DeserializeObject<ResponsePagarmeFatura>(result);
                diaPag = responsePagarme.Data.First().Cycle.StartAt.DateTime;
                if (responsePagarme.Data.First().Charge.Status == "paid")
                {
                    isCancel = false;

                    return pagamentosEmDia;
                }
                else if (responsePagarme.Data.First().Charge.Status == "canceled")
                {
                    isCancel = true;
                    return pagamentosEmDia;
                }
                else
                {
                    isCancel = false;
                    pagamentosEmDia = false;
                    return pagamentosEmDia;
                }
            }
            isCancel = false;
            diaPag = null;
            return pagamentosEmDia;

        }
        public int GerarHistoricoCompra(string cartao, string cep, string cpf, string cupomDesconto, string documentoAuxiliar, string email, string nResidencial, string observacao, string senha, string telefone)
        {
            try
            {
                tabCompraHistorico objCompraHis = new tabCompraHistorico
                {
                    cartaoCliente = cartao,
                    cep = cep,
                    cpf = cpf,
                    cupomDesconto = cupomDesconto,
                    dataHora = DateTime.Now,
                    documentoAuxiliar = documentoAuxiliar,
                    email = email,
                    nResidencial = nResidencial,
                    observacao = observacao,
                    senha = senha,
                    telefone = telefone
                };

                _ctx.tabCompraHistorico.Add(objCompraHis);
                _ctx.SaveChanges();

                return objCompraHis.codigo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void IncluirDetalhesHistorico(int compraHistoricoCodigo, string documentoAuxiliar, string observacao)
        {
            try
            {
                tabCompraHistoricoDetalhes objCompraHisDet = new tabCompraHistoricoDetalhes
                {
                    compraHistoricoCodigo = compraHistoricoCodigo,
                    dataHora = DateTime.Now,
                    documentoAuxiliar = documentoAuxiliar,
                    observacao = observacao
                };

                _ctx.tabCompraHistoricoDetalhes.Add(objCompraHisDet);
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public InfoPlanoResponse ObterPlano(int usuarioId)
        {
            try
            {
                InfoPlanoResponse response = new();
                var user = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == usuarioId);
                if (user.unidadeCodigo != 3)
                {
                    response.descricao = "Curso Presencial";
                    var validade = _ctx.tabUsuarioHistorico.FirstOrDefault(x => x.usuarioCodigo == usuarioId && x.statusAntigo != 1 && x.statusNovo == 1);
                   
                    response.validade = validade == null ? user.dataCriacao.AddMonths(10) : validade.dataStatus.AddMonths(10);
                    if (user.dataPagamento != null)
                    {
                        response.valor = _ctx.tabMensalidade.FirstOrDefault(x => x.codigo == user.mensalidade).valor;
                        var proxPag = _ctx.tabPagamentoPresencial.Where(x => x.usuarioCodigo == usuarioId).OrderByDescending(c => c.dataVencimento).FirstOrDefault();
                        var dtvenci = Convert.ToDateTime(proxPag.dataVencimento);
                        //  response.proxPagamento = new DateTime(Convert.ToInt32(user.dataPagamento), Convert.ToInt32(proxPag.Split("/")[0]), Convert.ToInt32(proxPag.Split("/")[1]));
                        response.proxPagamento = Convert.ToDateTime(dtvenci);
                    }
                }
                else
                {

                    var compracurso = _ctx.tabComprasCurso.FirstOrDefault(x => x.usuarioCodigo == usuarioId);
                    if (compracurso == null)
                    {
                        response.descricao = "Curso Online";
                        response.validade = null;
                        response.proxPagamento = null;
                        response.valor = 0;
                    }
                    else if (compracurso.assinaturaCodigo != null)
                    {

                        string url = $"https://api.pagar.me/core/v5/invoices?subscription_id={compracurso.assinaturaCodigo}";

                        var options = new RestClientOptions(url);
                        var client = new RestClient(options);
                        var request = new RestRequest("");
                        request.AddHeader("accept", "application/json");
                        request.AddHeader("authorization", "Basic c2tfTGRuNEpBWW9IbHVySnJFWjo="); //Producao
                        // request.AddHeader("authorization", "Basic c2tfdGVzdF9xbE5uRWdFc2R1V1ZvWDJkOg==");// Teste            

                        var responseApi = client.Get(request);

                        if (responseApi.IsSuccessStatusCode)
                        {
                            var result = responseApi.Content;

                            var responsePagarme = JsonConvert.DeserializeObject<ResponsePagarmeFatura>(result);

                            response.descricao = "Curso Online Mensal";
                            response.validade = responsePagarme.Data.First().Cycle.EndAt.DateTime;
                            response.proxPagamento = Convert.ToDateTime(response.validade).AddDays(1);
                            response.valor = 70;
                        }
                    }
                    else
                    {
                        response.descricao = "Curso Online Anual";
                        response.proxPagamento = null;
                        response.valor = 597;
                        response.validade = compracurso.data.AddYears(1);


                    }
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public erroModel CancelarAssinatura(int usuarioId, CancelarAssinaturaModel motivo)
        {
            try
            {
                var curso = _ctx.tabComprasCurso.FirstOrDefault(x => x.usuarioCodigo == usuarioId);

                var options = new RestClientOptions($"https://api.pagar.me/core/v5/subscriptions/{curso.assinaturaCodigo}");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Basic c2tfTGRuNEpBWW9IbHVySnJFWjo="); //Producao
                //request.AddHeader("authorization", "Basic c2tfdGVzdF9xbE5uRWdFc2R1V1ZvWDJkOg==");// Teste            

                var response = client.Delete(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content;

                    var responsePagarme = JsonConvert.DeserializeObject<CancelamentoResponse>(result);



                    if (responsePagarme.Status == "canceled")
                    {

                        _ctx.tabCursoOnlineCancelamento.Add(new tabCursoOnlineCancelamento
                        {
                            motivoCodigo = motivo.CancelamentoCodigo,
                            outroMotivo = motivo.observacao,
                            dataCancelamento = DateTime.Now
                        });
                        _ctx.SaveChanges();
                        return new erroModel
                        {
                            Mensagem = "success"
                        };
                    }
                    else
                    {
                        return new erroModel
                        {
                            Mensagem = "retorno da Api diferente de canceled"
                        };
                    }
                }
                else
                {
                    return new erroModel
                    {
                        Mensagem = "Status da Api diferente de sucesso"
                    };
                }
            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo da PagamentoService chamando CancelarAssinatura");
                return null;
            }
        }

    }
}
