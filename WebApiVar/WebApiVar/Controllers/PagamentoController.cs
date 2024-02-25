using Microsoft.AspNetCore.Mvc;
using WebApiVar.Application.Models;
using static WebApiVar.Application.Models.DoacaoRequest.Costumer.Phones;
using static WebApiVar.Application.Models.DoacaoRequest.Payments.Credit_card;
using static WebApiVar.Application.Models.DoacaoRequest.Payments;
using static WebApiVar.Application.Models.DoacaoRequest;
using RestSharp;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using WebApiVar.Application.Services;
using WebApiVar.Repository;
using static WebApiVar.Application.Models.PlanoPagarMeRequest.Costumer.Phones;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static WebApiVar.Application.Models.AdoteumAlunoResponse;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly VAREntities _ctx;
        public PagamentoController(VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("Doar")]
        public async Task<IActionResult> DoarumValor(
        [FromForm] string customer_address_state,
        [FromForm] string customer_address_city,
        [FromForm] string customer_address_zip_code,
        [FromForm] string customer_address_line_1,
        [FromForm] string customer_address_line_2,
        [FromForm] string customer_phones_mobile_phone_area_code,
        [FromForm] string customer_phones_mobile_phone_number,
        [FromForm] string customer_name,
        [FromForm] string customer_type,
        [FromForm] string customer_email,
        [FromForm] string customer_document,
        [FromForm] string customer_gender,
        [FromForm] string customer_birthdate,
        [FromForm] string payments_credit_card_card_number,
        [FromForm] string payments_credit_card_card_holder_name,
        [FromForm] int payments_credit_card_card_exp_month,
        [FromForm] int payments_credit_card_card_exp_year,
        [FromForm] string payments_credit_card_card_cvv,
        [FromForm] int items_0_amount)
        {
            var doacao = new DoacaoRequest
            {
                customer = new Costumer
                {
                    address = new Costumer.Address
                    {
                        country = "BR",
                        state = customer_address_state,
                        city = customer_address_city,
                        zip_code = customer_address_zip_code,
                        line_1 = customer_address_line_1,
                        line_2 = customer_address_line_2
                    },
                    phones = new Costumer.Phones
                    {
                        mobile_phone = new DoacaoRequest.Costumer.Phones.Mobile_phone
                        {
                            country_code = "55",
                            area_code = customer_phones_mobile_phone_area_code,
                            number = customer_phones_mobile_phone_number
                        }
                    },
                    name = customer_name,
                    type = customer_type,
                    email = customer_email,
                    code = "ADAL",
                    document = customer_document,
                    document_type = "CPF",
                    gender = customer_gender,
                    birthdate = customer_birthdate
                },
                items = new List<Items>
                {
                    new Items
                {
                    amount = items_0_amount,
                    description = "Doação Var",
                    quantity = 1,
                    code = "1000AL"
                }
                    },
                payments = new List<Payments>() { new Payments{
                        credit_card = new Credit_card
                        {
                            card = new DoacaoRequest.Payments.Credit_card.Card
                            {
                                number = payments_credit_card_card_number,
                                holder_name = payments_credit_card_card_holder_name,
                                exp_month = payments_credit_card_card_exp_month,
                                exp_year = payments_credit_card_card_exp_year,
                                cvv = payments_credit_card_card_cvv,
                               billing_address = new Credit_card.Card.Billing_address
                               {
                                    country = "BR",
                                    state = customer_address_state,
                                    city = customer_address_city,
                                    zip_code = customer_address_zip_code,
                                    line_1 = customer_address_line_1,
                                    line_2 = customer_address_line_2
                               }
                            },
                            operation_type = "auth_and_capture",
                            installments = 1,
                            statement_descriptor = "ADAL"
                        },
                        payment_method = "credit_card"
                    }
                    },
                antifraud_enabled = false
            };


            var pagamento = new PagamentoService(_ctx);
            var sucesso = Convert.ToBoolean(await pagamento.RealizarDoacao(doacao));

            if (sucesso)
                return Ok();
            else
                return BadRequest();

            //var base64AuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"sk_test_r64lDGZs8jI8Odz1:"));

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://api.pagar.me/core/v5/orders");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthString);

            //var response = await client.PostAsJsonAsync("https://api.pagar.me/core/v5/orders",JsonConvert.SerializeObject(doacao));        

        }
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("ComprarCursoOnline/{id}")]
        [Authorize]
        public async Task<IActionResult> ComprarOCursoOnline(
        [FromRoute] int id,
        [FromForm] string customer_address_state,
        [FromForm] string customer_address_city,
        [FromForm] string customer_address_zip_code,
        [FromForm] string customer_address_line_1,
        [FromForm] string customer_address_line_2,
        [FromForm] string customer_phones_mobile_phone_area_code,
        [FromForm] string customer_phones_mobile_phone_number,
        [FromForm] string customer_name,
        [FromForm] string customer_type,
        [FromForm] string customer_email,
        [FromForm] string customer_document,
        [FromForm] string payments_credit_card_card_number,
        [FromForm] string payments_credit_card_card_holder_name,
        [FromForm] int payments_credit_card_card_exp_month,
        [FromForm] int payments_credit_card_card_exp_year,
        [FromForm] string payments_credit_card_card_cvv,
        [FromForm] string? cupom)
        {
            var doacao = new DoacaoRequest
            {
                customer = new Costumer
                {
                    address = new Costumer.Address
                    {
                        country = "BR",
                        state = customer_address_state,
                        city = customer_address_city,
                        zip_code = customer_address_zip_code,
                        line_1 = customer_address_line_1,
                        line_2 = customer_address_line_2
                    },
                    phones = new Costumer.Phones
                    {
                        mobile_phone = new DoacaoRequest.Costumer.Phones.Mobile_phone
                        {
                            country_code = "55",
                            area_code = customer_phones_mobile_phone_area_code,
                            number = customer_phones_mobile_phone_number
                        }
                    },
                    name = customer_name,
                    type = customer_type,
                    email = customer_email,
                    code = "ADAL",
                    document = customer_document,
                    document_type = "CPF"
                },
                items = new List<Items>
                {
                    new Items
                {
                    amount = 59700,
                    description = "Curso FullStack - Var Solutions",
                    quantity = 1,
                    code = "COVR"
                }
                    },
                payments = new List<Payments>() { new Payments{
                        credit_card = new Credit_card
                        {
                            card = new DoacaoRequest.Payments.Credit_card.Card
                            {
                                number = payments_credit_card_card_number,
                                holder_name = payments_credit_card_card_holder_name,
                                exp_month = payments_credit_card_card_exp_month,
                                exp_year = payments_credit_card_card_exp_year,
                                cvv = payments_credit_card_card_cvv,
                               billing_address = new Credit_card.Card.Billing_address
                               {
                                    country = "BR",
                                    state = customer_address_state,
                                    city = customer_address_city,
                                    zip_code = customer_address_zip_code,
                                    line_1 = customer_address_line_1,
                                    line_2 = customer_address_line_2
                               }
                            },
                            operation_type = "auth_and_capture",
                            installments = id,
                            statement_descriptor = "COVR"
                        },
                        payment_method = "credit_card"
                    }
                    },
                antifraud_enabled = false
            };


            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var pagamento = new PagamentoService(_ctx);
            var auth = new AutenticacaoService(_ctx);
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            var user = await pagamento.ComprarCursoOnline(doacao, Convert.ToInt32(usuarioId), cupom);

            if (user != null)
                return Ok(auth.GerarTokenporId(user.codigo));
            else
                return BadRequest();

            //var base64AuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"sk_test_r64lDGZs8jI8Odz1:"));

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://api.pagar.me/core/v5/orders");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthString);

            //var response = await client.PostAsJsonAsync("https://api.pagar.me/core/v5/orders",JsonConvert.SerializeObject(doacao));        

        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("ComprarCursoOnline2/{id}")]
        public async Task<IActionResult> ComprarOCursoOnlineECadastrar(
        [FromRoute] int id,
        [FromForm] string customer_address_state,
        [FromForm] string customer_address_city,
        [FromForm] string customer_address_zip_code,
        [FromForm] string customer_address_line_1,
        [FromForm] string customer_address_line_2,
        [FromForm] string customer_phones_mobile_phone_area_code,
        [FromForm] string customer_phones_mobile_phone_number,
        [FromForm] string customer_name,
        [FromForm] string customer_type,
        [FromForm] string customer_email,
        [FromForm] string customer_document,
        [FromForm] string payments_credit_card_card_number,
        [FromForm] string payments_credit_card_card_holder_name,
        [FromForm] int payments_credit_card_card_exp_month,
        [FromForm] int payments_credit_card_card_exp_year,
        [FromForm] string payments_credit_card_card_cvv,
        [FromForm] string password,
        [FromForm] string? cupom)
        {

            var doacao = new DoacaoRequest
            {
                customer = new Costumer
                {
                    address = new Costumer.Address
                    {
                        country = "BR",
                        state = customer_address_state,
                        city = customer_address_city,
                        zip_code = customer_address_zip_code,
                        line_1 = customer_address_line_1,
                        line_2 = customer_address_line_2
                    },
                    phones = new Costumer.Phones
                    {
                        mobile_phone = new DoacaoRequest.Costumer.Phones.Mobile_phone
                        {
                            country_code = "55",
                            area_code = customer_phones_mobile_phone_area_code,
                            number = customer_phones_mobile_phone_number
                        }
                    },
                    name = customer_name,
                    type = customer_type,
                    email = customer_email,
                    code = "ADAL",
                    document = customer_document,
                    document_type = "CPF"
                },
                items = new List<Items>
                {
                    new Items
                {
                    amount = 59700,
                    description = "Curso FullStack - Var Solutions",
                    quantity = 1,
                    code = "COVR"
                }
                    },
                payments = new List<Payments>() { new Payments{
                        credit_card = new Credit_card
                        {
                            card = new DoacaoRequest.Payments.Credit_card.Card
                            {
                                number = payments_credit_card_card_number,
                                holder_name = payments_credit_card_card_holder_name,
                                exp_month = payments_credit_card_card_exp_month,
                                exp_year = payments_credit_card_card_exp_year,
                                cvv = payments_credit_card_card_cvv,
                               billing_address = new Credit_card.Card.Billing_address
                               {
                                    country = "BR",
                                    state = customer_address_state,
                                    city = customer_address_city,
                                    zip_code = customer_address_zip_code,
                                    line_1 = customer_address_line_1,
                                    line_2 = customer_address_line_2
                               }
                            },
                            operation_type = "auth_and_capture",
                            installments = id,
                            statement_descriptor = "COVR"
                        },
                        payment_method = "credit_card"
                    }
                    },
                antifraud_enabled = false
            };



            var pagamento = new PagamentoService(_ctx);
            var auth = new AutenticacaoService(_ctx);
            //var usuario = cadastro.CadastroUsuarioCursoOnline(new CadastroRequest
            //{
            //    email = doacao.customer.email,
            //    nome = doacao.customer.email.Split("@")[0],
            //    senha = password
            //});
            string ultimosQuatroDigitos = payments_credit_card_card_number.Substring(payments_credit_card_card_number.Length - 4, 4);
            int compraHistoricoCodigo = pagamento.GerarHistoricoCompra(ultimosQuatroDigitos,
                 customer_address_zip_code, customer_document, cupom, "",
                 customer_email, customer_address_line_2, "Preparação para chamar o pagar.me",
                 password, customer_phones_mobile_phone_number);

            var user = await pagamento.ComprarECadastrarCursoOnline(doacao, new CadastroRequest
            {
                email = doacao.customer.email,
                nome = doacao.customer.email.Split("@")[0],
                senha = password
            }, cupom, compraHistoricoCodigo);

            if (user == null)
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Retornou nulo");
                return BadRequest();
            }

            if (user.mensagem == "Email já existe")
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Retornou o erro que o Email já existe");
                return BadRequest(new { mensagem = "Este Email já existe" });
            }

            if (user.mensagem == "success")
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Retornou como Sucesso");
                return Ok(auth.GerarTokenporId(user.user.codigo));
            }
            else
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, user.mensagem, "Retornamos BadReques");
                return BadRequest();
            }


            //var base64AuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"sk_test_r64lDGZs8jI8Odz1:"));

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://api.pagar.me/core/v5/orders");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthString);

            //var response = await client.PostAsJsonAsync("https://api.pagar.me/core/v5/orders",JsonConvert.SerializeObject(doacao));        

        }
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("ComprarCursoOnlineMensal")]
        public async Task<IActionResult> ComprarCursoMensal(
               [FromForm] string customer_address_state,
    [FromForm] string customer_address_city,
    [FromForm] string customer_address_zip_code,
    [FromForm] string customer_address_line_1,
    [FromForm] string customer_address_line_2,
    [FromForm] string customer_phones_mobile_phone_area_code,
    [FromForm] string customer_phones_mobile_phone_number,
    [FromForm] string customer_name,
    [FromForm] string customer_type,
    [FromForm] string customer_email,
    [FromForm] string customer_document,
    [FromForm] string payments_credit_card_card_number,
    [FromForm] string payments_credit_card_card_holder_name,
    [FromForm] int payments_credit_card_card_exp_month,
    [FromForm] int payments_credit_card_card_exp_year,
    [FromForm] string payments_credit_card_card_cvv,
    [FromForm] string password)
        {
            var compra = new PlanoPagarMeRequest
            {
                customer = new PlanoPagarMeRequest.Costumer
                {
                    name = customer_name,
                    email = customer_email,
                    document = customer_document,
                    document_type = "CPF",
                    type = customer_type,
                    phones = new PlanoPagarMeRequest.Costumer.Phones
                    {
                        mobile_phone = new PlanoPagarMeRequest.Costumer.Phones.Mobile_phone
                        {
                            country_code = "55",
                            area_code = customer_phones_mobile_phone_area_code,
                            number = customer_phones_mobile_phone_number
                        },
                        home_phone = new PlanoPagarMeRequest.Costumer.Phones.Home_phone
                        {
                            country_code = "55",
                            area_code = customer_phones_mobile_phone_area_code,
                            number = customer_phones_mobile_phone_number
                        }   
                    }
                },
                card = new PlanoPagarMeRequest.Card
                {
                    billing_address = new PlanoPagarMeRequest.Card.Billing_address
                    {
                        line_1 = customer_address_line_1,
                        line_2 = customer_address_line_2,
                        zip_code = customer_address_zip_code,
                        city = customer_address_city,
                        country = "BR",
                        state = customer_address_state
                    },
                    holder_name = payments_credit_card_card_holder_name,
                    number = payments_credit_card_card_number,
                    exp_month = payments_credit_card_card_exp_month,
                    exp_year = payments_credit_card_card_exp_year,
                    cvv = payments_credit_card_card_cvv
                },
                plan_id = "plan_W8yNEDIajC77LVPx",
                payment_method = "credit_card",
                boleto_due_days = 5,
                metadata = new PlanoPagarMeRequest.Metadata
                {
                    id = "my_subscription_id"
                }
            };

            var pagamento = new PagamentoService(_ctx);
            var auth = new AutenticacaoService(_ctx);

            string ultimosQuatroDigitos = payments_credit_card_card_number.Substring(payments_credit_card_card_number.Length - 4, 4);
            int compraHistoricoCodigo = pagamento.GerarHistoricoCompra(ultimosQuatroDigitos,
                 customer_address_zip_code, customer_document, null, "",
                 customer_email, customer_address_line_2, "Preparação para chamar o pagar.me",
                 password, customer_phones_mobile_phone_number);
            var user = await pagamento.ComprarCursoOnlineMensal(compra, new CadastroRequest
            {
                email = compra.customer.email,
                nome = compra.customer.email.Split("@")[0],
                senha = password
            }, compraHistoricoCodigo);

            if (user == null)
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Retornou nulo");
                return BadRequest();
            }

            if (user.mensagem == "Email já existe")
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Retornou o erro que o Email já existe");
                return BadRequest(new { mensagem = "Este Email já existe" });
            }

            if (user.mensagem == "success")
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, "", "Retornou como Sucesso");
                return Ok(auth.GerarTokenporId(user.user.codigo));
            }
            else
            {
                pagamento.IncluirDetalhesHistorico(compraHistoricoCodigo, user.mensagem, "Retornamos BadReques");
                return BadRequest();
            }

        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("Adotar")]
        public async Task<IActionResult> AdoteUmAluno(
               [FromForm] string customer_address_state,
    [FromForm] string customer_address_zip_code,
    [FromForm] string customer_address_line_1,
    [FromForm] string customer_address_line_2,
    [FromForm] string customer_phones_mobile_phone_area_code,
    [FromForm] string customer_phones_mobile_phone_number,
    [FromForm] string customer_name,
    [FromForm] string customer_type,
    [FromForm] string customer_email,
    [FromForm] string customer_document,
    [FromForm] string payments_credit_card_card_number,
    [FromForm] string payments_credit_card_card_holder_name,
    [FromForm] int payments_credit_card_card_exp_month,
    [FromForm] int payments_credit_card_card_exp_year,
    [FromForm] string payments_credit_card_card_cvv)
        {
            var adocao = new PlanoPagarMeRequest
            {
                customer = new PlanoPagarMeRequest.Costumer
                {
                    name = customer_name,
                    email = customer_email,
                    document = customer_document,
                    document_type = "CPF",
                    type = customer_type,
                    phones = new PlanoPagarMeRequest.Costumer.Phones
                    {
                        mobile_phone = new PlanoPagarMeRequest.Costumer.Phones.Mobile_phone
                        {
                            country_code = "55",
                            area_code = customer_phones_mobile_phone_area_code,
                            number = customer_phones_mobile_phone_number
                        },
                        home_phone = new PlanoPagarMeRequest.Costumer.Phones.Home_phone
                        {
                            country_code = "55",
                            area_code = customer_phones_mobile_phone_area_code,
                            number = customer_phones_mobile_phone_number
                        }
                    }
                },
                card = new PlanoPagarMeRequest.Card
                {
                    billing_address = new PlanoPagarMeRequest.Card.Billing_address
                    {
                        line_1 = customer_address_line_1,
                        line_2 = customer_address_line_2,
                        zip_code = customer_address_zip_code,
                        city = customer_address_state,
                        country = "BR",
                        state = customer_address_state
                    },
                    holder_name = payments_credit_card_card_holder_name,
                    number = payments_credit_card_card_number,
                    exp_month = payments_credit_card_card_exp_month,
                    exp_year = payments_credit_card_card_exp_year,
                    cvv = payments_credit_card_card_cvv
                },
                plan_id = "plan_NZmPVdju3uvXejv0",
                payment_method = "credit_card",
                boleto_due_days = 5,
                metadata = new PlanoPagarMeRequest.Metadata
                {
                    id = "my_subscription_id"
                }
            };

            var pagamento = new PagamentoService(_ctx);
            var sucesso = Convert.ToBoolean(await pagamento.AdotarAluno(adocao));

            if (sucesso)
                return Ok();
            else
                return BadRequest();
        }
        [HttpGet]
        [Route("ObterCupom/{cupom}")]
        public IActionResult ObterCupom([FromRoute] string cupom)
        {
            var pagamento = new PagamentoService(_ctx);
            return Ok(pagamento.ObterCupom(cupom));
        } 
        [HttpGet]
        [Route("ObterPlano")]
        [Authorize]
        public IActionResult ObterPlano()
        {
            var pagamento = new PagamentoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            return Ok(pagamento.ObterPlano(Convert.ToInt32(usuarioId)));
        }
        [HttpDelete]
        [Route("CancelarAssinatura")]
        [Authorize]
        public IActionResult CancelarAssinatura([FromBody] CancelarAssinaturaModel motivo)
        {
            var pagamento = new PagamentoService(_ctx);
            var identidade = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = identidade.FindFirst("usuarioId").Value;
            return Ok(pagamento.CancelarAssinatura(Convert.ToInt32(usuarioId),motivo));
        }
    }
}
