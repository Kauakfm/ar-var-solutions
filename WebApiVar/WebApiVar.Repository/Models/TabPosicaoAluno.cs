using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabPosicaoAluno
    {
        [Key]
        public int id { get; set; }
        public string descricao { get; set; } 
        public bool isNoteVar { get; set; }
        public bool isAtivo { get; set; }
    }
}
