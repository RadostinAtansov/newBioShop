using BioShop.Data.Models;

namespace BioShop.Data.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }

        public string RecipeName { get; set; }

        public int Portions { get; set; }

        public double Size { get; set; }

        public string NecesseryProductsAndQuantity { get; set; }

        public string DesciptionStepByStepHowToBeMade { get; set; }

        public double TimeYouNeedToBeMade { get; set; }

        public string WhitchProductBelongThisRecipe { get; set; }

        public Product Product { get; set; }
    }
}