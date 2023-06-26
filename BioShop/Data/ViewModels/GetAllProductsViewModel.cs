namespace BioShop.Data.ViewModels
{
    public class GetAllProductsViewModel
    {
        public string ProductName { get; set; }

        public string MadeInCountry { get; set; }

        public decimal Price { get; set; }

        public DateTime Expires { get; set; }

        public string? Ingredients { get; set; }

        public int RecipesForThisProduct { get; set; }
    }
}