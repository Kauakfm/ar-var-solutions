using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{    
    public class Vw_SituacaoAtualVagasPorTurma
    {
        public int Vagas { get; set; }
        public string descricao { get; set; }        
        public int codigoTurma { get; set; }
        public int qtdConvocados { get; set; }
        public int qtdAlunos { get; set; }
        public int qtdEspera { get; set; }
        public int VagasComNote { get; set; }
    }
}
