// For DATA ACCESS LAYER
// Can store the SQL in string constants or use stored procedures for complex lookup or manipulation to not write that code in the application itself 

// use connection.Query.... when querying data and connection.Execute... when executing non queries


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vidly.Controllers
{

    [ApiController]
    [Route("api/dapper")]

    public class DapperControllerPractice : Controller
    {
        private const string CONNECTION_STRING = "Server=.; Database=AdventureWorks2017; User Id=sa; Password=Pittsburgh1; Trusted_Connection=False; MultipleActiveResultSets=true";


        [HttpGet("")]


        // ----------------------- READING AND RETRIVING DATA! -----------------------------

        //Non-Dynamic Parameters
        
        public async Task<IActionResult> Index()
        {
            var sql = @"SELECT 
                            Title
                            ,FirstName
                            ,MiddleName
                            ,LastName
                        FROM PERSON.Person
                        WHERE FirstName = @FirstName";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var persons = await connection.QueryAsync<Person>(sql, new { firstName = "Kevin"}); // Dapper will look for a variable named Kevin firstName in the obj supplied as the second argumnet
                return Ok(persons);
            }
        }
     

        //Dynamic Parameters!

        public async Task<IActionResult> Index([FromQuery] bool getKevins)
        {
            var sql = @"SELECT TOP 10
                        Title
                        ,FirstName
                        ,MiddleName
                        ,LastName
                    FROM PERSON.Person";

            var dynamicParameters = new DynamicParameters(); // Used if have a search feature with optional fields to dynamically build a SQL query - add a WHERE clause for a firstName 



            if (getKevins)
            {
                sql += " WHERE FirstName = @FirstName";
                dynamicParameters.Add("firstName", "Kevin");
                // Here if you type ?getKevins=true in the url it will run the sql query above and only return kevins
            }

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var resultSet = await connection.QueryAsync<Person>(sql, dynamicParameters);
                return Ok(resultSet);
            }

        }

        // Will grab and return first thing - to check if there is one thing that matches in the DB - better than querySingle b/c that still iterates over whole DB
        private async Task DemoWithQueryFirst(string CONNECTION_STRING)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            var sql = @"SELECT TOP 10
                        Title
                        ,FirstName
                        ,MiddleName
                        ,LastName
                    FROM PERSON.Person";

            var result = await connection.QueryFirstAsync<Person>(sql);
            Console.WriteLine(result);

        }


        // ----------- INSERTING / UPDATING / DELETING DATA--------------------------

        // INSERT - have an object in memory or get one from a part of the application and need to insert that data into the SQL table 

        private static async Task DemoExecuteAsyncWithInsert(string CONNECTION_STRING)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            var sql = @"INSERT INTO PERSON.PERSON
                        (FirstName
                        ,LastName
                        ,PhoneNumber
                        ,UserName
                        ,DateOfBirth
                        ,City
                        ,State
                        ,ZipCode)
                    VALUES
                        (@firstName, @lastName, @phoneNumber,
                        @userName, @dateOfBirth, @city, @state, @zipcode)";

            var newPerson = new Person()
            {
                FirstName = "Jim",
                LastName = "Jammington",
                City = "Chesapeake",
                State = "Virginia",
                Zipcode = "23320",
                DateOfBirth = DateTime.Parse("1991, 02, 02"),
                PhoneNumber = "301-672-7987",
                UserName = "sankp001"

            };

            var result = await connection.ExecuteAsync(sql, newPerson);

            Console.WriteLine(result);

        }


        //UPDATE

        private static async Task DemoExecuteAsyncWithUpdate(string CONNECTION_STRING)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            var sql = @"UPDATE PERSON.PERSON
                        SET FirstName = @firstName
                        WHERE BusinessEntityID = @businessEntityID";

            var result = await connection.ExecuteAsync(sql, param: new { firstName = "Nicolas", businessEntityId = 1 });


            // Result will be a number depending on how many rows are updated
        }



        // DELETE

        private static async Task DemoExecuteAsyncWithDelete(string CONNECTION_STRING)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            var sql = @"DELETE FROM PERSON.PERSON
                       WHERE LastName = @lastName";

            var result = await connection.ExecuteAsync(sql, param: new { lastName = "Jones" });
            // Will return a result if delete worked 
        }




        //STORED PROCEDURE - How to call a stored procedure. If the stored procedure is just doing updates, inserts, etc (any non query) - dont need to call QueryAsync b/c no need to
        // return anything - can just call ExecuteAsync()

        // If have a stored procedure that does updates, inserts AND returns you can set this up with Dapper too - Also if you have output parameters can do this with Dapper too 

        private static async Task DemoStoredProcedure(string CONNECTION_STRING)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            var sql = "dbo.LastNameList"; // This is the stored prcedure 

            var results = await connection.QueryAsync<string>(sql, commandType: CommandType.StoredProcedure);

        }



        // Transactions - Need to open connection manually when using transactions - if an exception occurs it will trigger a rollback b/c never getting to the commit

        public async Task<IActionResult> Transaction()
        {
            var sql = @"INSERT INTO [dbo].[TESTTABLE]
                                 ([Foobar])
                              VALUES
                                    (@foobar)";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    for (var x = 0; x < 1000; x++)
                    {
                        //if (x == 500)
                        //{
                        //    throw new Exception("ouch");
                        //}

                        await connection.ExecuteAsync(sql, new { foobar = $"testing {DateTime.UtcNow.Ticks}" }, transaction: transaction);

                    }
                    transaction.Commit();

                }
            }
                    return Ok();

        }


    }
}