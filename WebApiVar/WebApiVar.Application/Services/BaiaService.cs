using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class BaiaService
    {
        private readonly VAREntities _ctx;

        public BaiaService(VAREntities context)
        {
            _ctx = context;
        }

        public List<TabPosicaoAluno> ObterPosicoes()
        {
            {
                return _ctx.TabPosicaoAluno.ToList();
            }
        }


    }
}
