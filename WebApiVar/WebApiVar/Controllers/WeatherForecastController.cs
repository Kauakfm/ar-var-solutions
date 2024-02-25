using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiVar.Application.Services;
using WebApiVar.Repository;

namespace WebApiVar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly VAREntities _ctx;
        public WeatherForecastController (VAREntities ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        [Route("{blabla}")]
        public IActionResult Get()
        {
            var id = 577.ToString();
            var curso = 4.ToString();
            var proc = "Exec PROC_EVOLUCAO_ALUNO @curso , @id";
            return Ok(_ctx.Proc_EvolucaoAluno.FromSqlRaw(proc.Replace("@curso", curso).Replace("@id", id)).ToList().First());
        }
        

    }
}