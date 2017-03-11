using System.Windows;
using System.Windows.Controls;
using NoPassword.ViewModels;

namespace NoPassword.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBoxOnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var loginViewModel = DataContext as LoginViewModel;

            if (loginViewModel != null)
            {
                loginViewModel.Password = (sender as PasswordBox)?.SecurePassword;
            }
        }
    }
}
