using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public int status { get; set; }
        public int usuarioid { get; set; }
        public string nome { get; set; }
        public bool isEmail { get; set; }

        public bool isAluno { get; set; }

        public string? urlFoto { get; set; }

        public int parcelasEmAtraso { get; set; }   
        public int diasEmAtraso { get; set; }
        public bool isPagamentosEmDia { get; set; }

        public DateTime ultimoAcesso { get; set; }   

        public DateTime? diaPagamentoCursoRecorrente { get; set; }
        public DateTime? diapagamentoPresencial { get; set; }
    }
}
