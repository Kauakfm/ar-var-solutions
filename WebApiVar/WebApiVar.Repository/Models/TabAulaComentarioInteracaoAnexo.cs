using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabAulaComentarioInteracaoAnexo
    {
        [Key]
        public int codigo { get; set; }
        public int aulaComentarioInteracao { get; set; } 
        public string arquivo { get; set; } = string.Empty;
    }
}
