using CatalogRJD.Library;
using CatalogRJD.Library.AI;
using CatalogRJD.Library.DB;
using Moq;
using System.Data.SqlClient;
using System.Text;

namespace CatalogRJD.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Mock<IDAL> mockdal = new Mock<IDAL>();
            //mockdal.Setup(x => x.GetProducts(0, 5)).Returns(() => {
            //    return new List<Product>()
            //{
            //    new Product() { Id="4573610892", Name= "ЩЕТКА ГЕНЕРАТОРА", Mark = "BOSCH 1127014027 (BX2152)", Parameters = "5Х8Х23", MeasureUnitId = "796", Okpd2= "29.31.22.190", Okpd2Name="Оборудование электрическое прочее для транспортных средств, не включенное в другие группировки" }
            //};
            //});


            //IDAL _dal = mockdal.Object;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "RJDdatabase.db";

            DAL _dal = new DAL(builder.ConnectionString);

            ModelInteractor modelInteractor = new ModelInteractor();

            DataProcessor processor = new DataProcessor(modelInteractor, _dal);

            await processor.ProcessData();
        }
    }
}
