using mvvm.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.ViewModels
{
    public class AdminListProductsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;

        public List<string> Sorts { get; set; } = new() {
            "По производителю",
            "По возрастанию (Цена)",
            "По убыванию (Цена)",
            "По возрастанию (Скидка)",
            "По убыванию (Скидка)"
        };



        public List<string> Filters { get; set; } = new() {
            "Все диапазоны",
            "Без скидки",
            "1-5%",
            "5-9%",
            "9% и более"
        };



        public bool IsEnabledCart { get; set; }

        public List<DbProduct> Products { get; set; }

        public ObservableCollection<Product> Products1 { get; set; }

        public List<Kategory> Categories { get; set; }

        public List<Manufacturer> Manufacturers { get; set; }

        public ObservableCollection<Kategory> Categories1 { get; set; }

        public ObservableCollection<Manufacturer> Manufacturers1 { get; set; }

        public DbProduct SelectedProduct { get; set; }

        public Product selectProduct { get; set; }

        public string FullName { get; set; } = Settings.Default.UserName == string.Empty ? "Гость" : $"{Settings.Default.UserSurname} {Settings.Default.UserName} {Settings.Default.UserPatronymic}";

        public int? MaxRecords { get; set; } = 0;

        public int? Records { get; set; } = 0;

        public string HeaderTextManufacture { get; set; }

        public Visibility VisibilityManufacture { get; set; }

        public Visibility VisibilityCategorie { get; set; }

        public DelegateCommand<string> AddManufacturer { get; set; }
        public DelegateCommand<string> AddCategories { get; set; }

        public int idManufacture { get; set; }

        public string Manufacture { get; set; }

        public int idCategorie { get; set; }

        public string Categorie { get; set; }

        public string buttonManufacture { get; set; }

        public Manufacturer SelectedManufacture { get; set; }

        public Kategory SelectedCategorie { get; set; }
         
        public string buttonCategories { get; set; }

        public string HeaderTextCategorie { get; set; }
        public string SelectedSort
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        public string SelectedFilter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        public string Search
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        public AdminListProductsViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            CheckEnabled();
            SelectedFilter = "Все диапазоны";
            ProductModel.products = null;
            ProductModel.status = null;
            AddManufacturer = new DelegateCommand<string>(ManufactureMethod);
            AddCategories = new DelegateCommand<string>(CategorieMethod);
            VisibilityManufacture = Visibility.Hidden;
            VisibilityCategorie = Visibility.Hidden;
        }


        private void ManufactureMethod(string parametr)
        {
            if (parametr.Contains("Добавить"))
            {
                VisibilityManufacture = Visibility.Visible;
                HeaderTextManufacture = "Добавление производителя";
                buttonManufacture = "Добавить";
                idManufacture = 0;
                Manufacture = "";
            }
            else if (parametr.Contains("Редактировать"))
            {
                VisibilityManufacture = Visibility.Visible;
                HeaderTextManufacture = "Редактирование производителя";
                buttonManufacture = "Редактировать";
                idManufacture = SelectedManufacture.IdManufacturer;
                Manufacture = SelectedManufacture.ProductManufacture;
            }
        }

        private void CategorieMethod(string parametr)
        {
            if (parametr.Contains("Добавить"))
            {
                VisibilityCategorie = Visibility.Visible;
                HeaderTextCategorie = "Добавление категории";
                buttonCategories = "Добавить";
                idCategorie = 0;
                Categorie = "";
            }
            else if (parametr.Contains("Редактировать"))
            {
                VisibilityCategorie = Visibility.Visible;
                HeaderTextCategorie = "Редактирование категории";
                buttonCategories = "Редактировать";
                idCategorie = SelectedCategorie.Idkategory;
                Categorie = SelectedCategorie.ProductKategory;
            }
        }

        public DelegateCommand AddManufactureTrue => new(async () =>
        {
            Manufacturers1 = Manufacturers.ToObservableCollection();
            if (buttonManufacture == "Добавить")
            {
                await _productService.addManufature(Manufacture);
                VisibilityManufacture = Visibility.Hidden;
            }
            else if (buttonManufacture == "Редактировать") {
                await _productService.editManufature(idManufacture, Manufacture, Manufacturers1);
                VisibilityManufacture = Visibility.Hidden;
            }
        });

        public DelegateCommand AddCategorieTrue => new(async () =>
        {
            Categories1 = Categories.ToObservableCollection();
            if (buttonCategories == "Добавить")
            {
                await _productService.addCategories(Categorie);
                VisibilityManufacture = Visibility.Hidden;
            }
            else if (buttonCategories == "Редактировать")
            {
                await _productService.editCategorie(idCategorie, Categorie, Categories1);
                VisibilityManufacture = Visibility.Hidden;
            }
        });



        private void CheckEnabled() => IsEnabledCart = Global.CurrentCart.Any(c => c.ArticleName != null);

        private async void UpdateProduct()
        {
            var currentProduct = await _productService.GetProductsAdmin();
            var currentManufacturer = await _productService.getAllManufacrurersObjects();
            var currentCategorie = await _productService.getAllCategoriesObjects();
            Manufacturers = currentManufacturer;
            Categories = currentCategorie;
            MaxRecords = currentProduct.Count;

            if (!string.IsNullOrEmpty(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "Без скидки":
                        currentProduct = currentProduct.Where(p => p.Discount == 0).ToList();
                        break;
                    case "1-5%":
                        currentProduct = currentProduct.Where(p => p.Discount > 0 && p.Discount < 5).ToList();
                        break;
                    case "5-9%":
                        currentProduct = currentProduct.Where(p => p.Discount >= 5 && p.Discount < 9).ToList();
                        break;
                    case "9% и более":
                        currentProduct = currentProduct.Where(p => p.Discount >= 9).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(Search))
                currentProduct = currentProduct.Where(p => p.Title.ToLower().Contains(Search.ToLower())).ToList();

            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По производителю":
                        currentProduct = currentProduct.OrderBy(p => p.Manufacturer).ToList();
                        break;
                    case "По возрастанию (Цена)":
                        currentProduct = currentProduct.OrderBy(p => p.Price).ToList();
                        break;
                    case "По убыванию (Цена)":
                        currentProduct = currentProduct.OrderByDescending(p => p.Price).ToList();
                        break;
                    case "По возрастанию (Скидка)":
                        currentProduct = currentProduct.OrderBy(p => p.Discount).ToList();
                        break;
                    case "По убыванию (Скидка)":
                        currentProduct = currentProduct.OrderByDescending(p => p.Discount).ToList();
                        break;
                }
            }

            Records = currentProduct.Count;
            Products = currentProduct;
        }

        public DelegateCommand SignOutCommand => new(() =>
        {
            Settings.Default.UserId = 0;
            Settings.Default.UserName = string.Empty;
            Settings.Default.UserSurname = string.Empty;
            Settings.Default.UserPatronymic = string.Empty;
            Settings.Default.RoleName = string.Empty;
            _pageService.ChangePage(new SingInPage());
        });
        public DelegateCommand RedactCard => new(async () =>
        {
            ProductModel.products = SelectedProduct.Article;
            ProductModel.status = "Редактирование";
            _pageService.ChangePage(new EditAdminPage());
        });

        public DelegateCommand DeleteCard => new(async () =>
        {
            selectProduct = _productService.getProd(SelectedProduct.Article);
            Products1 = _productService.getAllProd();
            await _productService.deleteProduct(selectProduct, Products1);
            _pageService.ChangePage(new BrowseAdminPage());
        });
        public DelegateCommand AddCard => new(() =>
        {
            ProductModel.products = null;
            ProductModel.status = "Добавление";
            _pageService.ChangePage(new EditAdminPage());
        });

        public DelegateCommand CardCommand => new(() =>
        {
            _pageService.ChangePage(new BrowseAdminPage());
        });

    }
}
