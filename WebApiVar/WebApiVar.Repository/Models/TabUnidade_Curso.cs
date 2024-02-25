using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabUnidade_Curso
    {
        [Key]
        public int codigo { get; set; }
        public int unidadeCodigo { get; set; }
        public int cursoCodigo { get; set; }

    }
}
