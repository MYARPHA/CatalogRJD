using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogRJD.Library.DB
{
    public class DAL : IDAL
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Соединение с БД
        /// </summary>
        public SqlConnection SqlConnection { get; set; }

        public DAL(string connectionString) 
        {
            ConnectionString = connectionString;
            SqlConnection = new SqlConnection(ConnectionString);
        }
        
        /// <summary>
        /// Открывает соединение с БД
        /// </summary>
        public void Open()
        {
            SqlConnection.Open();
        }

        /// <summary>
        /// Закрывает соединение с БД
        /// </summary>
        public void Close()
        {
            SqlConnection.Close();
        }

        /// <summary>
        /// Возвращает список продуктов
        /// </summary>
        /// <param name="startIndex">Количество пропускаемых строк</param>
        /// <param name="count">Количество возвращаемых строк</param>
        /// <returns>Список продуктов</returns>
        public List<Product> GetProducts(int startIndex, int count)
        {
            List<Product> products = new List<Product>();
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
            
            string query = "SELECT SKIP " + startIndex + " TOP " + count + " * FROM [MTR] LEFT JOIN [OKPD_2] ON [MTR].[ОКПД2] = [OKPD_2].[OKPD2] LEFT JOIN [ED_IZM] ON [MTR].[Базисная Единица измерения] = [ED_IZM].[Код ЕИ]";

            SqlCommand cmd = new SqlCommand(query, SqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();
                product.ScmtrCode = (string)reader["scmtr_code"];
                product.Name = (string)reader["name"];
                product.Marking = (string)reader["marking"];
                product.Regulations = (string)reader["regulations"];
                product.Parameters = (string)reader["parameters"];
                product.EdIzmName = (string)reader["ed_izm"];               
                product.Grouping = (string)reader["grouping"];
                product.Okpd2 = (string)reader["okpd2"];
                products.Add(product);
            }
            return products;
        }

        /// <summary>
        /// Обновляет группу продукта
        /// </summary>
        /// <param name="productId">идентификатор продукта</param>
        /// <param name="group">имя группы</param>
        /// <returns></returns>
        public bool UpdateGroup(string productId, string group)
        {
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();

            string query = "UPDATE [MTR] SET [Группа] = " + group + " WHERE [код СКМТР] = " + productId; 

            SqlCommand command = new SqlCommand(query, SqlConnection);

            return command.ExecuteNonQuery() != 0;
        }

        /// <summary>
        /// Добавляет параметры продукта в БД
        /// </summary>
        /// <param name="productId">идентификатор продукта</param>
        /// <param name="parameters">массив параметров</param>
        public void AddParameters(string productId, string[] parameters)
        {
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();

            string query = "INSERT INTO Parameters (id, name, value) VALUES ";
            foreach (var item in parameters)
            {
                var items = item.Split(": ");
                query += $"('{productId}','{items[0]}','{items[1]}'),";

            }
            query = query.Remove(query.Length - 1);
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);

            sqlCommand.ExecuteNonQuery();
        }
    }
}
