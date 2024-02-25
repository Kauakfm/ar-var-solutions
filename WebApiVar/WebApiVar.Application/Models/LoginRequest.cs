using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }

        public string presencial { get; set; }
        
    }
}
