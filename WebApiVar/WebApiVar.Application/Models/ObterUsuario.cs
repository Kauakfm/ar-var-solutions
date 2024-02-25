using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Models
{
    public class ObterUsuario
    {
        public List<TabStatus> Status { get; set; } 
        
        public List<TabPosicaoAluno> Posicao { get; set; }

        public List<TabTurma> CodigoTurma { get; set; }

        public List<tabMensalidade> Mensalidade { get; set; }

        public List<tabDiaPagamento> DataPagamento { get; set; }

        public List<TabTurma> TurmasDisponiveis { get; set; }

    }
}
