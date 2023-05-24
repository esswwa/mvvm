using Microsoft.Extensions.Configuration;
using mvvm.Services;
using Pishi_Wash__Store.ViewModels;

namespace mvvm
{
    public class ViewModelLocator
    {
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;
        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            _configuration = builder.Build();

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<mWindowViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<BrowseProductViewModel>();
            services.AddTransient<BasketInfoViewModel>();
            services.AddTransient<BrowseAdminViewModel>();
            services.AddTransient<AdminListProductsViewModel>();
            #endregion

            #region Connection

            services.AddDbContext<Data.TradeContext>(options =>
            {
                try
                {
                    var conn = _configuration.GetConnectionString("LocalConnection");
                    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                }
                catch (MySqlConnector.MySqlException){}
            }, ServiceLifetime.Singleton);

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<DocumentService>();


            #endregion

            _provider = services.BuildServiceProvider();
            //foreach (var service in services)
            //{
            //    _provider.GetRequiredService(service.ServiceType);
            //}
        }
        public mWindowViewModel? mWindowViewModel => _provider?.GetRequiredService<mWindowViewModel>();
        public SignInViewModel? SignInViewModel => _provider?.GetRequiredService<SignInViewModel>();
        public SignUpViewModel? SignUpViewModel => _provider?.GetRequiredService<SignUpViewModel>();
        public BrowseProductViewModel? BrowseProductViewModel => _provider?.GetRequiredService<BrowseProductViewModel>();
        public BasketInfoViewModel? BasketInfoViewModel => _provider?.GetRequiredService<BasketInfoViewModel>();
        public BrowseAdminViewModel? BrowseAdminViewModel => _provider?.GetRequiredService<BrowseAdminViewModel>();
        public AdminListProductsViewModel? AdminListProductsViewModel => _provider?.GetRequiredService<AdminListProductsViewModel>();
    }
}
