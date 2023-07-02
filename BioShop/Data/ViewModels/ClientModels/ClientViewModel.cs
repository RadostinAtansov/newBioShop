namespace BioShop.Data.ViewModels.ProductModel
{

    using BioShop.Data.ViewModels.ProductModels;

    public class ClientViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Money { get; set; }

        public string City { get; set; }

        public string Car { get; set; }

        public List<AllRecipesProductViewModel> Products { get; set; }
    }
}