using System.Text;

namespace CatalogRJD.Library.AI
{
    public class ModelInteractor
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Объект для взаимодействия с OpenAI-like API языковой модели
        /// </summary>
        /// <param name="aiModel">API-Идентификатор модели</param>
        /// <param name="apiUrl">Web-Адрес API</param>
        public ModelInteractor(string aiModel = "qwen2.5-14b-instruct", string apiUrl = "http://127.0.0.1:1234/v1/completions")
        {
            AiModel = aiModel;
            ApiUrl = apiUrl;
        }

        /// <summary>
        /// API-Идентификатор модели
        /// </summary>
        public string AiModel { get; set; }

        /// <summary>
        /// Web-Адрес API
        /// </summary>
        public string ApiUrl { get; set; }


        /// <summary>
        /// Получить категорию продукта на основе запроса к OpenAI-like API языковой модели 
        /// </summary>
        /// <param name="text">текстовое описание продукта</param>
        /// <returns>категория продукта</returns>
        /// <exception cref="HttpRequestException">Ошибка HTTP запроса к API</exception>
        public async Task<string> Classify(string text)
        {
            using (_httpClient = new HttpClient())
            {
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
                var response = await _httpClient.PostAsync(ApiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Error: {response.StatusCode}");

                JsonResponse jsonResponse = System.Text.Json.JsonSerializer.Deserialize<JsonResponse>(await response.Content.ReadAsStringAsync());

                var category = System.Text.Json.JsonSerializer.Deserialize<ProductCategory>(jsonResponse.choices.FirstOrDefault().text);
                return category.product_category;
            }
        }

        /// <summary>
        /// Получить параметры продукта на основе запроса к OpenAI-like API языковой модели
        /// </summary>
        /// <param name="text">текстовое описание продукта и его параметров</param>
        /// <returns>массив строк с параметрами продукта</returns>
        /// <exception cref="HttpRequestException">Ошибка HTTP запроса к API</exception>
        public async Task<string[]> Parameterize(string text)
        {
            using (_httpClient = new HttpClient())
            {
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
                var response = await _httpClient.PostAsync(ApiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Error: {response.StatusCode}");

                JsonResponse jsonResponse = System.Text.Json.JsonSerializer.Deserialize<JsonResponse>(await response.Content.ReadAsStringAsync());

                var parameters = System.Text.Json.JsonSerializer.Deserialize<ProductParameters>(jsonResponse.choices.FirstOrDefault().text).product_parameters;
                return parameters;
            }
        }
    }
}


