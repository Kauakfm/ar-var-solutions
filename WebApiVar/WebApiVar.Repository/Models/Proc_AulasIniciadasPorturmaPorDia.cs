using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace WebApiVar.Repository.Models
{
    public class Proc_AulasIniciadasPorturmaPorDia
    {
        public int percentualFaltas { get; set; }
        public int totalregistros { get; set; }
        public int totalFaltas { get; set; }
        public DateTime DataAcesso { get; set; }
        public string TURMA { get; set; }
        public string nome { get; set; }
        public string Posicao { get; set; }
        public string LocalLogin { get; set; }
        public DateTime? PrimeiraAulasConcluidas { get; set; }
        public DateTime? UltimoDiaAulaConcluida { get; set; }
        public int TotalAulasConcluidas { get; set; }
        public int AulasConcluidas { get; set; }
        public int AulasIniciadas { get; set; }
        public string? chamada { get; set; }
        public string? observacao { get; set; }
        [Key]
        public int usuarioCodigo { get; set; }


    }
}