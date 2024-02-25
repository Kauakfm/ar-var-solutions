using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class ComentarioService
    {
        private readonly VAREntities _ctx;
        public ComentarioService(VAREntities ctx)
        {
            _ctx = ctx;
        }

        public List<ComentarioComNomeModel> VerComentarios(int aulanum)
        {
            var comentarios = _ctx.TabAulaComentario.Where(x => x.aulaCodigo == aulanum);
            var usuarios = _ctx.TabUsuario.ToList();
            var usuariosComComentarios = new List<ComentarioComNomeModel>();

            foreach (var comentario in comentarios)
            {
                var comentariocomnome = new ComentarioComNomeModel()
                {
                    comentario = comentario.comentario,
                    data = comentario.data,
                    nome = usuarios.FirstOrDefault(x => x.codigo == comentario.usuarioCodigo).nome
                };
                usuariosComComentarios.Add(comentariocomnome);

            }
            return usuariosComComentarios;
        }

        List<string> _palavroes = new List<string> { "fdp", "boquete", "punheta", "cú", "xoxota" };

        public bool SalvarComentario(ComentarioRequest request)
        {
            if (request == null)
                return false;

            if (!_ctx.TabAulaComentario.Any(x => x.comentario.ToLower() == request.comentario.ToLower() && x.usuarioCodigo == request.usuarioCodigo && x.aulaCodigo == request.aulaCodigo))
            {
                var comentariolimpo = request.comentario.ToLower();
                foreach (string palavrao in _palavroes)
                {
                    if (comentariolimpo.Contains(palavrao))
                        return false;
                }

                var addcomentario = _ctx.TabAulaComentario.Add(new TabAulaComentario
                {
                    aulaCodigo = request.aulaCodigo,
                    usuarioCodigo = request.usuarioCodigo,
                    comentario = request.comentario,
                    data = DateTime.Now
                });

                _ctx.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

