using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabCertificadosAlunos
    {
        [Key]

        public int codigo { get; set; }

        public int? usuarioCodigo { get; set; }

        public int? modulosCerficadosCodigo { get; set; }

        public string? urlCertificado { get; set; }
        
        public DateTime? dataGeracao { get; set; }
    }
}
