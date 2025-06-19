using Azure.AI.TextAnalytics;

namespace chat_backend.Modules.OnlineChat.Services
{
    public class SentimentService
    {
        private readonly TextAnalyticsClient _client;

        public SentimentService(TextAnalyticsClient client)
        {
            _client = client;
        }

        public async Task<TextSentiment> AnalyzeSentimentAsync(string message)
        {
            var response = await _client.AnalyzeSentimentAsync(message);
            var sentiment = response.Value.Sentiment;
            return sentiment;
        }
    }
}
