namespace BioShop.Data.ViewModels.ProductModels
{
    using BioShop.Data.ViewModels.RecipeModel;

    public class AllRecipesProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MadeInCountry { get; set; }

        public decimal Price { get; set; }

        public DateTime Expires { get; set; }

        public string? Ingredients { get; set; }

        public List<AllRecipesOnProductViewModel> RecipesProduct { get; set; }
    }
}