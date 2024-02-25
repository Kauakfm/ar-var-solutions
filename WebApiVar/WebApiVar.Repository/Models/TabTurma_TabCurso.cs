using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabTurma_TabCurso
    {
        [Key]
        public int codigo { get; set; }
        public int turmaCodigo { get; set; } 
        public int cursoCodigo { get; set; } 
        public int ordem { get; set; } 
    }
}
