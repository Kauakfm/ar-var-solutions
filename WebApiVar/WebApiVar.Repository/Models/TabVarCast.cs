using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabVarCast
    {
        [Key]
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string detalhe { get; set; }
        public string urlAula { get; set; }
        public DateTime dataCadastro { get; set; }
        public int? tempoVideoSegundos { get; set; }


    }
}
