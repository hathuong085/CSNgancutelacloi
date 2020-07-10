using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StreetFood.ViewModel
{
    public class CategoryCreateViewModel
    {
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public IFormFile CategoryImg { get; set; }
    }
}
