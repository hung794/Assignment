using Assignment.Entity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Assignment.Service
{
    internal class AccountService
    {
        private static readonly string Url = "https://music-i-like.herokuapp.com/api/v1/accounts";
        private static readonly string LoginUrl = "/authentication";
        private static readonly string FileName = "credential.txt";

        public async Task<bool> Register(Account account)
        {
            string json = JsonConvert.SerializeObject(account);
            HttpClient httpClient = new HttpClient();
            HttpContent contentToSend = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PostAsync(Url, contentToSend);
            return result.StatusCode == System.Net.HttpStatusCode.Created;
        }

        public async Task<bool> Login(LoginInfomation loginInfomation)
        {

            string json = JsonConvert.SerializeObject(loginInfomation);
            HttpClient httpClient = new HttpClient();
            HttpContent contentToSend = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpClient.PostAsync(Url + LoginUrl, contentToSend);
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await result.Content.ReadAsStringAsync();
                await SaveToken(content);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task SaveToken(string content)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, content);
        }

        public async Task<Credential> LoadToken()
        {
            string rootPath = ApplicationData.Current.LocalFolder.Path;
            string filePath = Path.Combine(rootPath, FileName);
            if (File.Exists(filePath)) {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile tokenFile = await storageFolder.GetFileAsync(FileName);
                string tokenString = await FileIO.ReadTextAsync(tokenFile);
                Credential credential = JsonConvert.DeserializeObject<Credential>(tokenString);
                return credential;
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteToken()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile tokenFile = await storageFolder.GetFileAsync(FileName);
            await tokenFile.DeleteAsync();
        }

        public async Task<Account> GetLoggedInAccount()
        {
            Credential credential = await LoadToken();
            if (credential == null)
            {
                return null;
            }
            else
            {
                string token = credential.access_token;
                Account account = await GetInformation(token);
                return account;
            }
        }

        public async Task<Account> GetInformation(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage result = await httpClient.GetAsync(Url);
                string content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Account account = JsonConvert.DeserializeObject<Account>(content);
                    return account;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
