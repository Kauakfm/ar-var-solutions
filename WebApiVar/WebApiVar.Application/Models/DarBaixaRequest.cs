using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebApiVar.Application.Models
{
    public class DarBaixaRequest
    {
        public int formaPagamento { get; set; }

        public DateTime dataLiquidacao { get; set; }

        public decimal valorPago { get; set; }

        public string observacao { get; set; }

        public int usuarioBaixa { get; set; }




    }
}
