using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class ObterCertificados
    {
        public string nome { get; set; }
        public string modulo { get; set; }
        public string urlCertificado { get; set; }
        public string nomeModulo { get; set; }
        public DateTime? dataConclusao { get; set; }

    }
}
