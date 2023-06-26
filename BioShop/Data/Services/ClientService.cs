namespace BioShop.Data.Services
{
    using BioShop.Data.Models;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class ClientService : IClientService
    {
        private readonly BioShopDataContext _dataContext;

        public ClientService(BioShopDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ClientViewModel> AddClient(ClientViewModel client)
        {

            var newClient = new Client()
            {
                Name = client.Name,
                Car = client.Car,
                City = client.City,
                Money = client.Money,
            };

            await _dataContext.Clients.AddAsync(newClient);
            await _dataContext.SaveChangesAsync();

            var newClient2 = new ClientViewModel()
            {
                Name = newClient.Name,
                Car = newClient.Car,
                City = newClient.City,
                Money = newClient.Money,
            };

            return newClient2;
        } // Yes

        public async Task AddProductToClient(ProductViewModel product, int id)
        {
            var newProduct = new Product()
            {
                Name = product.Name,
                Price = product.Price,
                Expires = product.Expires,
                Ingredients = product.Ingredients,
                MadeInCountry = product.MadeInCountry,
                
            };
            await _dataContext.Products.AddAsync(newProduct);
            await _dataContext.SaveChangesAsync();

            var clientProduct = new ClientProduct()
            {
                ClientId = id,
                ProductId = newProduct.Id
            };

            await _dataContext.ClientProducts.AddAsync(clientProduct);
            await _dataContext.SaveChangesAsync();
        } //Yes

        public async Task<ClientViewModel> GetClintByIdFromDb(int id)
        {
            var client =  await _dataContext.Clients.FindAsync(id);

            ArgumentNullException.ThrowIfNull(client);

            var newClient = new ClientViewModel()
            {
                Name = client.Name,
                Car = client.Car,
                City = client.City,
                Money = client.Money,
            };

            return newClient;
        } // Yes

        public async Task RemoveProductFromClient(int clientId, int productId)
        {
            var productClient = await _dataContext.ClientProducts
                .FirstOrDefaultAsync(c => c.ClientId == clientId && c.ProductId == productId);

            ArgumentNullException.ThrowIfNull(productClient);

            _dataContext.ClientProducts.Remove(productClient);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<ICollection<ClientViewModel>> ViewAllClientProducts(int id)
        {
            var clientProducts = _dataContext.Clients.Where(c => c.Id == id)
                .Select(c => new ClientViewModel()
                {
                    Name = c.Name,
                    City = c.City,
                    Money = c.Money,
                    Car = c.Car,
                    Products = c.Clients_Products.Where(p => p.ClientId == id).Select(n => new ProductViewModel()
                    {
                        Name = n.Product.Name,
                        Expires = n.Product.Expires,
                        Price = n.Product.Price,
                        Ingredients = n.Product.Ingredients,
                        MadeInCountry = n.Product.MadeInCountry,
                    }).ToList()
            }).ToList();

            return clientProducts;
        } // Yes
    }
}