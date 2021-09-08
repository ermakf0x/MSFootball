using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MSFootball.Models
{
    class MSClient
    {
        readonly HttpClient client;

        static MSClient _instance;
        public static MSClient Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MSClient();
                return _instance;
            }
        }

        MSClient()
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip;
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://t.flashscore.ru");
            client.DefaultRequestHeaders.Add("x-fsign", "SW9D1eZo");
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip");

            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.UseNagleAlgorithm = false;
        }

        public Task<string> GetDataAsync(string url)
        {
            return GetDataAsync(url, CancellationToken.None);
        }
        public async Task<string> GetDataAsync(string url, CancellationToken token)
        {
            var request = await client.GetAsync(url, token);
            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsStringAsync();
        }
    }
}
