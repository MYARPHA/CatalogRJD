namespace CatalogRJD.Library.DB
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; }
        public string Reglaments { get; set; }
        public string Parameters { get; set; }
        public List<string> ParametersList { get; set; }
        public string MeasureUnitId { get; set; }
        public string MeasureUnit { get; set; }
        public string Okpd2 { get; set; }
        public string Okpd2Name { get; set; }

    }
}