namespace CatalogRJD.Library.AI
{
    /// <summary>
    /// Результат выполнения запроса к OpenAI-like API
    /// </summary>
    public class JsonResponse
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        /// <summary>
        /// Ответы от языковой модели
        /// </summary>
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }

        public class Usage
        {
            public int prompt_tokens { get; set; }
            public int completion_tokens { get; set; }
            public int total_tokens { get; set; }
        }

        public class Choice
        {
            public int index { get; set; }
            public string text { get; set; }
            public object logprobs { get; set; }
            public string finish_reason { get; set; }
        }
    }
}