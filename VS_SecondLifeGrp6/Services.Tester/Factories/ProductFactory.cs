using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class ProductFactory
    {
        public static Product GenericProduct1 = new Product();
        public static Product GenericProduct2 = new Product();
        public static Product GenericProduct3 = new Product();

        // Generating errors --------------------
        public static Product BlankNameProduct = new Product();
        public static Product BlankDescriptionProduct = new Product();
        public static Product NegativePriceProduct = new Product();
        public static Product UnknownOwnerProduct = new Product();
        public static Product UnknownProduct = new Product();

        public static void InitFactory()
        {
            UserFactory.InitFactory();

            var list = All();
            var props = typeof(ProductFactory).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            for (int i=0; i<list.Count; i++)
            {
                var p = list[i];
                var propName = props[i].Name;
                p.Id = i;
                p.Name = propName;
                p.Price = i+1;
                p.Owner = UserFactory.GenericUser1;
                p.Description = "Fake Description";
                p.CreationDate = DateTime.Now.AddDays(i);
            }
            BlankDescriptionProduct.Description = BlankNameProduct.Name = String.Empty;
            NegativePriceProduct.Price = -1;
            UnknownOwnerProduct.Owner = UserFactory.UnknownUser;
            UnknownProduct.Id = -1;
        }

        public static List<Product> GenericList()
        {
            return new List<Product> { 
                GenericProduct1, 
                GenericProduct2, 
                GenericProduct3
            };
        }

        public static List<Product> ErrorList()
        {
            return new List<Product> { 
                BlankDescriptionProduct,
                BlankNameProduct,
                NegativePriceProduct, 
                UnknownOwnerProduct
            };
        }

        public static List<Product> All()
        {
            return GenericList().Concat(ErrorList()).ToList();
        }
    }
}
