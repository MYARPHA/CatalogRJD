using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using CatalogRJD.Library;
using CatalogRJD.Library.DB;
using Microsoft.Data.Sqlite;

namespace CatalogRJD.DesktopDemo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }

        public MainViewModel()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "RJDdatabase.db";
            SqliteConnection connection = new SqliteConnection(builder.ConnectionString);
            DAL dal = new DAL(connection);

            List<Product> products = dal.GetProducts(0,15);
            foreach (var product in products)
            {
                product.ParametersList = dal.GetParameters(product.ScmtrCode);
            }
            Products = products;
        }

        public ICommand SelectCommand => new Command(x=>SelectProduct((Product)x));

        public void SelectProduct(Product product)
        {
            if (product != null)
            {
                Product = product;
                OnPropertyChanged(nameof(Product));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
