﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Repository.Models
{
    public class TabPerguntaPesquisa
    {
        [Key]

        public int codigo { get; set; }

        public int pesquisaCodigo { get; set; }

        public string pergunta { get; set; }
    }
}
