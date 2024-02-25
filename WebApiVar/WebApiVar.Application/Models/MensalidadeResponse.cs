using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class MensalidadeResponse
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }

        public int mensalidade { get; set; }    
       
    }
}
