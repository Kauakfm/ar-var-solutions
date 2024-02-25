using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabCurso
    {
        [Key]
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string? detalhes { get; set; }
        public string? Logo { get; set; }
        public int? cargaHoraria { get; set; }
    }
}
