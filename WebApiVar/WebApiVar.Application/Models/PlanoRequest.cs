using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class PlanoRequest
    {

        public int diaPagamento { get; set; } 
        public int mensalidade { get; set; } 
        public int codigousuario { get; set; }
        public int status { get; set; }     
        

    }
}
