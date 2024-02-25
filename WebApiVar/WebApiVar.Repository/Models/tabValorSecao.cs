using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabValorSecao
    {
        [Key]   
        public int codigo { get; set; }

        public string? tipo { get; set; }

        public string? valor { get; set; }
    }
}
