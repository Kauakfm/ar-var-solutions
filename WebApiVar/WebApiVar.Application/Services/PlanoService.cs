using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class PlanoService
    {
        private readonly VAREntities _ctx;
        public PlanoService(VAREntities ctx)
        {
            _ctx = ctx;
        }
        public TabPlano ObterPlano(int usuCodigo)
        {
            try
            {
                var plano = _ctx.TabPlano.FirstOrDefault(x => x.codigo == usuCodigo);
                return plano;
              
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TabPlano> ObterPlanos()
        {
            try
            {
                var planos = _ctx.TabPlano.ToList();
                return planos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
     /*   public bool MudarPlano (PlanoRequest request,int codiogo) 
        {
            try
            {
                var plano = _ctx.TabPlano.FirstOrDefault(x => x.usuarioCodigo == codiogo);
                if(plano == null)
                    return false;
               plano.diaPagamento = request.,
                    plano.mensalidadeCodigo = request.,
                    plano.
            }
            catch (Exception ex)
            {
                return false;
            }
        }
     */
    }
}
