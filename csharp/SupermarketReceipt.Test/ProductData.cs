using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketReceipt.Test
{
    public static class ProductData
    {
        public static Product ProductTootbrush => new("toothbrush", ProductUnit.Each); 
        public static Product ProductApple => new("apple", ProductUnit.Kilo); 
        public static Product ProductRice => new("rice", ProductUnit.Each); 
        public static Product ProductToothpaste => new("toothpaste", ProductUnit.Each); 
        public static Product ProductTomatoe => new("tomatoe", ProductUnit.Each); 
    }
}
