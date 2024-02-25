using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Models
{
    public class Convocados
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string turma { get; set; }
        public string localizacao { get; set; }

        public string telefone { get; set; }
        public string posicao { get; set; }

        public DateTime? dataEmailEnviado { get; set; }

        public string mensalidade { get; set; }

        public bool isMensagemEnviada { get; set; }

        public bool isJaVeio { get; set; }
    }
}
