using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CatalogRJD.Library.AI.ProductParameters;

namespace CatalogRJD.Library.DB
{
    public class DAL
    {
        /// <summary>
        /// Соединение с БД
        /// </summary>
        public IDbConnection SqlConnection { get; set; }

        public DAL(SqliteConnection connection)
        {
            SqlConnection = connection;
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
            
            string query = "SELECT * FROM [MTR] LEFT JOIN [OKPD_2] ON [MTR].[okpd2] = [OKPD_2].[okpd2_code] LEFT JOIN [ED_IZM] ON [MTR].[ed_izm] = [ED_IZM].[ei_code] LIMIT " + startIndex + ", " + count;

            IDbCommand cmd = SqlConnection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;

            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();
                product.ScmtrCode = (string)reader["scmtr_code"];
                product.Name = (string)reader["name"];
                product.Marking = (string)reader["marking"];
                product.Regulations = (string)reader["regulations"];
                product.Parameters = (string)reader["parameters"];
                product.EdIzmName = (string)reader["ed_izm"];               
                product.Grouping = (string)reader["category"];
                product.Okpd2 = (string)reader["okpd2"];
                products.Add(product);
            }
            Close();
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

            string query = "UPDATE [MTR] SET category = '" + group + "' WHERE [scmtr_code] = " + productId;

            IDbCommand cmd = SqlConnection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            bool result = cmd.ExecuteNonQuery() != 0;
            Close();
            return result;
        }

        /// <summary>
        /// Добавляет параметры продукта в БД
        /// </summary>
        /// <param name="productId">идентификатор продукта</param>
        /// <param name="parameters">массив параметров</param>
        public void AddParameters(string productId, ProductParameter[] parameters)
        {
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();

            string query = "INSERT INTO Parameters (product_id, name, value) VALUES ";
            foreach (var item in parameters)
            {
                query += $"('{productId}','{item.parameter_name}','{item.parameter_value}'),";

            }
            query = query.Remove(query.Length - 1);

            IDbCommand cmd = SqlConnection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();
            Close();
        }
    }
}
