using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;
using WebApiVar.Services;

namespace WebApiVar.Application.Services
{
    public class AdministrativoService
    {
        private readonly VAREntities _ctx;

        public AdministrativoService(VAREntities context)
        {
            _ctx = context;
        }
        public bool DarPresenca(AlunoPresencaRequest request)
        {
            try
            {
                var user = _ctx.TabUsuario.Where(x => x.codigo == request.usuarioCodigo).FirstOrDefault();
                _ctx.TabAlunoPresenca.Add(new TabAlunoPresenca
                {
                    DataMarcacao = DateTime.Now,
                    DataPresenca = request.DataPresenca,
                    observacao = request.observacao,
                    posicaoCodigo = user.id_Posicao,
                    usuarioCodigo = request.usuarioCodigo,
                    statusPresencaCodigo = request.statusPresencaCodigo,
                    turmaCodigo = user.turmaCodigo,
                });
                _ctx.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }
        public List<TabTurma> ObterTurmas()
        {
            try
            {
                return _ctx.TabTurma.Where(x => x.isAtivo == true).ToList();
            }
            catch
            {
                throw;
            }
        }
        public List<Vw_SituacaoAtualVagasPorTurma> ObterVagas()
        {
            try
            {
                return _ctx.Vw_SituacaoAtualVagasPorTurma.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<Proc_AulasIniciadasPorturmaPorDia> ObterListaPresenca(int turmacodigo, DateTime date)
        {
            var proc = "exec proc_AulasIniciadasPorturmaPorDia @date,@userId";
            return _ctx.proc_AulasIniciadasPorturmaPorDia.FromSqlRaw(proc.Replace("@date", $"'{date.Year}-{date.Month}-{date.Day}'").Replace("@userId", turmacodigo.ToString())).ToList();
        }
        public List<ListaEspera> ObterListaEspera(int turmaCodigo = 0)
        {
            try
            {
                List<TabUsuario> objUser = new();
                if (turmaCodigo == 0)
                    objUser = _ctx.TabUsuario.Where(x => (x.status == 3 || x.status == 4) && x.unidadeCodigo == 1).ToList();
                else
                    objUser = _ctx.TabUsuario.Where(x => (x.status == 3 || x.status == 4) && x.unidadeCodigo == 1 && x.turmaCodigo == turmaCodigo).ToList();
                var turmas = _ctx.TabTurma.ToList();

                List<TabTurma> descTurma = new List<TabTurma>();
                List<ListaEspera> users = new List<ListaEspera>();
                foreach (var item in objUser)
                {
                    var descricaoTurma = turmas.FirstOrDefault(t => t.codigo == turmaCodigo)?.descricao;

                    users.Add(new ListaEspera
                    {
                        codigo = item.codigo,
                        nome = item.nome,
                        data = item.dataCriacao,
                        descricaoTurma = descricaoTurma
                    });
                }
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AprovarResponse AprovarMatricula(int id)
        {
            try
            {
                AprovarResponse response = new AprovarResponse();
                EmailService emailService = new EmailService();
                var usuarios = _ctx.TabUsuario.ToList();
                var codUser = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                var turmas = _ctx.TabTurma.ToList();
                var posicoes = _ctx.TabPosicaoAluno.ToList();
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                var baia = _ctx.TabUsuario.Where(x => x.turmaCodigo == usuario.turmaCodigo && (x.status == 1 || x.status == 6)).Select(y => y.id_Posicao).ToList();
                var baialivres = _ctx.TabPosicaoAluno.Where(x => !baia.Any(y => y == x.id) && x.isNoteVar == true && x.isAtivo == true).ToList().First();
                if (baialivres == null)
                {
                    response.mensagem = "Não tem vaga";
                    response.sucesso = false;
                    return response;
                }
                else
                {
                    usuario.status = 6;
                    usuario.id_Posicao = baialivres.id;
                    usuario.dataEmailEnviado = DateTime.Now;
                    _ctx.SaveChanges();

                    var Template = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Convocacao.html";
                    var htmlTemplate = System.IO.File.ReadAllText(Template);
                    var htmlArrumado = htmlTemplate
                    .Replace("@Nome", codUser.nome)
                    .Replace("@Baia", posicoes.FirstOrDefault(c => c.id == codUser.id_Posicao)?.descricao.ToString())
                    .Replace("@DataInicioPrazo", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("@DataLimiteApresentacao", DateTime.Now.AddDays(7).ToString("dd/MM/yyyy"))
                    .Replace("@Turma", turmas.FirstOrDefault(x => x.codigo == codUser.turmaCodigo)?.descricao)
                    .Replace("@NumeroContato", "+55 11 5198-7389")
                    .Replace("@Endereco", turmas.FirstOrDefault(x => x.codigo == codUser.turmaCodigo)?.localizacao);

                    emailService.EnviaEmailParametrizavelVar("Var Solutions", codUser.email, "Realizar Matrícula", htmlArrumado);

                    response.mensagem = "Aluno aprovado na turma de " + turmas.FirstOrDefault(x => x.codigo == codUser.turmaCodigo)?.descricao + " na baia " + posicoes.FirstOrDefault(c => c.id == codUser.id_Posicao)?.descricao.ToString();
                    response.sucesso = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                AprovarResponse response = new AprovarResponse();
                response.mensagem = "Deu algum problema na hora de aprovar";
                response.sucesso = false;
                return response;
            }
        }
        public AprovarResponse RejeitarMatricula(int id)
        {
            try
            {
                AprovarResponse response = new AprovarResponse();
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                if (usuario == null)
                {
                    response.mensagem = "Não achou esse usuarioCodigo";
                    response.sucesso = false;
                    return response;
                }
                var turma = _ctx.TabTurma.ToList();
                var posicao = _ctx.TabPosicaoAluno.ToList();
                var descTurma = turma.FirstOrDefault(x => x.codigo == usuario.turmaCodigo)?.descricao;
                var descPosicao = posicao.FirstOrDefault(x => x.id == usuario.id_Posicao)?.descricao;
                usuario.status = 2;
                usuario.descricao = "Usuario rejeitado no dia " + DateTime.Now.ToString("dd/MM/yyyy") + " era da turma " + descTurma + " sentava na " + descPosicao;
                usuario.id_Posicao = null;
                _ctx.SaveChanges();
                response.mensagem = "Usuario rejeitado no dia " + DateTime.Now.ToString("dd/MM/yyyy") + " era da turma " + descTurma + " sentava na " + descPosicao;
                response.sucesso = true;
                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<VerDetalhes> DetalhesUsuario(int id)
        {
            try
            {
                List<VerDetalhes> detalhes = new List<VerDetalhes>();
                var usuario = _ctx.TabUsuario.Where(x => x.codigo == id).ToList();

                if (usuario != null)
                {
                    var diaPagamento = _ctx.tabDiaPagamento.ToList();
                    var posicaoAluno = _ctx.TabPosicaoAluno.ToList();
                    var uf = _ctx.tabUf.ToList();
                    var raca = _ctx.tabRaca.ToList();
                    var genero = _ctx.tabGenero.ToList();
                    var unidade = _ctx.tabUnidade.ToList();
                    var mensalidade = _ctx.tabMensalidade.ToList();
                    var status = _ctx.TabStatus.ToList();
                    var turma = _ctx.TabTurma.ToList();
                    foreach (var item in usuario)
                    {
                        string descDiaPagamento = diaPagamento.FirstOrDefault(x => x.codigo == item.dataPagamento)?.descricao;
                        string descPosicaoAluno = posicaoAluno.FirstOrDefault(x => x.id == item.id_Posicao)?.descricao;
                        string descUf = uf.FirstOrDefault(x => x.codigo == item.codigo)?.decricao;
                        string descRaca = raca.FirstOrDefault(x => x.codigo == item.cor)?.decricao;
                        string descGenero = genero.FirstOrDefault(x => x.codigo == item.genero)?.decricao;
                        string descUnidade = unidade.FirstOrDefault(x => x.codigo == item.genero)?.descricao;
                        string descMensalidade = mensalidade.FirstOrDefault(x => x.codigo == item.mensalidade)?.descricao;
                        string descStatus = status.FirstOrDefault(x => x.codigo == item.status)?.descricao;
                        string descTurma = turma.FirstOrDefault(x => x.codigo == item.turmaCodigo)?.descricao;
                        detalhes.Add(new VerDetalhes
                        {
                            Codigo = item.codigo,
                            Nome = item.nome,
                            Email = item.email,
                            Senha = item.senha,
                            UltimoAcesso = item.ultimoAcesso == null ? "Sem data de ultimo acesso" : item.ultimoAcesso.ToString(),
                            DataCriacao = item.dataCriacao == null ? "Sem data de Criacao" : item.dataCriacao.ToString(),
                            Turma = descTurma == null ? "Nenhuma turma cadastrada" : descTurma,
                            DataNascimento = item.dataNascimento == null ? "Nenhuma data de nascimento" : item.dataNascimento.ToString(),
                            Status = descStatus,
                            UrlFoto = item.urlFoto == null ? "Nenhuma foto cadastrada" : item.urlFoto,
                            Cep = item.cep == null ? 0 : item.cep,
                            Rua = item.rua == null ? "Nenhuma rua " : item.rua,
                            Bairro = item.bairro == null ? "Nenhum bairro cadastrado " : item.bairro,
                            Cidade = item.cidade == null ? "Nenhuma cidade cadastrada" : item.cidade,
                            Celular = item.celular == null ? "Nenhum telefone cadastrado" : item.celular,
                            Descricao = item.descricao == null ? "Nenhuma data de nascimento" : item.descricao,
                            HaveNote = item.haveNote == true ? "Tem notebook" : "Não temnotebook",
                            Genero = descGenero == null ? "Nenhum genero cadastrado" : descGenero,
                            RG = item.RG == null ? "Nenhum RG cadastrado" : item.RG,
                            Cor = descRaca == null ? "Nenhuma cor cadastrada" : descRaca,
                            UF = descUf == null ? "Nenhum estado cadastrado" : descUf,
                            HaveInternetHouse = item.haveInternetHouse == true ? "Tem internet" : "Não tem internet",
                            RendaFamiliar = item.rendaFamiliar == null ? 0 : item.rendaFamiliar,
                            QntdPessoas = item.qntdPessoas == null ? 0 : item.qntdPessoas,
                            Mensalidade = descMensalidade == null ? "Nenhuma mensalidade cadastrada" : descMensalidade,
                            Cargo = item.cargo == null ? "Nenhum cargo cadastrado" : item.cargo,
                            Atividade = item.atividade == null ? "Nenhuma ativiadade cadastrada" : item.atividade,
                            NumeroCasa = item.numeroCasa == null ? "Nenhum numero de residencia cadastrado" : item.numeroCasa,
                            Complemento = item.complemento == null ? "Nenhum complemneto cadastrado" : item.complemento,
                            Unidade = descUnidade == null ? "Nenhuma unidade cadastrada" : descUnidade,
                            DataPagamento = descDiaPagamento == null ? "Nenhuma data de pagamento cadastrada" : descDiaPagamento

                        });
                    }
                }

                return detalhes;

                /*
                             var objCompleto = (from aa in _ctx.TabUsuario
                                               join bb in _ctx.tabDiaPagamento on aa.dataPagamento equals bb.codigo
                                               where aa.codigo == id
                                               join cc in _ctx.TabPosicaoAluno on aa.id_Posicao equals cc.id
                                               join dd in _ctx.tabUf on aa.UF equals dd.codigo
                                               join ee in _ctx.tabRaca on aa.cor equals ee.codigo
                                               join ff in _ctx.tabGenero on aa.genero equals ff.codigo
                                               join gg in _ctx.tabUnidade on aa.unidadeCodigo equals gg.codigo
                                               join hh in _ctx.tabMensalidade on aa.mensalidade equals hh.codigo
                                               join ii in _ctx.TabStatus on aa.status equals ii.codigo
                                               join jj in _ctx.TabTurma on aa.turmaCodigo equals jj.codigo
                                               select new VerDetalhes
                                               {
                                                   Codigo = aa.codigo,
                                                   Nome = aa.nome,
                                                   Email = aa.email,
                                                   Senha = aa.senha,
                                                   UltimoAcesso = aa.ultimoAcesso == null ? "Sem data de ultimo acesso" : aa.ultimoAcesso.ToString(),
                                                   DataCriacao = aa.dataCriacao == null ? "Sem data de Criacao" : aa.dataCriacao.ToString(),
                                                   Turma = jj.descricao == null ? "Nenhuma turma" : jj.descricao,
                                                   DataNascimento = aa.dataNascimento == null ? "Nenhuma data de nascimento" : aa.dataNascimento.ToString(),
                                                   Status = ii.descricao,
                                                   UrlFoto = aa.urlFoto,
                                                   Cep = aa.cep,
                                                   Rua = aa.rua,
                                                   Bairro = aa.bairro,
                                                   Cidade = aa.cidade,
                                                   Celular = aa.celular,
                                                   Descricao = aa.descricao,
                                                   HaveNote = aa.haveNote == true ? "Tem notebook" : "Não tem notebook",
                                                   Genero = ff.decricao,
                                                   RG = aa.RG,
                                                   Cor = ee.decricao,
                                                   UF = dd.decricao,
                                                   HaveInternetHouse = aa.haveInternetHouse == true ? "Tem internet" : "Não tem internet",
                                                   RendaFamiliar = aa.rendaFamiliar,
                                                   QntdPessoas = aa.qntdPessoas,
                                                   Mensalidade = hh.descricao,
                                                   Cargo = aa.cargo,
                                                   Atividade = aa.atividade,
                                                   NumeroCasa = aa.numeroCasa,
                                                   Complemento = aa.complemento,
                                                   Unidade = gg.descricao,
                                                   DataPagamento = bb.descricao

                                               }).ToList();

                            return objCompleto;
                          */

            }
            catch (Exception)
            {

                throw;
            }
        }
        public AprovarResponse EditarUsuario(EditarUsuarioRequest request, int id)
        {
            try
            {
                AprovarResponse response = new AprovarResponse();
                if (request.dataVencimento != null)
                {
                    DateTime dataVencimento = request.dataVencimento ?? DateTime.Now;
                    tabPagamentoPresencial pagantes = _ctx.tabPagamentoPresencial.Where(x => x.usuarioCodigo == id).FirstOrDefault();
                    var objUsuario = _ctx.TabUsuario.Where(c => c.codigo == id).FirstOrDefault();
                    int mensalidadeCodigo = objUsuario.mensalidade ?? 0;
                    var objValor = _ctx.tabMensalidade.Where(c => c.codigo == mensalidadeCodigo).FirstOrDefault();
                    if (objValor.valor > 0)
                    {
                        if (pagantes == null)
                        {
                            pagantes = new tabPagamentoPresencial()
                            {
                                usuarioCodigo = id,
                                competencia = DateTime.Now.Month.ToString().PadLeft(2, '0') + '/' + DateTime.Now.Year.ToString(),
                                dataVencimento = dataVencimento,
                                valor = objValor.valor
                            };
                        }
                        else
                        {
                            pagantes.dataVencimento = dataVencimento;
                        }
                        _ctx.tabPagamentoPresencial.Add(pagantes);
                        _ctx.SaveChanges();
                    }
                }

                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                if (request.status == 2)
                {
                    Desativar(usuario.codigo);
                }
                usuario.nome = request.nome;
                usuario.email = request.email;
                usuario.senha = request.senha;
                usuario.status = request.status;
                usuario.dataPagamento = request.dataPagamento;
                usuario.turmaCodigo = request.turma;
                usuario.id_Posicao = request.posicao;
                usuario.mensalidade = request.mensalidade;
                _ctx.SaveChanges();
                response.mensagem = "Usuario editado com sucesso";
                response.sucesso = true;
                return response;
            }
            catch (Exception ex)
            {
                AprovarResponse response = new AprovarResponse();
                response.mensagem = "Houve algum problema ao editar o usuário";
                response.sucesso = false;
                return response;
            }
        }


        public List<TabPosicaoAluno> ObterPosicoesDisponiveisPorTurma(int turmaCodigo, int? userId)
        {
            try
            {
                List<TabPosicaoAluno> posicao = new List<TabPosicaoAluno>();
                var lst_Posicao_id = new List<int?>();
                if (userId == null)
                    lst_Posicao_id = _ctx.TabUsuario.Where(c => c.turmaCodigo == turmaCodigo && (c.status == 1 || c.status == 6)).Select(x => x.id_Posicao).ToList();
                else
                    lst_Posicao_id = _ctx.TabUsuario.Where(c => c.turmaCodigo == turmaCodigo && (c.status == 1 || c.status == 6) && c.codigo != userId).Select(x => x.id_Posicao).ToList();
                var lstPosicaoAluno = _ctx.TabPosicaoAluno.Where(c => !lst_Posicao_id.Contains(c.id) && c.isNoteVar == true && c.isAtivo == true).ToList();
                if (lstPosicaoAluno.Count == 0)
                {
                    return posicao;
                }
                return lstPosicaoAluno;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Desativar(int id)
        {

            EmailService emailService = new EmailService();
            var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
            var statusAntigo = usuario.status;
            var historico = new tabUsuarioHistorico()
            {
                usuarioCodigo = usuario.codigo,
                dataStatus = DateTime.Now,
                observacao = "Aluno desativado",
                statusAntigo = statusAntigo,
                statusNovo = 2
            };
            usuario.status = 2;
            _ctx.tabUsuarioHistorico.Add(historico);
            _ctx.SaveChanges();
            var Template = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Content/EmailHtml/Desativar.html";
            var htmlTemplate = System.IO.File.ReadAllText(Template);
            var htmlArrumado = htmlTemplate.Replace("@Nome", usuario.nome.Split(" ")[0])
                .Replace("@Turma", _ctx.TabTurma.FirstOrDefault(x => x.codigo == usuario.turmaCodigo).descricao)
                .Replace("@NumeroContato", _ctx.tabUnidade.FirstOrDefault(x => x.codigo == usuario.unidadeCodigo).telContato);
            emailService.EnviaEmailParametrizavelInstituto("Var Solutions", usuario.email, "Matrícula desativada", htmlArrumado);
        }

        public List<ObterUsuario> ObterEditarUsuario(int id)
        {
            try
            {
                List<ObterUsuario> users = new List<ObterUsuario>();
                var usuario = _ctx.TabUsuario.Where(x => x.codigo == id).ToList();
                var mensalidade = _ctx.tabMensalidade.ToList();
                var status = _ctx.TabStatus.ToList();
                var turma = _ctx.TabTurma.ToList();
                var posicao = _ctx.TabPosicaoAluno.ToList();
                var dataPagamento = _ctx.tabDiaPagamento.ToList();
                foreach (var item in usuario)
                {
                    var objMen = mensalidade.Where(x => x.codigo == item.mensalidade).ToList();
                    var objStatus = status.Where(x => x.codigo == item.status).ToList();
                    var objTurma = turma.Where(x => x.codigo == item.turmaCodigo).ToList();
                    var objPosicao = posicao.Where(x => x.id == item.id_Posicao).ToList();
                    var objDataPagamento = dataPagamento.Where(x => x.codigo == item.dataPagamento).ToList();
                    var objturmas = turma.Where(x => x.codUnidade == 1 && x.isAtivo == true).ToList();
                    users.Add(new ObterUsuario
                    {
                        Mensalidade = objMen,
                        Status = objStatus,
                        CodigoTurma = objTurma,
                        Posicao = objPosicao,
                        DataPagamento = objDataPagamento,
                        TurmasDisponiveis = objturmas
                    });

                }
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TabUsuario ObterPerfilAdminisrtativo(int id)
        {
            return _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
        }
        public tabPagamentoPresencial ObterPrimeiroPagamento(int usuarioCodigo)
        {
            var user = _ctx.tabPagamentoPresencial.OrderBy(y => y.dataVencimento).FirstOrDefault(x => x.usuarioCodigo == usuarioCodigo);
            return user;
        }
        public List<Convocados> ObterConvocados(int turmaCodigo = 0)
        {
            try
            {
                List<TabUsuario> objUsuario = new();
                if (turmaCodigo == 0)
                    objUsuario = _ctx.TabUsuario.Where(c => c.status == 6).ToList();
                else
                    objUsuario = _ctx.TabUsuario.Where(c => c.status == 6 && c.turmaCodigo == turmaCodigo).ToList();
                var userhistorico = _ctx.tabUsuarioHistorico.ToList();
                var mensalidade = _ctx.tabMensalidade.ToList();
                var posicao = _ctx.TabPosicaoAluno.ToList();
                var turma = _ctx.TabTurma.ToList();
                List<Convocados> convocado = new List<Convocados>();
                foreach (var item in objUsuario)
                {
                    string descPosicao = posicao.FirstOrDefault(x => x.id == item.id_Posicao)?.descricao;
                    string descMensalidade = mensalidade.FirstOrDefault(x => x.codigo == item.mensalidade)?.descricao;
                    string descTurma = turma.FirstOrDefault(x => x.codigo == item.turmaCodigo)?.descricao;
                    string descLoc = turma.FirstOrDefault(x => x.codigo == item.turmaCodigo)?.localizacao;
                    convocado.Add(new Convocados
                    {
                        codigo = item.codigo,
                        nome = item.nome,
                        posicao = descPosicao,
                        turma = descTurma,
                        localizacao = descLoc,
                        telefone = item.celular,
                        dataEmailEnviado = item.dataEmailEnviado,
                        mensalidade = descMensalidade,
                        isMensagemEnviada = userhistorico.Any(x => x.usuarioCodigo == item.codigo && x.observacao == "Mensagem no whatsApp enviada"),
                        isJaVeio = userhistorico.Any(x => x.usuarioCodigo == item.codigo && x.statusAntigo == 6 && x.statusNovo == 3)
                    });

                }
                return convocado;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AprovarResponse VoltarParaListaEspera(int codigo)
        {
            AprovarResponse response = new AprovarResponse();
            var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == codigo);
            var userHistorico = _ctx.tabUsuarioHistorico.FirstOrDefault(x => x.usuarioCodigo == codigo);
            var turma = _ctx.TabTurma.ToList();
            var descTurma = turma.FirstOrDefault(x => x.codigo == usuario.turmaCodigo)?.descricao;
            usuario.id_Posicao = null;
            usuario.status = 3;
            userHistorico.observacao = null;
            _ctx.SaveChanges();
            response.mensagem = usuario.nome + " voltou para lista de espera na turma: " + descTurma;
            response.sucesso = true;
            return response;
        }
        public AprovarResponse MensagemEnviada(int codigo)
        {
            AprovarResponse response = new AprovarResponse();
            var userHistorico = _ctx.tabUsuarioHistorico.FirstOrDefault(x => x.usuarioCodigo == codigo);
            if (userHistorico != null)
            {
                userHistorico.observacao = "Mensagem no whatsApp enviada";
                _ctx.SaveChanges();
                response.mensagem = "Marcado como enviada!";
                response.sucesso = true;
                return response;
            }
            else
            {
                response.mensagem = "Não foi possivel marcar como enviada contate o suoporte";
                response.sucesso = false;
                return response;
            }
        }
        public List<PagantesAdm> ObterPagantes(int turmaCodigo = 0)
        {
            try
            {
                List<PagantesAdm> pagador = new List<PagantesAdm>();
                var pagantes = _ctx.tabPagamentoPresencial.Where(x => x.dataLiquidacao == null).ToList();
                var usuarios = _ctx.TabUsuario.ToList();
                if (turmaCodigo == 0)
                {
                    foreach (var item in pagantes)
                    {
                        var nome = usuarios.FirstOrDefault(x => x.codigo == item.usuarioCodigo)?.nome;
                        pagador.Add(new PagantesAdm
                        {
                            codigo = item.codigo,
                            nome = nome,
                            mensalidade = item.valor,
                            dataVencimento = item.dataVencimento,
                        });
                    }
                }
                else
                {
                    foreach (var item in pagantes)
                    {
                        var nome = usuarios.FirstOrDefault(x => x.codigo == item.usuarioCodigo && x.turmaCodigo == turmaCodigo)?.nome;
                        pagador.Add(new PagantesAdm
                        {
                            codigo = item.codigo,
                            nome = nome,
                            mensalidade = item.valor,
                            dataVencimento = item.dataVencimento,
                        });
                    }
                }
                return pagador;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AprovarResponse DarBaixa(DarBaixaRequest request, int codigo)
        {
            try
            {
                AprovarResponse response = new AprovarResponse();
                var user = _ctx.tabPagamentoPresencial.FirstOrDefault(x => x.codigo == codigo);
                if (user == null)
                {
                    response.mensagem = "Esse codigo de usuario não existe";
                    response.sucesso = false;
                    return response;
                }
                user.dataLiquidacao = request.dataLiquidacao;
                user.formaPagamento = request.formaPagamento;
                user.valorPago = request.valorPago;
                user.Observacao = request.observacao;
                user.usuarioBaixa = request.usuarioBaixa;

                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == request.usuarioBaixa)?.nome;

                _ctx.tabPagamentoPresencial.Update(user);
                _ctx.SaveChanges();
                response.mensagem = "Deu baixa com sucesso" + usuario + " .";
                response.sucesso = true;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UnidadeResponse InserirUnidade(UnidadeRequest request)
        {
            var response = new UnidadeResponse();
            try
            {

                TabUnidade tabUnidade = new TabUnidade()
                {
                    descricao = request.descricao,
                    qtdNote = request.qtdNote,
                    telContato = request.telContato
                };

                _ctx.tabUnidade.Add(tabUnidade);
                _ctx.SaveChanges();

                response.status = true;
                response.message = "Parabens!! Unidade inserida com sucesso!";
                return response;
            }
            catch
            {
                response.status = false;
                response.message = "Infelizmente não foi possivel inserir uma unidade!";
                return response;
            }
        }

        public List<TabUnidade> BuscarUnidades()
        {
            try
            {
                var unidades = _ctx.tabUnidade.ToList();

                return unidades;
            }
            catch
            {
                return null;
            }
        }

        public UnidadeResponse AtualizarUnidade(UnidadeRequest request, int id)
        {
            var response = new UnidadeResponse();
            try
            {
                var unidadeIgual = _ctx.tabUnidade.FirstOrDefault(x => x.descricao == request.descricao && x.qtdNote == request.qtdNote && x.telContato == request.telContato);
                if (unidadeIgual != null)
                {
                    response.status = false;
                    response.message = "A unidade com essas informações já é existente!";
                    return response;
                }

                var unidadeExistente = _ctx.tabUnidade.FirstOrDefault(x => x.codigo == id);
                if (unidadeExistente == null)
                {
                    response.status = false;
                    response.message = "Unidade não encontrada, Id incoerente!";
                    return response;
                }

                unidadeExistente.qtdNote = request.qtdNote;
                unidadeExistente.descricao = request.descricao;
                unidadeExistente.telContato = request.telContato;

                _ctx.tabUnidade.Update(unidadeExistente);
                _ctx.SaveChanges();

                response.status = true;
                response.message = "parabens!! Unidade Atualizada com sucesso!";
                return response;
            }
            catch
            {
                response.status = false;
                response.message = "Não foi possivel realizar a requisição";
                return response;
            }
        }

        public UnidadeResponse DeletarUnidade(int id)
        {
            var response = new UnidadeResponse();
            try
            {
                var unidadeExistente = _ctx.tabUnidade.FirstOrDefault(x => x.codigo == id);
                if (unidadeExistente == null)
                {
                    response.status = false;
                    response.message = "Unidade não encotrada, Id incoerente";
                    return response;
                }

                _ctx.tabUnidade.Remove(unidadeExistente);
                _ctx.SaveChanges();

                response.status = true;
                response.message = "parabens!! Unidade deletada com sucesso!";
                return response;
            }
            catch
            {
                response.status = false;
                response.message = "Não foi possivel realizar a requisição";
                return response;
            }
        }

        public bool VincularCursoUnidade(VinculoRequest request)
        {
            try
            {
                if (_ctx.TabUnidade_Curso.Any(x => x.cursoCodigo == request.cursoCodigo && x.unidadeCodigo == request.codigoUnidade))
                    return false;

                var unidadeExistente = _ctx.tabUnidade.FirstOrDefault(x => x.codigo == request.codigoUnidade);
                var cursoExistente = _ctx.TabCurso.FirstOrDefault(x => x.codigo == request.cursoCodigo);

                if (unidadeExistente == null || cursoExistente == null)
                {
                    return false;
                }

                TabUnidade_Curso vinculoTabUnidade_Curso = new TabUnidade_Curso()
                {
                    unidadeCodigo = request.codigoUnidade,
                    cursoCodigo = request.cursoCodigo
                };

                _ctx.TabUnidade_Curso.Add(vinculoTabUnidade_Curso);
                _ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DesvincularCursoUnidade(int id)
        {
            try
            {
                var unidadeVinculada = _ctx.TabUnidade_Curso.FirstOrDefault(x => x.codigo == id);
                if (unidadeVinculada == null)
                {
                    return false;
                }

                _ctx.TabUnidade_Curso.Remove(unidadeVinculada);
                _ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<UnidadesVinculadasResponse> BuscarUnidadesVinculadas()
        {
            try
            {
                var cursos = _ctx.TabCurso.ToList();
                var unidades = _ctx.tabUnidade.ToList();
                var unidadesCurso = _ctx.TabUnidade_Curso.ToList();
                List<UnidadesVinculadasResponse> response = new();

                foreach (var item in unidadesCurso)
                {
                    response.Add(new UnidadesVinculadasResponse
                    {
                        codigo = item.codigo,
                        curso = cursos.FirstOrDefault(x => x.codigo == item.cursoCodigo).descricao,
                        unidade = unidades.FirstOrDefault(x => x.codigo == item.unidadeCodigo).descricao
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                // Log ou manipulação de erro, se necessário
                return null;
            }
        }
    }
}







