using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class GenericResponse<T>
    {
        public string mensagem { get; set; }
        public T response { get; set; }
        public GenericResponse(string mensagem, T response)
        {
            this.mensagem = mensagem;
            this.response = response;
        }        
    }
}
