using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabCupomCursoOnline
    {
        [Key]
        public int codigo { get; set; }
        public string cupom { get; set; }
        public int valorCupomPorcento { get; set; }
        public DateTime dataVencimento { get; set; }
    }
}
