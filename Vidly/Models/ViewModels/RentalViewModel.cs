using System;
using System.Collections.Generic;

namespace Vidly.Models.ViewModels
{
    public class RentalViewModel
    {

        public Customers Customers { get; set; }

        public List<Movies> Movies { get; set; }

        public Rentals Rentals { get; set; }
    }
}


