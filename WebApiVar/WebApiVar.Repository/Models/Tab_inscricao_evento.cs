using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class Tab_inscricao_evento
    {
        [Key]
        public int id { get; set; }
        public string nome { get; set; } = string.Empty;  
        public string email { get; set; } = string.Empty;
        public string celular { get; set; } = string.Empty;

        public DateTime dataNascimento { get; set; }

        public int cep { get; set; } 
        public string rua { get; set; } 
        public string bairro { get; set; } = string.Empty;

        public string cidade { get; set; } = string.Empty;

        public string cpf { get; set; } = string.Empty;
        public string numero { get; set; } = string.Empty;
        public string complemento { get; set; } = string.Empty;
        public DateTime dataCad { get; set; }

        public DateTime dataCredenciamento { get; set; }

    }
}
