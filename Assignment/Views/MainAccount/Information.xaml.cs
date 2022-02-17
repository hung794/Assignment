using Assignment.Entity;
using Assignment.Service;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment.Views.MainAccount
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Information : Page
    {
        private readonly AccountService accountService;
        public Information()
        {
            this.InitializeComponent();
            this.accountService = new AccountService();
            Loaded += Information_Loaded;
        }

        private async void Information_Loaded(object sender, RoutedEventArgs e)
        {
            Credential credential = await accountService.LoadToken();
            string token = credential.access_token;
            Account account = await accountService.GetInformation(token);
            Avatar.ImageSource = new BitmapImage(new Uri(account.avatar));
            FullName.Text = account.firstName + " " + account.lastName;
            FirstName.Text = account.firstName;
            LastName.Text = account.lastName;
            Address.Text = account.address;
            Phone.Text = account.phone;
            Birthday.Text = account.birthday;
            switch (account.gender)
            {
                case 1:
                    Gender.Text = "Male";
                    break;
                case 2:
                    Gender.Text = "Female";
                    break;
                case 0:
                    Gender.Text = "Other";
                    break;
            }
            Email.Text = account.email;
            if (account.introduction != null)
            {
                Introduction.Text = account.introduction;
            }
            Template.Opacity = 1;
            Template1.Opacity = 1;
            Process.IsActive = false;
        }
    }
}
