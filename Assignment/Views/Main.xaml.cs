using Assignment.Entity;
using Assignment.Service;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main : Page
    {
        public static Frame frame;
        private readonly AccountService accountService;
        public Main()
        {
            this.InitializeComponent();
            this.accountService = new AccountService();
            this.Loaded += NavigationView_Loaded;
            frame = contentFrame;
        }

        private async void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            Credential credential = await accountService.LoadToken();
            string token = credential.access_token;
            Account account = await accountService.GetInformation(token);
            Greeting.Content = "Welcome, " + account.firstName + " " + account.lastName;
            _ = contentFrame.Navigate(typeof(MainAccount.Information));
        }

        private async void NavigationViewItem_Invoked(NavigationView sender, NavigationViewItemInvokedEventArgs e)
        {
            NavigationViewItemBase item = e.InvokedItemContainer;
            switch (item.Tag)
            {
                case "Information":
                    _ = contentFrame.Navigate(typeof(MainAccount.Information));
                    break;
                case "MySong":
                    _ = contentFrame.Navigate(typeof(MainAccount.MySong));
                    break;
                case "AddSong":
                    _ = contentFrame.Navigate(typeof(MainAccount.AddSong));
                    break;
                case "LastestSong":
                    _ = contentFrame.Navigate(typeof(MainAccount.LastestSong));
                    break;
                case "Logout":
                    _ = contentFrame.Navigate(typeof(MainAccount.BlankPage), null);
                    await accountService.DeleteToken();
                    _ = Frame.Navigate(typeof(Login));
                    break;
            }
        }

        private void SearchTxt_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            AutoSuggestBox suggestBox = sender;
            if(suggestBox.Text != "")
            {
                string text = suggestBox.Text;
                nvSample.SelectedItem = null;
                _ = contentFrame.Navigate(typeof(MainAccount.Search), text);
            }
        }
    }
}