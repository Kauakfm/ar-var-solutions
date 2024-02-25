using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabPresenca
    {
        [Key]
        public int codigo { get; set; }
        public int codigoUsuario { get; set; } 
        public DateTime data {get; set; }
    }
}
