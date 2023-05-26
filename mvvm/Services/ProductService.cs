using mvvm.Data;
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
                        if (item.ProductStatusActiv != "Удален")
                        {
                            products.Add(new DbProduct
                            {

                                Image = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                                Title = item.ProductName,
                                Description = item.ProductDescription,
                                Manufacturer = item.ProductManufacturerNavigation.ProductManufacture,
                                Price = item.ProductCost,
                                Discount = item.ProductDiscountAmount,
                                Article = item.ProductArticleNumber,
                                Quantity = item.ProductQuantityInStock,
                                Status = item.ProductStatusActiv
                            });
                        }    
                    }
                });
            }
            catch { }
            return products;
        }

        public async Task<List<DbProduct>> GetProductsAdmin()
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
                                Discount = item.ProductDiscountAmount,
                                Article = item.ProductArticleNumber,
                                Quantity = item.ProductQuantityInStock,
                                Status = item.ProductStatusActiv
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

        public async Task saveRedact(Orderuser SelectedOrder, ObservableCollection<Orderuser> Orders)
        {
            var item = Orders.First(i => i.OrderId == SelectedOrder.OrderId);
            var index = Orders.IndexOf(item);
            item.OrderDeliveryDate = DateOnly.FromDateTime(SelectedOrder.OrderDeliveryDate.ToDateTime(TimeOnly.FromDateTime(DateTime.Now)));
            item.OrderStatus = "Завершен";
            Orders.RemoveAt(index);
            Orders.Insert(index, item);
            await _tradeContext.SaveChangesAsync();
        }

        public async Task redactProduct(Product SelectedProduct, ObservableCollection<Product> Products)
        {
            var item = Products.First(i => i.ProductArticleNumber == SelectedProduct.ProductArticleNumber);
            var index = Products.IndexOf(item);

            item.ProductArticleNumber = SelectedProduct.ProductArticleNumber;
            item.ProductName = SelectedProduct.ProductName;
            item.ProductDescription = SelectedProduct.ProductDescription;
            item.ProductCategory = SelectedProduct.ProductCategory;
            item.ProductPhoto = SelectedProduct.ProductPhoto;
            item.ProductManufacturer = SelectedProduct.ProductManufacturer;
            item.ProductCost = SelectedProduct.ProductCost;
            item.ProductDiscountAmount = SelectedProduct.ProductDiscountAmount;
            item.ProductQuantityInStock = SelectedProduct.ProductQuantityInStock;
            item.ProductStatus = SelectedProduct.ProductStatus;

            Products.RemoveAt(index);
            Products.Insert(index, item);
            await _tradeContext.SaveChangesAsync();
        }

        public async Task deleteProduct(Product SelectedProduct, ObservableCollection<Product> Products)
        {
            var item = Products.First(i => i.ProductArticleNumber == SelectedProduct.ProductArticleNumber);
            var index = Products.IndexOf(item);
            item.ProductStatusActiv = "Удален";
            Products.RemoveAt(index);
            Products.Insert(index, item);
            await _tradeContext.SaveChangesAsync();
        }

        public async Task addProduct(Product SelectedProduct)
        {
            await _tradeContext.AddAsync(new Product
            {
                ProductArticleNumber= SelectedProduct.ProductArticleNumber,
                ProductName= SelectedProduct.ProductName,   
                ProductDescription= SelectedProduct.ProductDescription,
                ProductCategory= SelectedProduct.ProductCategory,
                ProductPhoto= SelectedProduct.ProductPhoto,
                ProductManufacturer= SelectedProduct.ProductManufacturer,
                ProductCost= SelectedProduct.ProductCost,
                ProductDiscountAmount = SelectedProduct.ProductDiscountAmount,
                ProductStatus= SelectedProduct.ProductStatus,
                ProductQuantityInStock = SelectedProduct.ProductQuantityInStock,
                ProductStatusActiv = "Активный"
            });
            await _tradeContext.SaveChangesAsync();
        }

        public Product getProd(string article) {

            return _tradeContext.Products.Where(i => i.ProductArticleNumber == article).First();
        }

        public ObservableCollection<Product> getAllProd()
        {
            return _tradeContext.Products.ToObservableCollection<Product>();
        }

        public List<int> getAllCategories()
        {
            return _tradeContext.Kategories.Select(i => i.Idkategory).ToList();
        }

        public List<int> getAllManufacrurers()
        {
            return _tradeContext.Manufacturers.Select(i => i.IdManufacturer).ToList();
        }


        public async Task<List<Kategory>> getAllCategoriesObjects()
        {
            return await _tradeContext.Kategories.ToListAsync();
        }

        public async Task<List<Manufacturer>> getAllManufacrurersObjects()
        {
            return await _tradeContext.Manufacturers.ToListAsync();
        }

    }

  

}
