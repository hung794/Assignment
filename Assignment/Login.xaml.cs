using Assignment.Entity;
using Assignment.Service;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if(Email.Text == "")
            {
                EmailError.Text = "Email is required!";
                count += 1;
            }
            else EmailError.Text = "";
            if (Password.Password == "")
            {
                PasswordError.Text = "Password is required!";
                count += 1;
            }
            else PasswordError.Text = "";
            if(count == 0)
            {
                LoginInfomation loginInfomation = new LoginInfomation
                {
                    email = Email.Text,
                    password = Password.Password.ToString()
                };
                AccountService accountService = new AccountService();
                bool loginSuccess = await accountService.Login(loginInfomation);
                if (loginSuccess)
                {
                    _ = Frame.Navigate(typeof(Views.Main));
                }
                else
                {
                    ContentDialog contentDialog = new ContentDialog()
                    {
                        Title = "Login",
                        Content = "Invalid Email or Password, please try again!",
                        CloseButtonText = "Back"
                    };
                    _ = await contentDialog.ShowAsync();
                }
            }
        }

        private void RegisterForm_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Views.Register));
        }
    }
}
