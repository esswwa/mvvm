namespace mvvm.ViewModels
{
    public class mWindowViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public Page PageSource { get; set; }
        public mWindowViewModel(PageService pageService)
        {
            _pageService = pageService;

            _pageService.onPageChanged += (page) => PageSource = page;

            _pageService.ChangePage(new SingInPage());
        }
    }
}
