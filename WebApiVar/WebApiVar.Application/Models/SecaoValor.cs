using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class SecaoValor
    {
        public int codigoSecao { get; set; }

        public string secao { get; set; }

        public string? tipo { get; set; }

        public string? valor { get; set; }

        public bool isativo { get; set; }


    }
}
