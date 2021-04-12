using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public interface IProductService : IService<Product>
    {
        public List<Product> GetLatest(int max=10);
        public List<Product> GetUserProducts(int id);
        public List<Product> GetProductsByKeys(string[] keys);
        public List<Product> GetProductsByInterest(int id);
        public List<ProductWithPhoto> GetProductWithPhotos(); 
        public List<ProductWithPhoto> GetProductForUserWithPhotos(int id);
    }
}
