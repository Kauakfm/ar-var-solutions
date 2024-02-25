using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class ComentarioRequest
    {        
        public string comentario { get; set; }
        public int aulaCodigo { get; set; }

        public int usuarioCodigo { get; set; }        
    }
}
