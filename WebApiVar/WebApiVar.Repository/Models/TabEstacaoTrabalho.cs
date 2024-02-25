using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabEstacaoTrabalho
    {
        [Key]
        public int id { get; set; }
        public int idEstacaoTrabalho { get; set; }
        public int nomeIdentificador { get; set; } 

    }
}
