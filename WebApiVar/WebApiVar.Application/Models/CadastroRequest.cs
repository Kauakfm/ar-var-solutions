using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class CadastroRequest
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
    }
}
