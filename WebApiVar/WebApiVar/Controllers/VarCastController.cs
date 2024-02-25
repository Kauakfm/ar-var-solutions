using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiVar.Application.Services;
using WebApiVar.Repository;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VarCastController : ControllerBase
    {
        private readonly VAREntities _ctx;
        public VarCastController(VAREntities ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("ObterPodcast")]
       
        public IActionResult ObterVarCast()
        {
            var cast = new VarCastService(_ctx);
            var castt = cast.ObterVarCast();
            if( castt == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(castt);
            }
        }
    }
}
