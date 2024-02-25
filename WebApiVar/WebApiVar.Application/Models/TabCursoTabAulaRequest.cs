using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class TabCursoTabAulaRequest
    {
        public int aulaCodigo { get; set; }
        public int cursoCodigo { get; set; }
        public int ordem { get; set; }
    }
}
