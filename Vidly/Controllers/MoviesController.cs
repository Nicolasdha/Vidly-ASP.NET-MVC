using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vidly.Controllers
{

    public class MoviesController : Controller
    {

        private List<Movies> GetMovies()
        {

            var moviesList = new List<Movies>
            {
                new Movies {Name = "The Last Of The Mohicans"},
                new Movies {Name = "The Warriors"}
            };

            return moviesList;
        }



        // GET: /<controller>/
        public IActionResult Index()
        {
            var movies = GetMovies();

            return View(movies);
        }


     



    }
}
