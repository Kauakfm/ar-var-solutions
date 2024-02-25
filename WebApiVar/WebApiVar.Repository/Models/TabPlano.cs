using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabPlano
    {
        [Key]
        public int codigo { get; set; }
        public int? usuarioCodigo { get; set; }

        public DateTime? diaInicializacao { get; set; }

        public int? diaPagamento { get; set; }
        public int? status { get ; set; }

        public int? mensalidadeCodigo { get; set; }

        public DateTime? dataEncerramento { get; set; }

        public string? descricaoDesistencia { get; set; }

    }
}
