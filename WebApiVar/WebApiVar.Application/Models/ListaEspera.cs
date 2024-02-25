using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class ListaEspera
    {
        public int codigo { get; set; }

        public string nome { get; set; }    

        public DateTime data { get; set; }

        public string descricaoTurma { get; set; }

    }
}
