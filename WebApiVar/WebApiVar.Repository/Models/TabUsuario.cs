using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabUsuario
    {
        [Key]
        public int codigo { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public DateTime? ultimoAcesso { get; set; }
        public DateTime dataCriacao { get; set; }
        public int? turmaCodigo { get; set; }
        public int? codigoInstrutor { get; set; }
        public DateTime? dataNascimento { get; set; }
        public int? cep { get; set; }
        public string? rua { get; set; }
        public string? bairro { get; set; }
        public string? cidade { get; set; }
        public string? celular { get; set; }
        public string? descricao { get; set; }
        public bool? haveNote { get; set; }

        public int? status { get; set; }
        public int? id_Posicao { get; set; }
        public bool isAluno { get; set; }
        public bool isEmail { get; set; }
        public string? urlFoto { get; set; }
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public int? cor { get; set; }
        public int? UF { get; set; }
        public bool? haveInternetHouse { get; set; }

        public int? genero { get; set; }

        public decimal? rendaFamiliar { get; set; }

        public int? qntdPessoas { get; set; }

        public int? mensalidade { get; set; }

        public string? cargo { get; set; }

        public string? sobreMim { get; set; }

        public string? atividade { get; set; }

        public string? numeroCasa { get; set; }
        
        public string? complemento { get; set; }

        public DateTime? dataEmailEnviado { get; set; }
        public DateTime? inscricaoPresencial { get; set; }
        public int? unidadeCodigo { get; set; } 

        public int? dataPagamento { get; set; }

    }
}