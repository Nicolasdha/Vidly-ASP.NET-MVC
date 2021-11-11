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
using Vidly.Models.ViewModels;

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

                var movies = cnn.Query<Movies>(sql);


                return (List<Movies>)movies;
            }

        }



        private List<Genres> GetGenres()
        {

            var sql = @"SELECT * from DBO.Genres";

            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {
                var genres = cnn.Query<Genres>(sql);


                return (List<Genres>)genres;
            }

        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            var movies = GetMovies();
            var genres = GetGenres();

            //var moviesList = new List<Movies> { movies };

            var viewModel = new MovieViewModel
            {
                Movies = movies,
                Genres = genres
            };

            return View(viewModel);
        }


        public IActionResult Edit(int id)
        {
            var movie = GetMovies().SingleOrDefault(m => m.Id == id);
            var genres = GetGenres();

            if (movie == null)
                return this.StatusCode(StatusCodes.Status418ImATeapot, "Customer Not Found");

            var viewModel = new MoviesNoListViewModel
            {
                Movies = movie,
                Genres = genres
            };


            return View(viewModel);

        }



        public IActionResult New()
        {


            var genres = GetGenres();

            var viewModel = new MoviesNoListViewModel
            {
                Movies = new Movies(),
                Genres = genres
            };


            return View(viewModel);
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

            return Redirect("/Movies");

            //var result = await connection.ExecuteAsync(sql, newPerson);
        }




        public IActionResult Save(Movies movies, int id)
        {
            var currentcustomer = GetMovies().SingleOrDefault(m => m.Id == id);

            var p = new DynamicParameters();
            p.Add("@name", movies.Name);
            p.Add("@genre", movies.Genre);
            p.Add("@release_date", movies.Release_date);
            p.Add("@Stock", movies.Stock);
            p.Add("@Date_added", movies.Date_added);
            p.Add("@Id", movies.Id);


            var sql = $@"UPDATE [dbo].[Movies]
                        SET
                            Name = @name,
                            Genre = @genre,
                            Stock = @stock,
                            Release_Date = @release_date,
                            Date_added = @Date_added
                        WHERE id = @Id";



            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                //var people = cnn.Query<Customers, MembershipType, Customers>(sql, (customer, member) => { customer.MembershipType = member; return customer; });
                cnn.Execute(sql, p);

            }

            return Redirect("/Movies");

        }





        public IActionResult Delete(int Id)
        {
            var p = new DynamicParameters();

            p.Add("@Id", Id);

            var sql = @"DELETE  FROM Movies WHERE ID = @Id";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {
                cnn.Execute(sql, p);
            }

            //return Redirect("/Movies");
            return Redirect("/Movies");
        }



    }
}
