using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabmodulosCertificados
    {
        [Key]
        public int codigo { get; set; }

        public string? descricao { get; set; }

        public string? nomeAssinatura { get; set; }

        public string? urlAssinatura { get; set; }   

        public string? nomeModulo { get; set;}

    }
}
