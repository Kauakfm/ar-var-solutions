using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class InscricaoRequest
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public DateTime dataCriacao { get; set; }
        public int turmaCodigo { get; set; }        
        public DateTime dataNascimento { get; set; }
        public int CEP { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string celular { get; set; }
        public string descricao { get; set; }
    }
}
