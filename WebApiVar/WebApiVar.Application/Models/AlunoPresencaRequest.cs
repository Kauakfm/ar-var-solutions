using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class AlunoPresencaRequest
    {
        public int usuarioCodigo { get; set; }
        public int statusPresencaCodigo { get; set; }
        public DateTime DataPresenca { get; set; }        
        public string observacao { get; set; }
    }
}
