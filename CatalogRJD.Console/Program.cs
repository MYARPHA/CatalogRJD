using CatalogRJD.Library;
using CatalogRJD.Library.AI;
using System.Text;

namespace CatalogRJD.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            DataProcessor processor = new DataProcessor();
            await processor.ProcessData();
        }
    }
}
