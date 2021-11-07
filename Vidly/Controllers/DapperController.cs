using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{

    [ApiController]
    [Route("api/dapper")]

    public class DapperController : Controller 
    {

    private const string CONNECTION_STRING = "Server=.; Database=Vidly; User Id=sa; Password=Pittsburgh1; Trusted_Connection=False; MultipleActiveResultSets=true";

        [HttpGet("")]

        //Get Customers
        public async Task<IActionResult> GetCustomers()
        {
            var sql = @"SELECT *
                        FROM [dbo].[customers]";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var persons = await connection.QueryAsync<Person>(sql);
                return Ok(persons);
            }
        }

    }
}
