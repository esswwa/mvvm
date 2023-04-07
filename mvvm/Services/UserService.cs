namespace mvvm.Services
{
    public class UserService
    {
        private readonly Data.TradeContext _tradeContext;
        public UserService(Data.TradeContext tradeContext)
        {
            _tradeContext = tradeContext;
        }
        public async Task<bool> AuthorizationAsync(string username, string password)
        {
            var user = await _tradeContext.Users.SingleOrDefaultAsync(u => u.UserLogin == username);
            if (user == null)
                return false;
            if (user.UserPassword.Equals(password))
            {
                await _tradeContext.Roles.ToListAsync();
                Settings.Default.UserId = user.UserId;
                Settings.Default.UserName = user.UserName;
                Settings.Default.UserSurname = user.UserSurname;
                Settings.Default.UserPatronymic = user.UserPatronymic;
                Settings.Default.RoleName = user.UserRoleNavigation.RoleName;
                return true;
            }
            return false;


        }

        public async Task<List<string>> getAllUser()
        {
            return await _tradeContext.Users.Select(u => u.UserLogin).AsNoTracking().ToListAsync();
        }

        public async Task RegistrationAsync(string UserName, string UserSurname, string UserPatronymic, string UserLogin, string UserPassword)
        {
            await _tradeContext.Users.AddAsync(new User
            {
                UserName = UserName,
                UserSurname = UserSurname,
                UserPatronymic = UserPatronymic,
                UserLogin = UserLogin,
                UserPassword = UserPassword,
                UserRole = 2
            });
            await _tradeContext.SaveChangesAsync();
        }
    }
}
