namespace RdpBotApp
{
    public class TelegramHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TelegramHttpClient> _logger;
        public TelegramHttpClient(ILogger<TelegramHttpClient> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task SendMessageToBot(string botTokenId, string roomId, string message)
        {
            string url = $"https://api.telegram.org/bot{botTokenId}/sendMessage?chat_id={roomId}&text={message}";
            var httpResponse = await _httpClient.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation($"[SUCCESS] {botTokenId} send {message} to {roomId}");
            }
            else
            {
                _logger.LogInformation($"[FAILED] {botTokenId} send {message} to {roomId} failed with {httpResponse.StatusCode}");
            }
        }
    }
}
