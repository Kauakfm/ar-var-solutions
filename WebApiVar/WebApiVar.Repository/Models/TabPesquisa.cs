using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabPesquisa
    {
        [Key]

        public int codigo { get; set; }

        public DateTime dataInicio { get; set; }

        public DateTime dataVencimento { get; set; }

        public string descricao { get; set; }

    }
}
