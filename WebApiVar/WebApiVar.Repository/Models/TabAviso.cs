using System.ComponentModel.DataAnnotations;

namespace WebApiVar.Repository.Models
{
    public class TabAviso
    {
        [Key]
        public int codigo { get; set; }
        public int codigoUnidade { get; set; }
        public string url { get; set; }
        public int tipo { get; set; }
        public bool ativo { get; set; }

    }
}
