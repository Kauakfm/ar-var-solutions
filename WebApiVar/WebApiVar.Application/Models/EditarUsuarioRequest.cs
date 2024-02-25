using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class EditarUsuarioRequest
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public int status { get; set; }
        public int? turma { get; set; }
        public int? posicao { get; set; }

        public int? mensalidade { get; set; }

        public int? dataPagamento { get; set;}

        public DateTime? dataVencimento { get; set; }



    }
}
