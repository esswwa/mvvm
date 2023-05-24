namespace Pishi_Wash__Store.ViewModels
{
    public class BrowseAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;

        public List<string> Sorts { get; set; } = new() {
            "По возрастанию",
            "По убыванию"
        };



        public List<string> Filters { get; set; } = new() {
            "Все диапазоны", 
            "Новый",
            "Завершен"
        };

        public List<string> OrderFilters { get; set; } = new() {
            "Новый", 
            "Завершен" 
        };

        public ObservableCollection<Orderuser> Orders { get; set; }

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

        public BrowseAdminViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            SelectedFilter = "Все диапазоны";
        }


        private async void UpdateProduct()
        {
            var currentOrder = await _productService.GetOrders();
            MaxRecords = currentOrder.Count;

            
            if (!string.IsNullOrEmpty(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "Новый":
                        currentOrder = currentOrder.Where(p => p.OrderStatus == "Новый").ToList();
                        break;
                    case "Завершен":
                        currentOrder = currentOrder.Where(p => p.OrderStatus == "Завершен").ToList();
                        break;
                }
            }
            //if (!string.IsNullOrEmpty(Search))
            //    currentProduct = currentProduct.Where(p => p.Title.ToLower().Contains(Search.ToLower())).ToList();



            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию":
                        currentOrder = currentOrder.OrderBy(p => p.OrderId).ToList();
                        break;
                    case "По убыванию":
                        currentOrder = currentOrder.OrderByDescending(p => p.OrderId).ToList();
                        break;
                }
            }


            Records = currentOrder.Count;
            Orders = new ObservableCollection<Orderuser>(currentOrder);
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


        public DelegateCommand HelpCommand => new(() =>
        {
           
        });

        #region Caterories

        public Orderuser SelectedOrder { get; set; }


        // Редактирование
        public bool IsDialogEditOrderOpen { get; set; } = false;
        public DateTime EditDataOrder { get; set; }
        public int EditStatusOrderIndex { get; set; }

        public DelegateCommand EditOrderCommand => new(() =>
        {
            if (SelectedOrder == null)
                return;
            _productService.saveRedact(SelectedOrder, Orders);

        });
        #endregion

    }
}
