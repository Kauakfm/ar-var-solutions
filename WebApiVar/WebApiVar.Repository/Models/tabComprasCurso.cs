using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabComprasCurso
    {
        [Key]
        public int codigo { get; set; }
        public int usuarioCodigo { get; set; }
        public DateTime data { get; set; }
        public int parcelas { get; set; }
        public int? tipo { get; set; }
        public string? assinaturaCodigo { get; set; }
    }
}
