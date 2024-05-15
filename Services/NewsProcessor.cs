using NewsAPIMobilePark.Models;
using Newtonsoft.Json;

namespace NewsAPIMobilePark.Services
{
    public class NewsProcessor(IConfiguration configuration)
    {
        private Dictionary<string, char[]> _languages = [];

        private readonly IConfiguration _configuration = configuration;

        public void ConfigureLanguages()
        {
            try
            {
                _languages = _configuration?.GetSection("Languages").Get<Dictionary<string, char[]>>();
            }
            catch (Exception)
            {
                //Обработать ошибку о том, что нет в конфиге языков, если надо
            }
        }


        public int CountVowels(string content, string language)
        {
            var result = _languages.TryGetValue(language, out var vowels);
            if (result == false)
            {
                throw new ArgumentException($"Неверный язык: {language}");
            }
            int counter = 0;
            for (int i = 0; i < content.Length; i++)
            {
                for (int j = 0; j < vowels?.Length; j++)
                {
                    var vowel = vowels[j];
                    if (content[i].ToString().Equals(vowel.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        counter++;
                        break;
                    }
                }
            }
            return counter;
        }


        public IEnumerable<KeyValuePair<string, int>> ProcessArticles(IEnumerable<Article>? articles, int startIndex, int endIndex, string language)
        {
            List<KeyValuePair<string, int>> result = [];
            if (articles == null || articles.Any() == false) return result;

            foreach (var article in articles)
            {
                try
                {
                    if (startIndex < 0 || endIndex > article.Content.Length - 1)
                    {
                        throw new ArgumentException($"Неверно заданы границы фрагмента, новость: {article.Content}");
                    }
                }
                catch (Exception)
                {
                    //Обработать ошибку о неправильных границах, если надо
                    continue;
                }
                string fragment = article.Content[startIndex..endIndex];
                var vowelsCount = CountVowels(fragment, language);
                result.Add(new KeyValuePair<string, int>(fragment, vowelsCount));
            }
            return result;
        }


        public async Task<string> GetResults(string query, int startIndex, int endIndex, string language)
        {
            string? apiKey = _configuration?["NewsApiSettings:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return "Не удалось получить APIKey из конфига";
            }
            using HttpClient client = new();
            var url = $"https://newsapi.org/v2/everything?q={query}&apiKey={apiKey}&language={language.ToLower()}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) throw new Exception($"Не уадлось получить данные: {response.ReasonPhrase}");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            NewsData? obj = JsonConvert.DeserializeObject<NewsData>(jsonResponse);
            var result = ProcessArticles(obj?.Articles, startIndex, endIndex, "EN");
            result = result.OrderByDescending(x => x.Value);
            return JsonConvert.SerializeObject(result);
        }
    }
}
