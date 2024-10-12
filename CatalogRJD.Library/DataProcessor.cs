using CatalogRJD.Library.AI;
using CatalogRJD.Library.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CatalogRJD.Library.AI.ProductParameters;

namespace CatalogRJD.Library
{
    public class DataProcessor
    {
        ModelInteractor _interactor;
        DAL _dal;

        public DataProcessor(ModelInteractor modelInteractor, DAL dal)
        {
            _interactor = modelInteractor;
            _dal = dal;
        }

        public async Task ProcessData(int startIndex = 0, int count = 5)
        {
            _interactor = new ModelInteractor();

            _dal.Open();

            List<Product> products = _dal.GetProducts(startIndex, count);

            foreach (Product product in products)
            {
                var group = await _interactor.Classify(product.Name);

                var parameters = await _interactor.Parameterize(product.Name + " " + product.Marking + " " + product.Parameters);


                _dal.UpdateGroup(product.ScmtrCode, group);

                _dal.AddParameters(product.ScmtrCode, parameters);

                Console.WriteLine(product.ScmtrCode + "\n" + group + "\n" + String.Join("\n", parameters.Select(x => x.parameter_name + ": " + x.parameter_value)));
            }

        }
    }
}



