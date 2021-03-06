using Assignment.Entity;
using Assignment.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment.Views.MainAccount
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MySong : Page
    {
        private readonly SongService songService;
        private readonly AccountService accountService;
        private int index;
        private List<Song> list = new List<Song>();
        public MySong()
        {
            this.InitializeComponent();
            this.songService = new SongService();
            this.accountService = new AccountService();
            Loaded += Grid_Loaded;
            mediaPlayerElement.MediaPlayer.MediaFailed += CheckMediaFailed;
            mediaPlayerElement.MediaPlayer.MediaEnded += CheckMediaEnded;
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Credential credential = await accountService.LoadToken();
            list = await songService.GetMySongs(credential.access_token);
            if (list.Count == 0)
            {
                await Dialog.ShowAsync();
                Process.IsActive = false;
                MySong1.Opacity = 0;
            }
            else
            {
                ObservableCollection<Song> songs = new ObservableCollection<Song>(list);
                MySong1.ItemsSource = songs;
                MySong1.Opacity = 1;
            }
        }

        private async void PlayMusic(Song song)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri(song.link));
                SongName.Text = song.name;
                SingerName.Text = song.singer;
                mediaPlayerElement.AutoPlay = true;
                MySong1.SelectedIndex = index;
            });
        }

        private void CheckMediaFailed(object sender, MediaPlayerFailedEventArgs e)
        {
            if (index + 1 < list.Count)
            {
                index += 1;
                PlayMusic(list[index]);
            }
        }

        private void CheckMediaEnded(MediaPlayer sender, object args)
        {
            if (index < list.Count - 1)
            {
                index += 1;
                PlayMusic(list[index]);
            }
        }

        private void MySong1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song song = MySong1.SelectedItem as Song;
            index = list.IndexOf(song);
            if (index == 0)
            {
                PreviousButton.IsEnabled = false;
            }
            else
            {
                PreviousButton.IsEnabled = true;
            }
            if (index == list.Count - 1)
            {
                NextButton.IsEnabled = false;
            }
            else
            {
                NextButton.IsEnabled = true;
            }
            PlayMusic(song);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            mediaPlayerElement.MediaPlayer.Pause();
            base.OnNavigatedFrom(e);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _ = index - 1 > 0 ? index -= 1 : index = 0;
            PlayMusic(list[index]);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _ = index + 1 < list.Count ? index += 1 : index = list.Count - 1;
            PlayMusic(list[index]);
        }

        private void MySong1_Loaded(object sender, RoutedEventArgs e)
        {
            Process.IsActive = false;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _ = Main.frame.Navigate(typeof(AddSong));
        }
    }
}