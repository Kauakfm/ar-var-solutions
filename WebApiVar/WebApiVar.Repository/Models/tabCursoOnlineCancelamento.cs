using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class tabCursoOnlineCancelamento
    {
        [Key]
        public int codigo { get; set; }
        public int? motivoCodigo { get; set; }
        public string? outroMotivo { get; set; }
        public DateTime dataCancelamento { get; set; }
    }
}
