﻿using CatalogRJD.Library.AI;
using CatalogRJD.Library.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogRJD.Library
{
    public class DataProcessor
    {
        ModelInteractor _interactor;
        IDAL _dal;

        public DataProcessor(ModelInteractor modelInteractor, IDAL dal)
        { 
            _interactor = modelInteractor;
            _dal = dal;
        }

        public async Task ProcessData(int startIndex=0, int count=5)
        {
            _interactor = new ModelInteractor();

            _dal.Open();

            List<Product> products = _dal.GetProducts(startIndex,count);

            foreach (Product product in products)
            {
                var group = await _interactor.Classify(product.Name + " " + product.Mark);

                var parameters = await _interactor.Parameterize(product.Name + " " + product.Mark + " " + product.Parameters + " " + product.Okpd2Name);

                _dal.UpdateGroup(product.Id, group);

                _dal.AddParameters(product.Id, parameters);

                Console.WriteLine(product.Id + "\n" + group + "\n" + String.Join("\n", parameters));
            }

            _dal.Close();
        }
    }
}