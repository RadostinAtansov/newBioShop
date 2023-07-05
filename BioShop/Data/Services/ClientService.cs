namespace BioShop.Data.Services
{
    using AutoMapper;
    using BioShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.ProductModel;
    using BioShop.Data.ViewModels.ClientModels;
    using BioShop.Data.ViewModels.ProductModels;

    public class ClientService : IClientService
    {
        private readonly BioShopDataContext _dataContext;
        private readonly IMapper _mapper;

        public ClientService(BioShopDataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<AddClientToShopViewModel> AddClient(AddClientToShopViewModel client)
        {
            Client newClient = _mapper.Map<Client>(client);

            await _dataContext.Clients.AddAsync(newClient);
            await _dataContext.SaveChangesAsync();

            var returnAddedClient = _mapper.Map<AddClientToShopViewModel>(newClient);

            return returnAddedClient;
        }

        public async Task AddProductToClient(AddProductViewModel product, int id)
        {
            Product newProduct = _mapper.Map<Product>(product);

            await _dataContext.Products.AddAsync(newProduct);
            await _dataContext.SaveChangesAsync();

            var client = await _dataContext.Clients.FindAsync(id);

            ArgumentNullException.ThrowIfNull(client);

            ClientProduct clientProduct = new ClientProduct()
            {
                ClientId = client.Id,
                ProductId = newProduct.Id
            };

            await _dataContext.ClientProducts.AddAsync(clientProduct);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<GetClientIdViewModel> GetClientByIdFromDb(int id)
        {
            var client = await _dataContext.Clients.FindAsync(id);

            ArgumentNullException.ThrowIfNull(client, "Not Found");

            GetClientIdViewModel newClient = _mapper.Map<GetClientIdViewModel>(client);

            return newClient;
        }

        public async Task RemoveProductFromClient(int clientId, int productId)
        {
            ClientProduct? productClient = await _dataContext.ClientProducts
                .FirstOrDefaultAsync(c => c.ClientId == clientId && c.ProductId == productId);

            ArgumentNullException.ThrowIfNull(productClient);

            _dataContext.ClientProducts.Remove(productClient);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<GetClientAndAllHisProductsViewModel> ViewAllClientProducts(int id)
        {
            Client? client = await _dataContext.Clients.FindAsync(id);

            ArgumentNullException.ThrowIfNull(client);

            GetClientAndAllHisProductsViewModel? clientProducts = await _dataContext.Clients.Where(c => c.Id == client.Id)
                    .Select(c => new GetClientAndAllHisProductsViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        City = c.City,
                        Money = c.Money,
                        Car = c.Car,
                        Products = c.Clients_Products.Where(p => p.ClientId == id).Select(n => new AllRecipesProductViewModel()
                        {
                            Id = n.Product.Id,
                            Name = n.Product.Name,
                            Expires = n.Product.Expires,
                            Price = n.Product.Price,
                            Ingredients = n.Product.Ingredients,
                            MadeInCountry = n.Product.MadeInCountry,
                        }).ToList()
                    }).FirstOrDefaultAsync();

            return clientProducts;
        }        
    }
}