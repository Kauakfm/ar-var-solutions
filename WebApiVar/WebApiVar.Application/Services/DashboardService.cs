using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class DashboardService
    {
        private readonly decimal renda_por_pessoa_gratuito = 700;
        private readonly decimal renda_por_pessoa_nivel_2 = 900;
        private readonly decimal renda_por_pessoa_nivel_3 = 1100;
        private readonly decimal renda_por_pessoa_nivel_4 = 1300;

        private readonly VAREntities _ctx;
        public DashboardService(VAREntities ctx)
        {
            _ctx = ctx;
        }
        public TabUsuario ObterPerfil(int id)
        {
            return _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
        }
        public int ObterQtdComentarios(int usuarioCodigo)
        {
            return _ctx.TabAulaComentario.Where(x => x.usuarioCodigo == usuarioCodigo).Count();
        }
        public bool EditarSobreMim(SobreMim request, int id)
        {
            try
            {
                var user = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                user.sobreMim = request.Sobremim;

                _ctx.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public DashboardResponse EditarPerfil(DashboardRequest request, int id)
        {
            var dash = new DashboardResponse();
            try
            {
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                if (usuario == null)
                {
                    dash.sucesso = false;
                    dash.mensagem = "Não achou o usuario";
                    return dash;
                }
                usuario.nome = request.nome;
                usuario.cargo = request.cargo;
                usuario.dataNascimento = request.dataNascimento;
                usuario.genero = request.genero;
                usuario.cep = request.CEP;
                usuario.rua = request.rua;
                usuario.bairro = request.bairro;
                usuario.cidade = request.cidade;
                usuario.celular = request.celular;
                usuario.RG = request.RG;
                usuario.CPF = request.CPF;
                usuario.cor = request.Cor;
                usuario.UF = request.UF;
                usuario.haveNote = request.haveNote;
                usuario.haveInternetHouse = request.haveInternetHouse;
                usuario.numeroCasa = request.numeroCasa;
                usuario.complemento = request.complemento;
                usuario.sobreMim = request.sobreMim;
                usuario.atividade = request.atividade;

                if (_ctx.TabUsuario.Any(x => x.codigo != id && x.CPF == request.CPF))
                {
                    dash.sucesso = false;
                    dash.mensagem = "CPF já existe no banco.";
                    return dash;
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
                    dash.sucesso = false;
                    dash.mensagem = "A idade minima permitida é de 16 anos";
                    return dash;
                }
                string rg = request.RG;
                if (rg.Length <= 4)
                {
                    dash.sucesso = false;
                    dash.mensagem = "Minimo de caracteres permitidos do RG é 5";
                    return dash;
                }
                string celular = request.celular;
                if (celular.Length != 11)
                {
                    dash.sucesso = false;
                    dash.mensagem = "Minimo de caracteres permitidos do celular é 11";
                    return dash;
                }
                int genero = request.genero;
                if (genero == 0) 
                {
                    dash.sucesso = false;
                    dash.mensagem = "Genero veio 0";
                    return dash;
                }
                if (request.Cor == 0)
                {
                    dash.sucesso = false;
                    dash.mensagem = "cor veio 0";
                }
                if(request.complemento == "")
                {
                    dash.sucesso = false;
                    dash.mensagem = "Complemento vaio vazio";
                    return dash;
                }
                if(request.numeroCasa == "")
                {
                    dash.sucesso = false;
                    dash.mensagem = "numero casa veio vazio";
                }
                string cleanCPF = request.CPF.Replace(".", "").Replace("-", "");
                usuario.CPF = cleanCPF;

                RendaFamiliar(request.renda, request.pessoas, id);
                _ctx.TabUsuario.Update(usuario);
                _ctx.SaveChanges();
                dash.sucesso = true;
                dash.mensagem = "Dados atualizados com sucesso.";
               
                return dash;
            }
            catch (Exception ex)
            {
                dash.sucesso = false;
                dash.mensagem = "Não foi possivel atualizar os dados";
                return dash;
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
        public List<AlunosDestaque> ObterAlunosBons()
        {
            var concluido = _ctx.TabConclusao.ToList();
            var usuarios = _ctx.TabUsuario.ToList();
            var concluidousuario = _ctx.TabConclusao.ToList().GroupBy(x => x.usuarioId);
            List<AlunosDestaque> alunosquedaoparaogasto = new();

            foreach (var item in concluidousuario)
            {
                alunosquedaoparaogasto.Add(
                    new AlunosDestaque
                    {
                        qtd = concluido.Where(x => x.usuarioId == item.Key).Count(),
                        nome = usuarios.FirstOrDefault(x => x.codigo == item.Key)
                    }
                );
            }
            return alunosquedaoparaogasto.OrderByDescending(x => x.qtd).ToList();
        }

        public List<AlunosDestaque> ObterAlunosBonsNaSemana()
        {
            DateTime hoje = DateTime.Today;
            DateTime dataInicioSemana = hoje.AddDays(-(int)hoje.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime dataFimSemana = dataInicioSemana.AddDays(6);

            var conclusoesNaSemana = _ctx.TabConclusao
                .Where(x => x.data >= dataInicioSemana && x.data <= dataFimSemana)
                .ToList();

            var usuarios = _ctx.TabUsuario.ToList();
            var concluidosPorUsuario = conclusoesNaSemana.GroupBy(x => x.usuarioId);

            List<AlunosDestaque> alunosDestaqueSemana = new List<AlunosDestaque>();

            foreach (var item in concluidosPorUsuario)
            {
                alunosDestaqueSemana.Add(
                    new AlunosDestaque
                    {
                        qtd = item.Count(),
                        nome = usuarios.FirstOrDefault(x => x.codigo == item.Key)
                    }
                );
            }

            return alunosDestaqueSemana.OrderByDescending(x => x.qtd).ToList();
        }
    }
}



