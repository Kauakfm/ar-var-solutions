using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class MaterialService
    {
        private readonly VAREntities _ctx;

        public MaterialService(VAREntities context)
        {
            _ctx = context;
        }

        public List<TabMaterial> ObterMaterial(int id)
        {
            var aulaMaterial = _ctx.TabAulaMaterial.Where(x => x.idAula == id).ToList();
            var materialIds = aulaMaterial.Select(y => y.idMaterial).ToList();
            var materiais = _ctx.TabMaterial.Where(x => materialIds.Contains(x.id)).ToList();

            return materiais;
        }



    }
}
