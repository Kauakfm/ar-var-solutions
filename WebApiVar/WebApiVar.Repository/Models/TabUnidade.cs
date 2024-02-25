using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabUnidade
    {
        [Key]
        public int codigo { get; set; }
        public string? descricao { get; set; }
        public int? qtdNote { get; set; }
        public string? telContato { get; set; }

    }
}
