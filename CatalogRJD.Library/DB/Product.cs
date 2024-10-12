namespace CatalogRJD.Library.DB
{
    public class Product
    {
        public string ScmtrCode { get; set; }
        public string Name { get; set; }
        public string Marking { get; set; }
        public string Regulations { get; set; }
        public string Parameters { get; set; }
        public List<string> ParametersList { get; set; }
        public string EdIzmName { get; set; }
        public string Okpd2 { get; set; }
        public string Okpd2Name { get; set; }
        public string Grouping { get; set; }

    }
}