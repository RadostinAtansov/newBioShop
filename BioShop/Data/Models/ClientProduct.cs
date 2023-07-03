namespace BioShop.Data.Models
{
    public class ClientProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}