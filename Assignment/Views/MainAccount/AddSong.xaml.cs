using Assignment.Entity;
using Assignment.Service;
using Assignment.Util;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment.Views.MainAccount
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddSong : Page
    {
        private string imageUrl = "";
        private string songUrl = "";
        public AddSong()
        {
            this.InitializeComponent();
        }

        private async void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            AddMusicSuccessText.Text = "";
            string result = await FileCloudinary.ChooseFile("image");
            if(result != "")
            {
                AddImgSuccessText.Text = "Success!";
                imageUrl = result;
            }
        }

        private async void ChooseSong_Click(object sender, RoutedEventArgs e)
        {
            AddMusicSuccessText.Text = "";
            string result = await FileCloudinary.ChooseFile("music");
            if (result != "")
            {
                AddMusicSuccessText.Text = "Success!";
                songUrl = result;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Name.Text = NameError.Text = "";
            Description.Text = DescriptionError.Text = "";
            Singer.Text = SingerError.Text = "";
            Author.Text = AuthorError.Text = "";
            songUrl = imageUrl = "";
            ThumbnailError.Text = AddImgSuccessText.Text = "";
            AddMusicSuccessText.Text = LinkError.Text = "";
        }

        private async void AddSong_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if(Name.Text == "")
            {
                NameError.Text = "Name is required!";
                count += 1;
            }
            else NameError.Text = "";
            if (Description.Text == "")
            {
                DescriptionError.Text = "Description is required!";
                count += 1;
            }
            else DescriptionError.Text = "";
            if (Singer.Text == "")
            {
                SingerError.Text = "Singer is required!";
                count += 1;
            }
            else SingerError.Text = "";
            if (Author.Text == "")
            {
                AuthorError.Text = "Author is required!";
                count += 1;
            }
            else AuthorError.Text = "";
            if (imageUrl == "")
            {
                ThumbnailError.Text = "Thumbnail is required!";
            }
            else
            {
                ThumbnailError.Text = "";
            }

            if (songUrl == "")
            {
               LinkError.Text = "Link is required!";
                count += 1;
            }
            else LinkError.Text = "";
            if (count == 0)
            {
                Song song = new Song
                {
                    name = Name.Text,
                    description = Description.Text,
                    singer = Singer.Text,
                    author = Author.Text,
                    thumbnail = imageUrl,
                    link = songUrl
                };
                SongService songService = new SongService();
                AccountService accountService = new AccountService();
                Credential credential = await accountService.LoadToken();
                bool isSuccess = await songService.AddSong(song, credential.access_token);
                Dialog.Content = !isSuccess ? "Add Song Success!" : "Add Song Failed, Please Try Again!";
                _ = await Dialog.ShowAsync();
                Process.IsActive = true;
                if (Dialog.IsLoaded)
                {
                    Process.IsActive = false;
                }
            }
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _ = Main.frame.Navigate(typeof(MySong));
        }
    }
}
