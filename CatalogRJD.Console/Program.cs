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
        static async Task Main(string[] args)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "RJDdatabase.db";

            SqliteConnection connection = new SqliteConnection(builder.ConnectionString);

            DAL _dal = new DAL(connection);

            ModelInteractor modelInteractor = new ModelInteractor();

            DataProcessor processor = new DataProcessor(modelInteractor, _dal);

            await processor.ProcessData();

            _dal.Close();
        }
    }
}
