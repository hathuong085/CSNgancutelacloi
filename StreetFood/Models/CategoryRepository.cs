using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetFood.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDBContext context;

        public CategoryRepository(AppDBContext context)
        {
            this.context = context;
        }
        public Category Create(Category cate)
        {
            context.Categories.Add(cate);
            context.SaveChanges();
            return cate;
        }

        public bool Delete(int id)
        {
            Category category = Get(id);
            if (category != null)
            {
                var products = (from p in context.Products where p.CategoryId == id select p).ToList();
                if (products != null)
                {
                    context.Products.RemoveRange(products);
                }
                context.Categories.RemoveRange(category);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Category Edit(Category category)
        {
            var edit=context.Categories.Attach(category);
            edit.State = EntityState.Modified;
            context.SaveChanges();
            return category;
        }

        public Category Get(int id)
        {
            Category category= context.Categories.Find(id);
            return category;
        }

        public IEnumerable<Category> Gets()
        {
            return context.Categories.ToList();
        }
    }
}
