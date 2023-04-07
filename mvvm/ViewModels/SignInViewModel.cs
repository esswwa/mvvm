using mvvm.Services;

namespace mvvm.ViewModels
{
    public class SignInViewModel : BindableBase
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageButton { get; set; }
        public SignInViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }
        public AsyncCommand SignInCommand => new(async () =>
        {
            await Task.Run(async () =>
            {
                if (await _userService.AuthorizationAsync(Username, Password))
                {
                    ErrorMessageButton = string.Empty;
                    await Application.Current.Dispatcher.InvokeAsync(async () => _pageService.ChangePage(new BrowseProductPages()));
                }
                else
                    ErrorMessageButton = "Неверный логин или пароль";
            });
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Пустые поля";
                ErrorMessageButton = string.Empty;
            }
            else
                ErrorMessage = string.Empty;

            if (ErrorMessage.Equals(string.Empty))
                return true; return false;
        });
        public DelegateCommand SignUpCommand => new(() => _pageService.ChangePage(new SignUpPage()));
        public DelegateCommand SignInLaterCommand => new(() => _pageService.ChangePage(new BrowseProductPages()));
    }
}
