using CatalogRJD.Library.AI;
using CatalogRJD.Library.DB;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogRJD.Library
{
    public class DataProcessor
    {
        public async Task ProcessData(int startIndex=0, int count=5)
        {
            Mock<IDAL> mockdal = new Mock<IDAL>();
            mockdal.Setup(x => x.GetProducts(startIndex, count)).Returns(() => { return new List<Product>() 
            { 
                new Product() { Id="4573610892", Name= "ЩЕТКА ГЕНЕРАТОРА", Mark = "BOSCH 1127014027 (BX2152)", Parameters = "5Х8Х23", MeasureUnitId = "796", Okpd2= "29.31.22.190", Okpd2Name="Оборудование электрическое прочее для транспортных средств, не включенное в другие группировки" }
            }; });

            IDAL dal = mockdal.Object;

            ModelInteractor interactor = new ModelInteractor();

            dal.Open();

            List<Product> products = dal.GetProducts(startIndex,count);

            foreach (Product product in products)
            {
                var group = await interactor.Classify(product.Name);

                var parameters = await interactor.Parameterize(product.Name + " " + product.Mark + " " + product.Parameters + " " + product.Okpd2Name);

                //Console.WriteLine(product.Id + " " + group + "\n" + String.Join("\n", parameters));

                dal.UpdateGroup(product.Id, group);

                dal.AddParameters(product.Id, parameters);
            }

            dal.Close();
        }
    }
}
