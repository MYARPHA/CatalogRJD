
using System.Text;

namespace CatalogRJD.Console
{
    internal class Program
    {
        record Product(string name, string m, string par, string okpd);
        static Product product = new Product("3D-ПРИНТЕР","FLYINGBEAR GHOST 5 АРТ.CM000003645","FDM ОБЛАТЬ ПЕЧАТИ 255Х210Х200 DНИТИ 1,75 150ММ/СЕК 24В 300ВТ 388Х337Х411 14,5КГ","26.20.16.140");

        //private static readonly string apiKey = "ваш-ключ-API"; // Ваш API-ключ
        private static readonly string customApiUrl = "http://127.0.0.1:1234/v1/completions"; // Ваш кастомный OpenAI-like эндпоинт

        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            // Установка заголовков для авторизации
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Формирование тела запроса
            var requestBody = new
            {
                model = "lmstudio-community/Qwen2.5-14B-Instruct-GGUF/Qwen2.5-14B-Instruct-Q4_K_M.gguf", // Укажите вашу модель
                prompt = "Выбери категорию для продукта: " + product.name.ToLower(),
                max_tokens = 512,
                response_format = new {
                    type = "json_schema",
                    json_schema = new {
                        name = "product_category_response",
                        strict = "true",
                        schema = new {
                            type = "object",
                            properties = new {
                                product_category = new {
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
            var response = await client.PostAsync(customApiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));

            // Проверяем успешность запроса и читаем ответ
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine("Ответ от API:");
                System.Console.WriteLine(jsonResponse);
            }
            else
            {
                System.Console.WriteLine($"Ошибка: {response.StatusCode}");
            }


            // Формирование тела запроса
            var requestBody2 = new
            {
                model = "lmstudio-community/Qwen2.5-14B-Instruct-GGUF/Qwen2.5-14B-Instruct-Q4_K_M.gguf", // Укажите вашу модель
                prompt = "Укажи список параметров продукта: " + product.name.ToLower() + " " + product.m.ToLower() + " " + product.par.ToLower(),
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
            jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(requestBody2);

            // Отправляем POST-запрос
            response = await client.PostAsync(customApiUrl, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));

            // Проверяем успешность запроса и читаем ответ
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine("Ответ от API:");
                System.Console.WriteLine(jsonResponse);
            }
            else
            {
                System.Console.WriteLine($"Ошибка: {response.StatusCode}");
            }
        }
    }
}
