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

        public DbProduct SelectedProduct { get; set; }

        public string FullName { get; set; } = Settings.Default.UserName == string.Empty ? "Гость" : $"{Settings.Default.UserSurname} {Settings.Default.UserName} {Settings.Default.UserPatronymic}";

        public int? MaxRecords { get; set; } = 0;

        public int? Records { get; set; } = 0;

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
        }

        private void CheckEnabled() => IsEnabledCart = Global.CurrentCart.Any(c => c.ArticleName != null);

        private async void UpdateProduct()
        {
            var currentProduct = await _productService.GetProducts();
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
            _pageService.ChangePage(new EditAdminPage());

        });

        public DelegateCommand DeleteCard => new(async () =>
        {

        });

        public DelegateCommand CardCommand => new(() =>
        {
            _pageService.ChangePage(new BasketInfoPage());
        });
    }
}
