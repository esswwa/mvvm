﻿using mvvm.Data.Models;
using mvvm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.ViewModels
{
    public class EditAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;

        public ObservableCollection<Product> Products { get; set; }
        public Product Product { get; set; }

        public string ProductArticleNumber { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public List<Kategory> ProductCategory { get; set; }

        public string ProductPhoto { get; set; }

        public string TextHead { get; set; }

        public List<Manufacturer> ProductManufacturer { get; set; }

        public float ProductCost { get; set; }

        public int ProductDiscountAmount { get; set; }

        public int ProductQuantityInStock { get; set; }

        public List<string> ProductStatus { get; set; }

        public string ProductStatus1 { get; set; }
        public Manufacturer ProductManufacturer1 { get; set; }
        public Kategory ProductCategory1 { get; set; }

        public EditAdminViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            Products = _productService.getAllProd();
            if (ProductModel.products != null)
            {
                TextHead = "Редактирование продукта";
                Product = _productService.getProd(ProductModel.products);
                ProductArticleNumber = Product.ProductArticleNumber;
                ProductName = Product.ProductName;
                ProductDescription = Product.ProductDescription;
                ProductPhoto = Product.ProductPhoto;
                ProductCost = Product.ProductCost;
                ProductDiscountAmount = Product.ProductDiscountAmount;
                ProductQuantityInStock = Product.ProductQuantityInStock;

                List<Manufacturer> Manufacturers = _productService.getAllManufacrurers();   
                List<Kategory> Categories = _productService.getAllCategories();


                ProductStatus = new List<string> { ("уп."), ("шт.") };
                ProductCategory = Categories;
                ProductManufacturer = Manufacturers;

                ProductStatus1 = Product.ProductStatus;
                ProductManufacturer1 = Product.ProductManufacturerNavigation;
                ProductCategory1 = Product.ProductCategoryNavigation;
            }
            else
            {
                TextHead = "Добавление нового продукта";
                List<Manufacturer> Manufacturers = _productService.getAllManufacrurers();
                List<Kategory> Categories = _productService.getAllCategories();


                
                Product = new Product();
                Product.ProductArticleNumber = "";
                Product.ProductName = "";
                Product.ProductDescription = "";
                Product.ProductPhoto = "";
                Product.ProductCost = 0;
                Product.ProductDiscountAmount = ProductDiscountAmount;
                Product.ProductQuantityInStock = ProductQuantityInStock;
                ProductStatus = new List<string> { ("уп."), ("шт.") };
                ProductCategory = Categories;
                ProductManufacturer = Manufacturers;
            }

            }
        public DelegateCommand ReturnBackCommand => new(() =>
        {
            _pageService.ChangePage(new AdminListProducts());
        });
        public AsyncCommand EditCommand => new(async () =>
        {
            Product.ProductName = ProductName;
            Product.ProductDescription = ProductDescription;
            Product.ProductCategory = ProductCategory1.Idkategory;
            Product.ProductPhoto = ProductPhoto;
            Product.ProductManufacturer = ProductManufacturer1.IdManufacturer;
            Product.ProductCost = ProductCost;
            Product.ProductDiscountAmount = ProductDiscountAmount;
            Product.ProductQuantityInStock = ProductQuantityInStock;
            Product.ProductStatus = ProductStatus1;

            await _productService.redactProduct(Product, Products);
            _pageService.ChangePage(new AdminListProducts());
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Product.ProductArticleNumber)
            || string.IsNullOrWhiteSpace(ProductName)
            || string.IsNullOrWhiteSpace(ProductDescription)
            || string.IsNullOrWhiteSpace(ProductStatus1)
            || ProductModel.status == "Добавление")
                return false;
            return true;
        });

        public AsyncCommand AddCommand => new(async () =>
        {
            Product.ProductArticleNumber = ProductArticleNumber;
            Product.ProductName = ProductName;
            Product.ProductDescription = ProductDescription;
            Product.ProductCategory = ProductCategory1.Idkategory;
            Product.ProductPhoto = ProductPhoto;
            Product.ProductManufacturer = ProductManufacturer1.IdManufacturer;
            Product.ProductCost = ProductCost;
            Product.ProductDiscountAmount = ProductDiscountAmount;
            Product.ProductQuantityInStock = ProductQuantityInStock;
            Product.ProductStatus = ProductStatus1;

            await _productService.addProduct(Product);
            _pageService.ChangePage(new AdminListProducts());
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(ProductArticleNumber)
            || string.IsNullOrWhiteSpace(ProductName)
            || string.IsNullOrWhiteSpace(ProductDescription)
            || string.IsNullOrWhiteSpace(ProductStatus1)
            || ProductModel.status == "Редактирование")
                return false;
            return true;
        });

    }
}
