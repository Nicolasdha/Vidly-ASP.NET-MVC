using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Movies
    {

        public string Name { get; set; }
        public int Id { get; set; }
        public string Genre { get; set; }

        [Display(Name = "Release Date")]
        public DateTime Release_date { get; set; }

        [Display(Name = "Date added to inventory")]
        public DateTime Date_added { get; set; }

        [Display(Name = "Currently in stock")]
        public int Stock { get; set; }

  
    }
}
