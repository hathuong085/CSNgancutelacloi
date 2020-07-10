using Microsoft.AspNetCore.Http;
using StreetFood.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StreetFood.ViewModel
{
    public class HomeCreateViewModel
    {
        [Required]
        public IFormFile Img { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
