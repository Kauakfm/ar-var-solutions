using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabCompraHistorico
    {
        [Key]
        public int codigo { get; set; }
        public DateTime dataHora { get; set; }
        public string email { get; set; }
        public string? telefone { get; set; }
        public string cpf { get; set; }
        public string? cep { get; set; }
        public string? nResidencial { get; set; }
        public string? senha { get; set; }
        public string? cupomDesconto { get; set; }
        public string? cartaoCliente { get; set; }
        public string? observacao { get; set; }
        public string? documentoAuxiliar { get; set; }

    }
}
