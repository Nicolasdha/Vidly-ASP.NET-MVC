using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vidly.Controllers
{

    public class MoviesController : Controller
    {
        private const string CONNECTION_STRING = "Server=.; Database=Vidly; User Id=sa; Password=Pittsburgh1; Trusted_Connection=False; MultipleActiveResultSets=true";

        private List<Movies> GetMovies()
        {

            var sql = @"SELECT * FROM [dbo].[Movies]";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                var people = cnn.Query<Movies>(sql);


                return (List<Movies>)people;
            }

        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            var movies = GetMovies();

            return View(movies);
        }


        public IActionResult Edit(int id)
        {
            var movie = GetMovies().SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return this.StatusCode(StatusCodes.Status418ImATeapot, "Customer Not Found");


            return View(movie);

        }



        public IActionResult New()
        {

            var movie = new Movies();


            return View(movie);
        }



        [HttpPost]
        public IActionResult Create(Movies movies)
        {

            var p = new DynamicParameters();
            p.Add("@name", movies.Name);
            p.Add("@genre", movies.Genre);
            p.Add("@release_date", movies.Release_date);
            p.Add("@Stock", movies.Stock);
            p.Add("@Date_added", movies.Date_added);


            var sql = $@"INSERT INTO [dbo].[Movies] (name, genre, stock, release_date, date_added)
                            VALUES (@name, @genre, @Stock, @release_date,  @Date_added);";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                //var people = cnn.Query<Customers, MembershipType, Customers>(sql, (customer, member) => { customer.MembershipType = member; return customer; });
                cnn.Execute(sql, p);

            }

            return Redirect("/");

            //var result = await connection.ExecuteAsync(sql, newPerson);
        }




    }
}
