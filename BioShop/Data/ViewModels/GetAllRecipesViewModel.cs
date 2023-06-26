namespace BioShop.Data.ViewModels
{
    public class GetAllRecipesViewModel
    {
        public string RecipeName { get; set; }

        public int Portions { get; set; }

        public double Size { get; set; }

        public string NecesseryProductsAndQuantity { get; set; }

        public string DesciptionStepByStepHowToBeMade { get; set; }

        public double TimeYouNeedToBeMade { get; set; }

        public string WhitchProductBelongThisRecipe{ get; set; }
    }
}
