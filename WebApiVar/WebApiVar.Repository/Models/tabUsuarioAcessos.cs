using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabUsuarioAcessos
    {
        [Key]
        public int codigo { get; set; }
        public int usuarioCodigo { get; set; } 
        public DateTime dataStatus { get; set; }
        public bool isPresencial { get; set; } 

    }
}
