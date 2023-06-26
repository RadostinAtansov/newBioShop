namespace BioShop.Data.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Money { get; set; }

        public string City { get; set; }

        public string Car { get; set; }

        public virtual List<ClientProduct> Clients_Products { get; set; }
    }
}