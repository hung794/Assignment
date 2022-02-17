using Assignment.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service
{
    internal class SongService
    {
        private static readonly string Url = "https://music-i-like.herokuapp.com/api/v1/songs";
        private static readonly string MySongUrl = "/mine";
        public async Task<List<Song>> GetLastestSongs()
        {
            List<Song> list = new List<Song>();
            List<Song> listFinal = new List<Song>();
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage result = await httpClient.GetAsync(Url);
                string content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    list = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                if (list.Count >= 1)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        listFinal.Add(list[i]);
                    }
                }
                return listFinal;
            }
        }

        public async Task<List<Song>> GetMySongs(string token)
        {
            List<Song> list = new List<Song>();
            List<Song> listFinal = new List<Song>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage result = await httpClient.GetAsync(Url + MySongUrl);
                string content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    list = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                if(list.Count >= 1)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        listFinal.Add(list[i]);
                    }
                }
                return listFinal;
            }
        }

        public async Task<bool> AddSong(Song song, string token)
        {
            string json = JsonConvert.SerializeObject(song);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpContent contentToSend = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PostAsync(Url + MySongUrl, contentToSend);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<List<Song>> Search(string keyword)
        {
            List<Song> list = await GetLastestSongs();
            List<Song> listFinal = new List<Song>();
            if (list.Count >= 1)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].name.Contains(keyword) || list[i].singer.Contains(keyword))
                    {
                        listFinal.Add(list[i]);
                    }
                }
            }
            return listFinal;
        }
    }
}
