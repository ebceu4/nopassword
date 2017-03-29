using System;
using System.Security;
using System.Windows;
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
                if (_storage.ContainsKey(UserLogin) && _storage.ContainsKey(UserPassword))
                {
                    var savedLogin = _storage.Get(UserLogin);
                    var savedPassword = _storage.Get(UserPassword);

                    if (savedLogin == Login && savedPassword == Password?.ToString())
                    {
                        _navigation.NavigateToDeviceListScreen();
                    }
                    else
                    {
                        MessageBox.Show("Wrong login/password pair.");
                    }
                }
                else
                {
                    _storage.Set(UserLogin, Login);
                    _storage.Set(UserPassword, Password?.ToString());
                    _navigation.NavigateToDeviceListScreen();
                }
            }
            catch (Exception ex)
            {
                //log
                //message
            }
        }
    }
}