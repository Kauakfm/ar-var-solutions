using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabTurma
    {
        [Key]
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool isFinalizada { get; set; } 
        public DateTime dataInicio { get; set; } 
        public DateTime dataTermino { get; set; } 
        public int vagasComNote { get; set; }
        public string diaAula { get; set; }
        public int? horarioInicio { get; set; }

        public int? horarioFim { get ; set; }
        public string? localizacao { get; set; }
        public int codUnidade { get; set; }

        public bool isAtivo { get; set; } 
     }
}
