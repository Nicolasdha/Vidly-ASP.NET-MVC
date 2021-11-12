using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;
using Vidly.Models.ViewModels;

namespace Vidly.Controllers
{
    public class RentalsController : Controller
    {
        private const string CONNECTION_STRING = "Server=.; Database=Vidly; User Id=sa; Password=Pittsburgh1; Trusted_Connection=False; MultipleActiveResultSets=true";


        private List<Customers> GetCustomers()
        {

            var sql = @"SELECT 
                            c.*, mt.*
                        FROM DBO.Customer c
                        JOIN DBO.MembershipType mt
                            ON C.MembershipTypeId = MT.ID;";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                var people = cnn.Query<Customers>(sql);


                return (List<Customers>)people;
            }
        }




        private List<Movies> GetMovies()
        {

            var sql = @"SELECT * FROM [dbo].[Movies]";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                var movies = cnn.Query<Movies>(sql);


                return (List<Movies>)movies;
            }

        }





        public IActionResult Index()
        {

       
            return View();
        }





        public IActionResult New(Rentals rentals)
        {

            var customer = GetCustomers().SingleOrDefault(c => c.Id == rentals.Id);
            var movies = GetMovies();

            //Or a foreach for movies to create a new rentalViewModel per movie to add to the db once submitted?
            // Or a getRentals method 



            var viewModel = new RentalViewModel
            {
                Customers = customer,
                Movies = movies,
                Rentals = new Rentals()
            };

            return View(viewModel);
        }

    }
}
