using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FlashScore.Football
{
    public class FSFootballClient : HttpClient
    {
        private FSFootballClient(HttpMessageHandler handler) : base(handler)
        {
            BaseAddress = new Uri("https://t.flashscore.ru");
            DefaultRequestHeaders.Add("x-fsign", "SW9D1eZo");
            DefaultRequestHeaders.Add("accept-encoding", "gzip");
        }
        public static FSFootballClient Create(HttpMessageHandler handler = null)
        {
            var _handler = handler ?? new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            };
            return new FSFootballClient(_handler);
        }

        public async Task<string> GetMatchesAsync(CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/f_1_0_2_cs_1_0");
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<string> GetH2HAsync(FSID id, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_hh_1_" + id.ToString());
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<string> GetStatisticAsync(FSID id, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_st_1_" + id.ToString());
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<string> GetSummaryAsync(FSID id, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_su_1_" + id.ToString());
            return await SendAndReadAsStringAsync(message, token);
        }

        static HttpRequestMessage CreateHttpRequestMessage(string uri)
        {
            return new HttpRequestMessage(HttpMethod.Get, uri);
        }
        async Task<string> SendAndReadAsStringAsync(HttpRequestMessage message, CancellationToken token)
        {
            using var response = await SendAsync(message, token);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
