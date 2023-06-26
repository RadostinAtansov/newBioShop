namespace BioShop.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MadeInCountry { get; set; }

        public decimal Price { get; set; }

        public DateTime Expires { get; set; }

        public string? Ingredients { get; set; }

        public virtual List<Recipe> Recipes { get; set; }
        public virtual List<ClientProduct> Clients_Products { get; set; }
    }
}