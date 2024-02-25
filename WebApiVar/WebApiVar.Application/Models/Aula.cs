using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class Aula
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string detalhe { get; set; }
        public DateTime dataCadastro { get; set; }
        public int? tempoVideoSegundos { get; set; }
        public string? arquivo { get; set; }
        public bool? isDesafio { get; set; }
        public int? moduloId { get; set; }
    }
}
