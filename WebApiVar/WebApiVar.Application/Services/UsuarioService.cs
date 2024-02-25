using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{

    public class UsuarioService
    {
        private readonly VAREntities _ctx;
        public UsuarioService(VAREntities ctx)
        {
            _ctx = ctx;
        }
        public List<TabUsuario> ObterListaDeEspera()
        {
            try
            {
                return _ctx.TabUsuario.Where(x => x.status == 3 || x.status == 4).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AprovarMatricula(int id)
        {
            try
            {
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                usuario.status = 1;
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool RejeitarMatricula(int id)
        {
            try
            {
                var usuario = _ctx.TabUsuario.FirstOrDefault(x => x.codigo == id);
                usuario.status = 4;
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TabUsuario> PegarUsuarios()
        {
            var user = _ctx.TabUsuario.ToList();
            return user;
        }
        public List<TabUsuario> PegarUsuarios(int id)
        {
            var user = _ctx.TabUsuario.Where(x => x.codigo == id).ToList();
            return user;
        }
    }
}
