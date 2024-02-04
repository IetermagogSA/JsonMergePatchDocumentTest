namespace JsonMergePatchDocumentTest.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal PriceIncl { get; set; }
        public ICollection<OptionSize> OptionSizes { get; set; } = new List<OptionSize>();
    }
}
