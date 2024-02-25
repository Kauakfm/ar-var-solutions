using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{

    public class AulaRequest
    {        
        public string descricao { get; set; }
        public string detalhe { get; set; }
        public string urlAula { get; set; }
        public bool isAtivo { get; set; }
        public int tempoVideoSegundos { get; set; }        
        public int moduloId { get; set; }
        public string? urlAulaPanda { get; set; }       
    }
}
