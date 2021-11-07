using System;
namespace Vidly.Models
{
    public class Movies
    {

        public string Name { get; set; }
        public int Id { get; set; }
        public string Genre { get; set; }
        public DateTime Release_date { get; set; }
        public DateTime Date_added { get; set; }
        public int Stock { get; set; }


    }
}
