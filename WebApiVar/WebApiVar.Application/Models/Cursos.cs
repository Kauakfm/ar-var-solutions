﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository.Models;

namespace WebApiVar.Application.Models
{
    public class Cursos : TabCurso
    {
        public DateTime? Atualizadoem { get; set; }        
    }
}
