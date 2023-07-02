namespace BioShop.Data.ViewModels.ProductModels
{
    public class AddProductProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MadeInCountry { get; set; }

        public decimal Price { get; set; }

        public DateTime Expires { get; set; }

        public string? Ingredients { get; set; }
    }
}