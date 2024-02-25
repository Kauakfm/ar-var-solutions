using Microsoft.EntityFrameworkCore;
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
    public class VarCastService
    {
        private readonly VAREntities _ctx;
        public VarCastService (VAREntities ctx)
        {
            _ctx = ctx;
        }

        public List<TabVarCast> ObterVarCast()
        {
            try
            {
                var cast = _ctx.TabVarCast.ToList();
                return cast;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
