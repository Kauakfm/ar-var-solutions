using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabMaterial
    {
        [Key]
        public int id { get; set; }
        public string url { get; set; }
        public string nome { get; set; } = string.Empty;
    }
}
