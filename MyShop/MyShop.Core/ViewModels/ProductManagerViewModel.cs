using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class ProductManagerViewModel
    {
        public Product Product { get; set; }
        //IEnumerable is a list that you can iterate through
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
