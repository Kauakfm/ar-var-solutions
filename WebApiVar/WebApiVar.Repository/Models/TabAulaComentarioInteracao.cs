using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabAulaComentarioInteracao
    {
        [Key]
        public int codigo { get; set; } 
        public int aulaComentarioCodigo { get; set; }
        public string comentario { get; set; } 
        public int usuarioCodigo { get; set; } 
        public DateTime data { get; set; }
    }
}
