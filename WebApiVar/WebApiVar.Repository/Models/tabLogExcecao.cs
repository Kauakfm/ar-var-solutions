using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabLogExcecao
    {
        [Key]
        public int codigo { get; set; } 
        public string excecao { get; set; }
        public DateTime dataHora { get; set; }
        public string referencia { get; set; }


    }
}
