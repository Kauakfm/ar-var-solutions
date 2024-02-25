using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class VerDetalhes
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string UltimoAcesso { get; set; }
        public string DataCriacao { get; set; }
        public string Turma { get; set; }
        public string DataNascimento { get; set; }
        public int? Cep { get; set; }
        public string? Rua { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Celular { get; set; }
        public string? Descricao { get; set; }
        public string? HaveNote { get; set; }

        public string? Status { get; set; }
        public int? Id_Posicao { get; set; }
        public bool IsAluno { get; set; }
        public bool IsEmail { get; set; }
        public string? UrlFoto { get; set; }
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public string? Cor { get; set; }
        public string? UF { get; set; }
        public string? HaveInternetHouse { get; set; }

        public string? Genero { get; set; }

        public decimal? RendaFamiliar { get; set; }

        public int? QntdPessoas { get; set; }

        public string? Mensalidade { get; set; }

        public string? Cargo { get; set; }

        public string? SobreMim { get; set; }

        public string? Atividade { get; set; }

        public string? NumeroCasa { get; set; }

        public string? Complemento { get; set; }

        public string? DataEmailEnviado { get; set; }
        public string? InscricaoPresencial { get; set; }
        public string? Unidade { get; set; }

        public string? DataPagamento { get; set; }

    }
}
