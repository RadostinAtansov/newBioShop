using BioShop.Data.Models;

namespace BioShop.Data.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Money { get; set; }

        public string City { get; set; }

        public string Car { get; set; }

        public List<ProductViewModel> Products { get; set; }
    }
}