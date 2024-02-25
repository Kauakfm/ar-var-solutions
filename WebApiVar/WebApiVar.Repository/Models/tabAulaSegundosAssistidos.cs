using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabAulaSegundosAssistidos
    {
        [Key]
        public int codigo { get; set; }
        public int aulaCodigo { get; set; }
        public int segundos { get; set; }
        public int usuarioCodigo { get; set; }
    }
}
