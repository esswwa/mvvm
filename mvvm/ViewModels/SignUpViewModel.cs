using mvvm.Services;

namespace mvvm.ViewModels
{
    public class SignUpViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly UserService _userService;
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserPatronymic { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string ErrorMessage { get; set; } = "Обязательно";
        private List<string> _userLogin { get; set; } = new();

        public SignUpViewModel(PageService pageService, UserService userService)
        {
            _pageService = pageService;
            _userService = userService;
            Task.Run(async () => _userLogin = await _userService.getAllUser());
        }
        public AsyncCommand SignUpCommand => new(async () =>
        {
            await _userService.RegistrationAsync(UserName, UserSurname, UserPatronymic, UserLogin, UserPassword);
            _pageService.ChangePage(new SingInPage());
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(UserName)
            || string.IsNullOrWhiteSpace(UserSurname)
            || string.IsNullOrWhiteSpace(UserPatronymic)
            || string.IsNullOrWhiteSpace(UserLogin)
            || string.IsNullOrWhiteSpace(UserPassword))
                ErrorMessage = "Обязательно";
            else if (UserLogin.Length < 4)
                ErrorMessage = "Слишком короткий логин";
            else if (_userLogin.Contains(UserLogin))
                ErrorMessage = "Логин занят";
            else
                ErrorMessage = string.Empty;

            return ErrorMessage == string.Empty;
        });
        public DelegateCommand SignInCommand => new(() =>
        {
            _pageService.ChangePage(new SingInPage());
        });
    }
}
