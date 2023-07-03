namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels.ClientModels;
    using BioShop.Data.ViewModels.ProductModel;
    using BioShop.Data.ViewModels.ProductModels;

    public interface IClientService
    {
        Task<GetClientIdViewModel> GetClientByIdFromDb(int id);

        Task<GetClientAndAllHisProductsViewModel> ViewAllClientProducts(int id);

        Task<AddClientToShopViewModel> AddClient(AddClientToShopViewModel client);

        Task RemoveProductFromClient(int clientId, int productId);

        Task AddProductToClient(AddProductViewModel product, int id);
    }
}