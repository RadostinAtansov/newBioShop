namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels.ClientModels;
    using BioShop.Data.ViewModels.ProductModel;
    using BioShop.Data.ViewModels.ProductModels;

    public interface IClientService
    {
        Task<GetClienttyIdViewModel> GetClientByIdFromDb(int id);

        Task<ClientViewModel> ViewAllClientProducts(int id);

        Task<AddClientToShopViewModel> AddClient(AddClientToShopViewModel client);

        Task RemoveProductFromClient(int clientId, int productId);

        Task AddProductToClient(AddProductProductViewModel product, int id);
    }
}