using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class AutenticacaoService
    {
        private readonly VAREntities _ctx;
        public AutenticacaoService(VAREntities ctx)
        {
            _ctx = ctx;
        }
        public LoginResponse VerificarLogin(LoginRequest request)
        {
            try
            {
                DateTime? diapag = null;
                DateTime? diapagamentoPresencial = null;
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.email == request.email && x.senha == request.password && x.status != 2 && x.isEmail == true);
                if (usuario == null)
                {
                    return null;
                }
                else
                {

                    bool isPagamentosEmDia = true;
                    int diasEmAtraso = 0;
                    int parcelasEmAtraso = 0;

                    try
                    {
                        PagamentoService pagServ = new PagamentoService(_ctx);
                        bool isCancel = false;
                        if (usuario.unidadeCodigo == 1)
                            isPagamentosEmDia = pagServ.ValidarPagamento(usuario.codigo, out parcelasEmAtraso, out diasEmAtraso, out diapagamentoPresencial);
                        else if(usuario.unidadeCodigo == 3)
                            isPagamentosEmDia = pagServ.ValidarPagamentoOnline(usuario.codigo,out diapag, out isCancel);                       

                    }
                    catch (Exception ex) { }

                    tabUsuarioAcessos objUACessos = new tabUsuarioAcessos
                    {
                        usuarioCodigo = usuario.codigo,
                        dataStatus = DateTime.Now,
                        isPresencial = request.presencial == "true" ? true : false
                    };
                    _ctx.Add(objUACessos);

                    if (request.presencial == "true")
                    {
                        DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        DateTime dataFinal = dataInicial.AddDays(1).AddSeconds(-1);
                        TabAlunoPresenca tabAlunoPresenca = _ctx.TabAlunoPresenca.FirstOrDefault(c => c.DataPresenca >= dataInicial && c.DataPresenca <= dataFinal && c.usuarioCodigo == usuario.codigo);


                        if (tabAlunoPresenca == null || tabAlunoPresenca.codigo == 0)
                        {
                            tabAlunoPresenca = new TabAlunoPresenca
                            {
                                DataMarcacao = DateTime.Now,
                                DataPresenca = DateTime.Now,
                                posicaoCodigo = usuario.id_Posicao ?? null,
                                turmaCodigo = usuario.turmaCodigo ?? null,
                                statusPresencaCodigo = 1, //Presente
                                usuarioCodigo = usuario.codigo
                            };
                            _ctx.TabAlunoPresenca.Add(tabAlunoPresenca);
                        }
                    }

                    DateTime ultimoAcesso = usuario.ultimoAcesso ?? DateTime.Now;
                    usuario.ultimoAcesso = DateTime.Now;
                    _ctx.TabUsuario.Update(usuario);

                    _ctx.SaveChanges();



                    return new LoginResponse
                    {
                        token = GeraTokenJwt(usuario),
                        status = Convert.ToInt32(usuario.status),
                        usuarioid = usuario.codigo,
                        nome = usuario.nome,
                        isAluno = usuario.isAluno,
                        urlFoto = usuario.urlFoto,
                        isPagamentosEmDia = isPagamentosEmDia,
                        diasEmAtraso = diasEmAtraso,
                        parcelasEmAtraso = parcelasEmAtraso,
                        ultimoAcesso = ultimoAcesso,
                        diaPagamentoCursoRecorrente = diapag,
                        diapagamentoPresencial = diapagamentoPresencial
                    };
                }
            }
            catch (Exception ex)
            {
                LogExcecaoService objLogS = new LogExcecaoService(_ctx);
                objLogS.gravarLog(ex, "Metodo VerificarLogin na AutenticacaoService");

                return null;
            }

        }
        public PresencaResponse MarcarPresenca(int id)
        {

            var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
            var turmaUsuario = _ctx.TabTurma.FirstOrDefault(x => x.codigo == usuario.turmaCodigo);

            var response = new PresencaResponse();
            var turma = new TabTurma();
            if (usuario == null)
            {
                response.sucesso = false;
                return response;
            }
            else
            {
                if (!_ctx.TabPresenca.Any(x => x.codigoUsuario == usuario.codigo && x.data.Day == DateTime.Now.Day && x.data.Month == DateTime.Now.Month && x.data.Year == DateTime.Now.Year)
                    && turmaUsuario.diaAula.Split("-").Any(x => x == DateTime.Now.DayOfWeek.ToString())
                    && turmaUsuario.horarioFim > +(DateTime.Now.Hour * 60) + DateTime.Now.Minute)
                {
                    TabPresenca tabPresenca = new TabPresenca();
                    tabPresenca.data = DateTime.Now;
                    tabPresenca.codigoUsuario = usuario.codigo;

                    if (turmaUsuario.horarioInicio + 30 < (tabPresenca.data.Hour * 60) + tabPresenca.data.Minute)
                        response.mensagem = "Ta Atrasado parça";


                    _ctx.TabPresenca.Add(tabPresenca);



                    _ctx.SaveChanges();
                }

                response.sucesso = true;
                return response;
            }
        }
        public LoginResponse GerarTokenporId(int idusuario)
        {
            var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == idusuario);

            if (usuario == null)
            {
                return null;
            }
            else
            {
                return new LoginResponse
                {
                    token = GeraTokenJwt(usuario),
                    status = Convert.ToInt32(usuario.status),
                    usuarioid = usuario.codigo,
                    nome = usuario.nome,
                    isAluno = usuario.isAluno,
                    urlFoto = usuario.urlFoto

                };
            }
        }
        private string GeraTokenJwt(TabUsuario usuario)
        {
            var issuer = "var";
            var audience = "var";
            var key = "c013239a-5e89-4749-b0bb-07fe4d21710d";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("status", usuario.status.ToString()),
                new Claim("usuarioId", usuario.codigo.ToString()),
                new Claim("unidadeId",usuario.unidadeCodigo.ToString())
            };

            var token = new JwtSecurityToken(issuer: issuer, claims: claims, audience: audience, expires: DateTime.Now.AddMinutes(180), signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;

        }
    }
}
