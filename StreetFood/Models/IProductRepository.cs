using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetFood.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Gets();
        Product Get(int id);
        Product Create(Product pd);
        bool Delete(int id);
        Product Edit(Product pd);

    }
}
