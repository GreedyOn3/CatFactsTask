namespace NetwiseTask.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;

        public TranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TranslateToPolishAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var encodedText = Uri.EscapeDataString(text);

            var url =
                $"https://api.mymemory.translated.net/get" +
                $"?q={encodedText}&langpair=en|pl";

            var response = await _httpClient.GetFromJsonAsync<MyMemoryResponse>(
                url);

            if (response?.ResponseData?.TranslatedText is null)
            {
                throw new InvalidOperationException(
                    "Nie udało się przetłumaczyć faktu.");
            }

            return response.ResponseData.TranslatedText;
        }

        private class MyMemoryResponse
        {
            public ResponseData? ResponseData { get; set; }
        }

        private class ResponseData
        {
            public string TranslatedText { get; set; } = string.Empty;
        }

    }
}
