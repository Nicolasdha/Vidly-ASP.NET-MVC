using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vidly.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {

        private const string CONNECTION_STRING = "Server=.; Database=Vidly; User Id=sa; Password=Pittsburgh1; Trusted_Connection=False; MultipleActiveResultSets=true";


        private List<Customers> GetCustomers()
        {

            var sql = @"SELECT * 
                        FROM DBO.Customer C";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                var people = cnn.Query<Customers>(sql);

                return (List<Customers>)people;
            }
        }

        public IActionResult Index()
        {

            var customers = GetCustomers();

            return View(customers);
        }


        public IActionResult Details(int id)
        {

            var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return this.StatusCode(StatusCodes.Status418ImATeapot, "Customer Not Found");


            return View(customer);


        }
    }
}
