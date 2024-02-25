using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabConclusao
    {
        [Key]
        public int id { get; set; } 
        public int usuarioId { get; set; } 
        public int aulaId { get; set; } 
        public DateTime data { get; set; }
        public string? linkTarefa { get; set; } = string.Empty;
        public bool? isAprovado { get; set; } 
    }
}
