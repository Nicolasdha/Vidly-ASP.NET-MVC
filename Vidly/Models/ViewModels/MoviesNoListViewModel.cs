using System;
using System.Collections.Generic;

namespace Vidly.Models.ViewModels
{
    public class MoviesNoListViewModel
    {

        public Movies Movies { get; set; }

        public List<Genres> Genres { get; set; }
    }
}
