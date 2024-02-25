using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabEstilizacao
    {
        [Key]
        public int codigo { get; set; }
        public string css { get; set; }
        public bool isAtivo { get; set; }
        public string descricao { get; set; }
    }
}
