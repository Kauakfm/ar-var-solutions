using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class VagasModel
    {
        public int Qtd { get; set; }
        public string Turma { get; set; }
        public int IdTurma { get; set; }
        public bool hasnote { get; set; }

        public int espera { get; set; } 
    }
}
