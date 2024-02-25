using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabValorSecaoCodigo
    {
        [Key]
        public int codigo { get; set; }

        public int secaoCodigo { get; set; }

        public int valorSecaoCodigo { get; set; }
    }
}
