using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class InfoPlanoResponse
    {
        public string descricao { get; set; }   
        public decimal valor { get; set; }
        public DateTime? proxPagamento { get; set; }
        public DateTime? validade { get; set; }

    }
}
