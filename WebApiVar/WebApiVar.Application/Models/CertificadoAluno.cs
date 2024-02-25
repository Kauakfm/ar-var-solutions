using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class CertificadoAluno
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string nomeAluno { get; set; }
        public string dataConclusao { get; set; } 
        public string nomeAssinatura { get; set; }
        public string urlAssinatura { get; set; }
        
        public int usuarioCodigo { get; set; }
        public string codigoCertificado { get; set; }

    }
}
