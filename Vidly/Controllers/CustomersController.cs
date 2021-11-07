using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private DapperController _dapperController;

        private List<Customers> GetCustomers()
        {



            var customer = new List<Customers>{
                new Customers { Name = "Nicolas Durik-Ha", Id = 33},
                new Customers { Name = "Loopah Scoopah", Id= 22 },
                new Customers { Name = "Loufah Poofah", Id= 11 }
                };

            return customer;
        }



        // GET: /<controller>/
        public IActionResult Index(DapperController dapperController)
        {
            _dapperController = dapperController;


            var result = _dapperController.GetCustomers();


            //var customers = GetCustomers();
           
            return View(result);
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
