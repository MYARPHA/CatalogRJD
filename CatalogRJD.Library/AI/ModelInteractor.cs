using System.Text;

namespace CatalogRJD.Library.AI
{
    public class ModelInteractor
    {
        private HttpClient _httpCient;
        public string AiModel { get; set; } = "qwen2.5-14b-instruct";
        public string ApiUrl { get; set; } = "http://127.0.0.1:1234/v1/completions";

        public async Task<string> Classify(string text)
        {
            _httpCient = new HttpClient();

            var requestBody = new
            {
                model = AiModel,
                prompt = "Выбери категорию для продукта: " + text,
                max_tokens = 512,
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "product_category_response",
                        strict = "true",
                        schema = new
                        {
                            type = "object",
                            properties = new
                            {
                                product_category = new
                                {
                                    type = "string"
                                }
                            },
                            required = new[] { "product_category" }
                        }
                    }
                },
                temperature = 0.7
            };

            // Преобразуем тело запроса в JSON
            var jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(requestBody);

            // Отправляем POST-запрос
            var response = await _httpCient.PostAsync(ApiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) throw new Exception($"Error: {response.StatusCode}");

            JsonResponse jsonResponse = System.Text.Json.JsonSerializer.Deserialize<JsonResponse>(await response.Content.ReadAsStringAsync());

            var category = System.Text.Json.JsonSerializer.Deserialize<ProductCategory>(jsonResponse.choices.FirstOrDefault().text);
            return category.product_category;
        }


        public async Task<string[]> Parameterize(string text)
        {
            _httpCient = new HttpClient();

            var requestBody = new
            {
                model = AiModel,
                prompt = "Укажи список параметров продукта: " + text,
                max_tokens = 512,
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "product_parameters_response",
                        strict = "true",
                        schema = new
                        {
                            type = "object",
                            properties = new
                            {
                                product_parameters = new
                                {
                                    type = "array",
                                    items = new
                                    {
                                        type = "string"
                                    }
                                }
                            },
                            required = new[] { "product_parameters" }
                        }
                    }
                },
                temperature = 0.7
            };

            // Преобразуем тело запроса в JSON
            var jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(requestBody);

            // Отправляем POST-запрос
            var response = await _httpCient.PostAsync(ApiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) throw new Exception($"Error: {response.StatusCode}");

            JsonResponse jsonResponse = System.Text.Json.JsonSerializer.Deserialize<JsonResponse>(await response.Content.ReadAsStringAsync());

            var parameters = System.Text.Json.JsonSerializer.Deserialize<ProductParameters>(jsonResponse.choices.FirstOrDefault().text).product_parameters;
            return parameters;
        }
    }
}


