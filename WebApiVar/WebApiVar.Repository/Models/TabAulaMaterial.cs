using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabAulaMaterial
    {
        [Key]
        public int id { get; set; }
        public int idAula { get; set; }
        public int idMaterial { get; set; }

    }
}
