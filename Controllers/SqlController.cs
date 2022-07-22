using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQL_StoreProcedure.Controllers
{
    [Route("api/[controller]")]
    public class SqlController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SqlController(ApplicationDbContext context, IConfiguration configuracion)
        {
            _context = context;
            _configuration = configuracion;
        }


        [HttpGet("Test")]
        public async Task<ActionResult<TestVM>> getTest()
        {
            var testOk = await _context.Test.FirstOrDefaultAsync(x => x.IdTest == 1);

            if(testOk == null)
            {
                return BadRequest("Ocurrio un error inesperado");
            }

            return Ok(testOk.Descripcion);
        }

        [HttpGet("Stored")]
        public async Task<ActionResult<TestVM>> getStoredProcedure()
        {
            var sqlClient = new SQLHelpers(_configuration);

            sqlClient.EjecutarProcAlmacenado("GetException");

            return Ok();
        }
    }
}
