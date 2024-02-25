using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class CursoAmostra
    {
        public string Curso { get; set; }
        public List<AulasPorModuloAmostra> aulas { get; set; }
    }
}
