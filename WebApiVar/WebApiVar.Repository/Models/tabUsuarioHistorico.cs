using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabUsuarioHistorico
    {
        [Key]
        public long codigo { get; set; } 
        public int usuarioCodigo { get; set; }
        public DateTime dataStatus { get; set; }
        public string? observacao { get; set; }
        public int? statusAntigo { get; set; }
        public int? statusNovo { get; set; }
        public string? observacaoWhatsapp { get; set; }  
    }
}
