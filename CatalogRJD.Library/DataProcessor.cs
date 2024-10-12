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
    /// <summary>
    /// Обработчик данных
    /// </summary>
    public class DataProcessor
    {
        ModelInteractor _interactor;
        DAL _dal;

        public DataProcessor(ModelInteractor modelInteractor, DAL dal)
        {
            _interactor = modelInteractor;
            _dal = dal;
        }

        /// <summary>
        /// Обрабатывает данные продуктов, обновляет категории, добавляет параметры
        /// </summary>
        /// <param name="startIndex">количество пропускаемых строк</param>
        /// <param name="count">количество обрабатываемых строк</param>
        /// <returns></returns>
        public async Task ProcessData(int startIndex, int count)
        {
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



