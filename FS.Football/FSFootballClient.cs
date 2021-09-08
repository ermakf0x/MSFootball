using FS.Football.Parsing;
using FS.Football.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FS.Football
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
        public async Task<MatchCollection> GetMatchesAsync(CancellationToken token, IDataParser<MatchCollection> parser)
        {
            var res = await GetMatchesAsync(token);
            var _parser = parser ?? new RegularMatchesParser();
            return _parser.Parse(res);
        }

        public async Task<string> GetH2HAsync(string url, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_hh_1_" + url);
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<string> GetH2HAsync(MatchUrl url, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_hh_1_" + url.HasNull(nameof(url)));
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<H2HParserResult> GetH2HAsync(MatchUrl url, CancellationToken token, IDataParser<H2HParserResult> parser)
        {
            var res = await GetH2HAsync(url, token);
            var _parser = parser ?? new RegularH2HParser();
            return _parser.Parse(res);
        }

        public async Task<string> GetStatisticAsync(MatchUrl url, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_st_1_" + url.HasNull(nameof(url)));
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<MatchStatistic> GetStatisticAsync(MatchUrl url, CancellationToken token, IDataParser<MatchStatistic> parser)
        {
            var res = await GetStatisticAsync(url, token);
            var _parser = parser ?? new RegularStatisticParser();
            return _parser.Parse(res);
        }

        public async Task<string> GetSummaryAsync(string url, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_su_1_" + url);
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<string> GetSummaryAsync(MatchUrl url, CancellationToken token)
        {
            using var message = CreateHttpRequestMessage("/x/feed/df_su_1_" + url.HasNull(nameof(url)));
            return await SendAndReadAsStringAsync(message, token);
        }
        public async Task<MatchSummary> GetSummaryAsync(MatchUrl url, CancellationToken token, IDataParser<MatchSummary> parser)
        {
            var res = await GetSummaryAsync(url, token);
            var _parser = parser ?? new RegularSummaryParser();
            return _parser.Parse(res);
        }

        private HttpRequestMessage CreateHttpRequestMessage(string uri)
        {
            return new HttpRequestMessage(HttpMethod.Get, uri);
        }
        private async Task<string> SendAndReadAsStringAsync(HttpRequestMessage message, CancellationToken token)
        {
            using var response = await SendAsync(message, token);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
