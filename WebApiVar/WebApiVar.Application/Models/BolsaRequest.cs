using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class BolsaRequest  : PlanoRequest
    {
        public DateTime dataNascimento { get; set; }
        public int CEP { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string celular { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public int Cor { get; set; }
        public int UF { get; set; }
        public bool haveNote { get; set; }
        public bool haveInternetHouse { get; set; }
        public string? descricao { get; set; }

        public int genero { get; set; }
        public decimal renda { get; set; }

        public int pessoas { get; set; }

        public int turmaCodigo { get; set; }

        public string complemento { get; set; }

        public string numeroCasa { get; set; }

        public int diaPagamento { get; set; }
    }
}