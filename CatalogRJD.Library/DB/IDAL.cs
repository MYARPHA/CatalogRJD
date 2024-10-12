using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogRJD.Library.DB
{
    public interface IDAL
    {
        public void Open();
        public void Close();

        public List<Product> GetProducts(int startIndex, int count);
        public bool UpdateGroup(string productId, string group);

        public void AddParameters(string productId, string[] parameters);
    }
}
