﻿using mvvm.Data;
using mvvm.Data.Models;

namespace mvvm.Services
{
    public class ProductService
    {
        private readonly TradeContext _tradeContext;
        public ProductService(TradeContext tradeContext)
        {
            _tradeContext = tradeContext;
        }

        public async Task<List<DbProduct>> GetProducts()
        {
            List<DbProduct> products = new();
            try
            {
                var product = await _tradeContext.Products.ToListAsync();
                await _tradeContext.Manufacturers.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var item in product)
                    {
                        products.Add(new DbProduct
                        {

                            Image = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                            Title = item.ProductName,
                            Description = item.ProductDescription,
                            Manufacturer = item.ProductManufacturerNavigation.ProductManufacture,
                            Price = item.ProductCost,
                            Discount = item.ProductDiscountAmount.Value,
                            Article = item.ProductArticleNumber,
                            Quantity = item.ProductQuantityInStock
                        });
                    }
                });
            }
            catch { }
            return products;
        }
        public async Task<List<DbProduct>> getBasket() {

            List<DbProduct> a = new();
            var b = await GetProducts();

            foreach (var item in Global.CurrentCart)
            {
                var product = b.SingleOrDefault(c => c.Article.Equals(item.ArticleName));
                if (product != null)
                {
                    product.Count = Global.CurrentCart.Single(a => a.ArticleName.Equals(product.Article)).Count;
                    a.Add(product);
                }
            }
            return a;
        }
        public async Task<List<Point>> GetPoints() => await _tradeContext.Pickuppoints.AsNoTracking().ToListAsync();

        public async Task SaveChangesAsync() => await _tradeContext.SaveChangesAsync();

        public async Task<int> AddOrder(Orderuser order)
        {
            await _tradeContext.Orderusers.AddAsync(order);
            await _tradeContext.SaveChangesAsync();

            foreach (var item in Global.CurrentCart)
            {
                await _tradeContext.Orderproducts.AddAsync(new Orderproduct
                {
                    OrderId = order.OrderId,
                    ProductArticleNumber = item.ArticleName,
                    ProductCount = item.Count
                });
                await _tradeContext.SaveChangesAsync();
            }

            return order.OrderId;
        }

        public async Task<List<DbProduct>> GetListFullInformation()
        {
            var currentProduct = await GetProducts();
            List<DbProduct> product = new();
            foreach (var item in currentProduct)
            {
                if (Global.CurrentCart.FirstOrDefault(c => c.ArticleName.Equals(item.Article)) != null)
                    product.Add(item);
            }
            return product;
        }

        public async Task<List<Orderuser>> GetOrders()
        {
            _tradeContext.Orderusers.ToList();
            _tradeContext.Products.ToList();
            return _tradeContext.Orderusers.ToList();
        }


    }

  

}