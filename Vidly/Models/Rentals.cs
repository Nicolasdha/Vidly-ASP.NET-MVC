using System;
namespace Vidly.Models
{
    public class Rentals
    {
        public byte Id { get; set; }
        public Customers Customers { get; set; }
        public Movies Movies { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }

    }
}


