using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class AvisosService
    {
        private readonly VAREntities _ctx;
        public AvisosService(VAREntities ctx)
        {
            _ctx = ctx;
        }        
        public List<TabAviso> ObterAvisos(int unidadecodigo)
        {
            var avisos = _ctx.TabAviso.Where(x => x.codigoUnidade == unidadecodigo && x.ativo).ToList();
            return avisos;
        }

        public List<TabPerguntaPesquisa> ObterPerguntas(int usuarioCodigo)
        {
            try
            {
                var objPesquisa = _ctx.tabPesquisa.FirstOrDefault(x => x.dataVencimento > DateTime.Now);    
                if (objPesquisa == null)
                {
                    return null;
                }
                else
                {
                    var codigoPesquisa = objPesquisa.codigo;
                    var objspergunta = _ctx.tabPerguntaPesquisa.Where(c => c.pesquisaCodigo == codigoPesquisa).ToList();
                    var respostas = _ctx.tabPerguntaPesquisaResposta.Where(x => x.usuarioCodigo == usuarioCodigo).ToList();
                    List<TabPerguntaPesquisa> perguntasResponse = new List<TabPerguntaPesquisa>();
                    foreach (var objpergunta in objspergunta)
                    {
                        if (!respostas.Any(x => x.perguntaCodigo == objpergunta.codigo))
                        {
                            perguntasResponse.Add(objpergunta);
                        }
                    }
                    return perguntasResponse;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool MarcarPerguntas(RespostaPesquisaRequest request, int usuarioCodigo)
        {
            try
            {
                _ctx.tabPerguntaPesquisaResposta.Add(new TabPerguntaPesquisaResposta
                {
                    perguntaCodigo = request.perguntaCodigo,
                    resposta = request.resposta,
                    usuarioCodigo = usuarioCodigo,
                });
                _ctx.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
