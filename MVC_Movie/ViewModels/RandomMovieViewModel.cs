using System.Collections.Generic;
using MVC_Movie.Models;

namespace MVC_Movie.ViewModels
{
    public class RandomMovieViewModel
    {
        public Movie Movie { get; set; }
        public List<Customer> Customers { get; set; }
    }
}