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
    public sealed partial class Search : Page
    {
        private readonly SongService songService = new SongService();
        private int index;
        private string parameters = "";
        private List<Song> list = new List<Song>();
        public Search()
        {
            this.InitializeComponent();
            Loaded += Grid_Loaded;
            mediaPlayerElement.MediaPlayer.MediaFailed += CheckMediaFailed;
            mediaPlayerElement.MediaPlayer.MediaEnded += CheckMediaEnded;
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

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            list = await songService.Search(parameters);
            SearchResultText.Text = list.Count == 0 ? "No Result Found!" : "Found " + list.Count.ToString() + " Song Contains Keywords '" + parameters + "'";
            if (list.Count == 0)
            {
                Process.IsActive = false;
                LastestSong1.Opacity = 0;
            }
            else
            {
                LastestSong1.Opacity = 1;
                ObservableCollection<Song> songs = new ObservableCollection<Song>(list);
                LastestSong1.ItemsSource = songs;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            parameters = (string)e.Parameter;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            mediaPlayerElement.Source = null;
            base.OnNavigatedFrom(e);
        }

        private async void PlayMusic(Song song)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri(song.link));
                SongName.Text = song.name;
                SingerName.Text = song.singer;
                mediaPlayerElement.AutoPlay = true;
                LastestSong1.SelectedIndex = index;
            });
        }

        private void LastestSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song song = LastestSong1.SelectedItem as Song;
            index = list.IndexOf(song);
            PreviousButton.IsEnabled = index != 0;
            NextButton.IsEnabled = index != list.Count - 1;
            PlayMusic(song);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _ = index + 1 < list.Count ? index += 1 : index = list.Count - 1;
            PlayMusic(list[index]);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _ = index - 1 > 0 ? index -= 1 : index = 0;
            PlayMusic(list[index]);
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Process.IsActive = false;
        }
    }
}
