/*DATA ACCESS LAYER SHOULD NOT BE IN THE WEB APP - ANOTHER ASSEMBLY WITH ALL BUSINESS LOGIC IN IT AND INTERFACES AND ALL THE IMPLEMENTATION
 IN ANOTHER ASSEMBLY - And then set up dependancy injection so youre injecting a service interface and pulling that in
 */


using System;
namespace Vidly.Models
{
    public class Person
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

    }
}
