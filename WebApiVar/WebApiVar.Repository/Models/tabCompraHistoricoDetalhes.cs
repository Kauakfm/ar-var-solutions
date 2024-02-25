using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabCompraHistoricoDetalhes
    {
        [Key]
        public int codigo { get; set; }
        public int compraHistoricoCodigo { get; set; }
        public DateTime dataHora { get; set; }
        public string? observacao { get; set; }
        public string? documentoAuxiliar { get; set; }

    }
}
