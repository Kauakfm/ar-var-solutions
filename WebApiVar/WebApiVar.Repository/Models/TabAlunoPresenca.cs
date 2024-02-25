using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabAlunoPresenca
    {
        [Key]
        public long codigo { get; set; }
        public int usuarioCodigo { get; set; }
        public int? posicaoCodigo { get; set; }
        public int? turmaCodigo { get; set; }
        public int statusPresencaCodigo { get; set; }
        public DateTime DataPresenca { get; set; }
        public DateTime DataMarcacao { get; set; }
        public string? observacao { get; set; }
    }
}
