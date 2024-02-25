using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class CursoRequest
    {
        public string titulo { get; set; }
        public string detalhes { get; set; }
        public string? Logo { get; set; }
        public int? cargaHoraria { get; set; }
    }
}
