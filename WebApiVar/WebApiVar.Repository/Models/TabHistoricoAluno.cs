using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabHistoricoAluno
    {
        [Key]
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idStatus { get; set; }
        public DateTime data { get; set; }
    }
}
