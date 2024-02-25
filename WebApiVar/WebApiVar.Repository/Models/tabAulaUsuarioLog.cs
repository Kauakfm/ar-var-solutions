using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApiVar.Repository.Models
{
    public class tabAulaUsuarioLog
    {
        [Key]
        public long codigo { get; set; }
        public int aulaCodigo { get; set; }
        public int usuarioCodigo { get; set; }
        public DateTime dataAcesso { get; set; }
    }
}
