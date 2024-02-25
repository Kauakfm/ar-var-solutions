using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabPagamentoPresencial
    {
        [Key]
        public int codigo { get; set; }
        public int usuarioCodigo { get; set; } 
        public string competencia { get; set; }
        public DateTime dataVencimento { get; set; }
        public decimal valor { get; set; }
        public DateTime? dataLiquidacao { get; set; }
        public int? formaPagamento { get; set; }
        public decimal? valorPago { get; set; }
        public int? usuarioBaixa { get; set; }
        public string? Observacao { get; set; }

    }
}
