using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabAula
    {
        [Key]
        public int codigo { get; set; } 
        public string descricao { get; set; }
        public string detalhe { get; set; }
        public string urlAula { get; set; } 
        public bool isAtivo { get; set; } 
        public DateTime dataCadastro { get; set; } 
        public int? tempoVideoSegundos { get; set; } 
        public string? arquivo { get; set; } 
        public bool? isDesafio { get; set; }
        public int? moduloId { get; set; }
        public string? urlAulapanda { get; set; }
    }
}
