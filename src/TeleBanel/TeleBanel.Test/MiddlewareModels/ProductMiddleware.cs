using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TeleBanel.Models;
using TeleBanel.Models.Middlewares;

namespace TeleBanel.Test.MiddlewareModels
{
    public class ProductMiddleware : IProductMiddleware
    {
        private readonly Dictionary<int, Product> _products;

        public ProductMiddleware()
        {
            _products = new Dictionary<int, Product>();

            for (var i = 0; i < 10; i++)
            {
                _products.Add(
                    i,
                    new Product()
                    {
                        Id = i,
                        Title = "عنوان عکس " + i,
                        Descriptin = $"این محصول تستی به شماره عنوان {i} می باشد",
                        Image = ((Bitmap)Properties.Resources.ResourceManager.GetObject("_" + i)).ToByte()
                    });
            }
        }

        public Product GetProduct(int id)
        {
            if (_products.ContainsKey(id))
            {
                return _products[id];
            }

            return null;
        }

        public Product[] GetProducts()
        {
            return _products.Values.OrderBy(p => p.Id).ToArray();
        }

        public int[] GetProductsId()
        {
            return _products.Keys.OrderBy(i => i).ToArray();
        }

        public void SetProduct(Product p)
        {
            _products[p.Id] = p;
        }

        public void DeleteProduct(int pId)
        {
            if (_products.ContainsKey(pId))
                _products.Remove(pId);
        }
    }
}
