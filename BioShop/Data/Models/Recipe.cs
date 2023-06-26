namespace BioShop.Data.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int Portions { get; set; }

        public double Size { get; set; }

        public string NecesseryProductsAndQuantity { get; set; }

        public string DesciptionStepByStepHowToBeMade { get; set; }

        public double TimeYouNeedToBeMade { get; set; }

        public int? CurrentProductId { get; set; }
        public Product? CurrentProduct { get; set; }
    }
}