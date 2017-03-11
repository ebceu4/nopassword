using System;
using System.Security;
using Caliburn.Micro;
using NoPassword.General;
using NoPassword.General.Navigation;

namespace NoPassword.ViewModels
{
    public sealed class LoginViewModel : Screen
    {
        private const string UserLogin = "USER_LOGIN";
        private const string UserPassword = "USER_PASSWORD";

        private readonly IStorage _storage;
        private readonly INavigation _navigation;

        public string Login { get; set; }

        public SecureString Password { get; set; }

        public LoginViewModel(INavigation navigation, IStorage storage)
        {
            DisplayName = "Login";

            _navigation = navigation;
            _storage = storage;
        }

        public void DoLogin()
        {
            if (string.IsNullOrEmpty(Login) || (Password?.Length).GetValueOrDefault() == 0)
                return;

            try
            {
                _storage.Set(UserLogin, Login);
                _storage.Set(UserPassword, Password?.ToString());

                _navigation.NavigateToDeviceListScreen();
            }
            catch (Exception ex)
            {
                //log
                //message
            }
        }
    }
}