using CatalogRJD.Library;
using CatalogRJD.Library.AI;
using CatalogRJD.Library.DB;
using Microsoft.Data.Sqlite;
using Moq;
using System.Data.SqlClient;
using System.Text;

namespace CatalogRJD.Console
{
    internal class Program
    {
        //Пример использования библиотеки для обработки данных с использованием AI
        static async Task Main(string[] args)
        {
            //БД
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "RJDdatabase.db";
            SqliteConnection connection = new SqliteConnection(builder.ConnectionString);
            DAL _dal = new DAL(connection);
            
            //AI
            ModelInteractor modelInteractor = new ModelInteractor("qwen2.5-14b-instruct", "http://127.0.0.1:1234/v1/completions");

            //Обработка данных БД с использованием AI
            DataProcessor processor = new DataProcessor(modelInteractor, _dal);
            await processor.ProcessData(0, 5);

        }
    }
}
