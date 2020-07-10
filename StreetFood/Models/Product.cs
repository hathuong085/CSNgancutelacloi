using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StreetFood.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Img { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string KeySearch { get; set; }
        public string Title { get; set; }
    }
}
