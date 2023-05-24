using mvvm.Views;
using Org.BouncyCastle.Asn1.X500;

namespace mvvm.ViewModels
{
    public class BasketInfoViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly DocumentService _documentService;


        private static readonly Random rnd = new();

        public ObservableCollection<DbProduct> Products { get; set; }
        public List<Point> Points { get; set; }
        public DbProduct SelectedProduct { get; set; }
        public Point SelectedPoint { get; set; }
        public float OrderAmmount { get; set; } = 0;
        public float DiscountAmmount { get; set; } = 0;
        private float _orderAmmount;
        public string FullName { get; set; } = Settings.Default.UserName == string.Empty ? "Гость" :
                    $"{Settings.Default.UserSurname} {Settings.Default.UserName} {Settings.Default.UserPatronymic}";


        public BasketInfoViewModel (PageService pageService, ProductService productService,DocumentService documentService){

            _pageService = pageService;
            _productService = productService;
            _documentService = documentService;
            Task.Run(async () => { Products = new ObservableCollection<DbProduct>(await _productService.getBasket()); ValueCheck();
            Points = await _productService.GetPoints(); });
        }
        public DelegateCommand ReturnBackCommand => new(() =>
        {
            _pageService.ChangePage(new BrowseProductPages());
        });

        public DelegateCommand SignOutCommand => new(() =>
        {
            Settings.Default.UserId = 0;
            Settings.Default.UserName = string.Empty;
            Settings.Default.UserSurname = string.Empty;
            Settings.Default.UserPatronymic = string.Empty;
            Settings.Default.RoleName = string.Empty;
            Global.CurrentCart.Clear();
            _pageService.ChangePage(new SingInPage());
        });

        public DelegateCommand RemoveCommand => new(() =>
        {
            if (SelectedProduct == null)
                return;
            var item = Products.First(i => i.Article.Equals(SelectedProduct.Article));
            var index = Products.IndexOf(item);
            item.Count--;

            var test = Global.CurrentCart.First(x => x.ArticleName.Equals(SelectedProduct.Article));
            var test2 = Global.CurrentCart.IndexOf(test);

            if (item.Count <= 0)
            {
                Products.Remove(SelectedProduct);
                Global.CurrentCart.Remove(test);
            }
            else
            {
                Products.RemoveAt(index);
                Products.Insert(index, item);
                Global.CurrentCart[test2].Count--;
            }
            ValueCheck();
        });
        private void ValueCheck()
        {
            OrderAmmount = 0;
            DiscountAmmount = 0;
            _orderAmmount = 0;
            if (Products.Count <= 0)
            {
                OrderAmmount = 0;
                DiscountAmmount = 0;
            }
            else
            {
                foreach (var item in Products)
                {
                    OrderAmmount += (item.Count * item.Price) - ((item.Count * item.Price) * item.Discount / 100);
                    _orderAmmount += item.Count * item.Price;
                }
                OrderAmmount = (float)Math.Round(OrderAmmount, 2);
                DiscountAmmount = (float)Math.Round((_orderAmmount - OrderAmmount), 2);
            }
        }

        public AsyncCommand CreateOrderCommand => new(async () =>
        {
            int code = rnd.Next(100, 999);
            await _documentService.GetCheck(SelectedPoint, code, await _productService.AddOrder(new Orderuser
            {
                OrderBeginDate = DateOnly.FromDateTime(DateTime.Now),
                OrderDeliveryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                OrderPickupPoint = SelectedPoint.IdPickupPoint,
                OrderFIO = FullName == "Гость" ? string.Empty : FullName,
                OrderCode = code,
                OrderStatus = "Новый"
            }), await _productService.GetListFullInformation());

            Products.Clear();
            Global.CurrentCart?.Clear();
            ValueCheck();
        }, bool () => { return SelectedPoint != null && Products.Count != 0; });

    }
}
