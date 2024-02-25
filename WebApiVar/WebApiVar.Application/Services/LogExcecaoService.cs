using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApiVar.Application.Models;
using WebApiVar.Repository;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Services
{
    public class LogExcecaoService
    {
        private readonly VAREntities _ctx;

        public LogExcecaoService(VAREntities context)
        {
            _ctx = context;
        }
        public void gravarLog(Exception ex, string referencia)
        {
            try
            {
                tabLogExcecao objLog = new tabLogExcecao
                {
                    dataHora = DateTime.Now,
                    excecao = ex.Message,
                    referencia = referencia
                };
                _ctx.tabLogExcecao.Add(objLog);

            }
            catch (Exception)
            {
            }
        }
    }
}
