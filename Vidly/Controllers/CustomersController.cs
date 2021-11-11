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
using Vidly.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vidly.Controllers
{
    public class CustomersController : Controller
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

                var people = cnn.Query<Customers, MembershipType, Customers>(sql, (customer, member) => { customer.MembershipType = member; return customer; });


                return (List<Customers>)people;
            }
        }




        private List<MembershipType> GetMembershiptype()
        {

            var sql = @"SELECT * from DBO.MembershipType";

            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {
                var memberships = cnn.Query<MembershipType>(sql);


                return (List<MembershipType>)memberships;
            }

        }



        public IActionResult Index()
        {

            var customers = GetCustomers();

            return View(customers);
        }



        public IActionResult Edit(int id)
        {

            var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
            var memberships = GetMembershiptype();

            if (customer == null)
                return this.StatusCode(StatusCodes.Status418ImATeapot, "Customer Not Found");

            var viewModel = new NewCustomerViewModel
            {
                Customers = customer,
                MembershipType = memberships
            };


            return View(viewModel);

        }


        public IActionResult New()
        {
            var memberships = GetMembershiptype();

            var viewModel = new NewCustomerViewModel
            {
                Customers = new Customers(),
                MembershipType = memberships
            };


            return View(viewModel);
        }


        /*
            Model binding by passing in NewCustomerViewModel as an argument. B/c model behind the view is of type NewCustomerViewModel and by passing in as an argumnet the
            MVC will automatically map request data to that object = MODEL BINDING -
            MVC binds that model to the request data

            When the POST request goes to the application MVC will use the Header Properties set by the form (name, bday, membershiptype, etc) to initialize
            the parameter of our action

            If change the type of Create(Customer viewModel) - MVC will bind the Customer object to the form data b/c all of the keys in the form are prefixed with Model.Customer...
         */

        [HttpPost]
        public IActionResult Create(NewCustomerViewModel viewModel)
        {

            var p = new DynamicParameters();
            p.Add("@name", viewModel.Customers.Name);
            p.Add("@birthday", viewModel.Customers.Birthdate);
            p.Add("@memberId", viewModel.Customers.MembershipTypeId);
            p.Add("@subscribed", viewModel.Customers.IsSubscribedToNewsletter);

            var sql = $@"INSERT INTO [dbo].[Customer] (name, birthdate, IsSubscribedToNewsLetter, MembershipTypeId)
                            VALUES (@name, @birthday, @subscribed, @memberId);";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                //var people = cnn.Query<Customers, MembershipType, Customers>(sql, (customer, member) => { customer.MembershipType = member; return customer; });
                cnn.Execute(sql, p);

            }

            return Redirect("/Customers");

            //var result = await connection.ExecuteAsync(sql, newPerson);
        }



        public IActionResult Save(NewCustomerViewModel viewModel)
        {

            var memberships = GetMembershiptype();

            //if (!ModelState.IsValid)
            //{
            //    var viewModel = new NewCustomerViewModel
            //    {
            //        Customers = customer,
            //        MembershipType = memberships
            //    };

            //    return View("New", viewModel);
            //};


            var currentcustomer = GetCustomers().SingleOrDefault(c => c.Id == viewModel.Customers.Id);

            var p = new DynamicParameters();
            p.Add("@name", viewModel.Customers.Name);
            p.Add("@birthday", viewModel.Customers.Birthdate);
            p.Add("@memberId", viewModel.Customers.MembershipTypeId);
            p.Add("@subscribed", viewModel.Customers.IsSubscribedToNewsletter);
            p.Add("@Id", viewModel.Customers.Id);


            var sql = $@"UPDATE [dbo].[Customer]
                        SET
                            Name = @name,
                            Birthdate = @birthday,
                            IsSubscribedToNewsletter = @subscribed,
                            MembershipTypeId = @memberId
                        WHERE id = @Id";



            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {

                //var people = cnn.Query<Customers, MembershipType, Customers>(sql, (customer, member) => { customer.MembershipType = member; return customer; });
                cnn.Execute(sql, p);

            }

            return Redirect("/Customers");

        }





        public IActionResult Delete( int Id)
        {
            var p = new DynamicParameters();

            p.Add("@Id", Id);

            var sql = @"DELETE  FROM Customer WHERE ID = @Id";


            using (IDbConnection cnn = new SqlConnection(CONNECTION_STRING))
            {
                cnn.Execute(sql, p);
            }

            return Redirect("/Customers");
        }

    }
}
