namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IClientService
    {
        Task<ClientViewModel> GetClientByIdFromDb(int id);

        Task<ClientViewModel> ViewAllClientProducts(int id);

        Task<ClientViewModel> AddClient(ClientViewModel client);

        Task RemoveProductFromClient(int clientId, int productId);

        Task AddProductToClient(ProductViewModel product, int id);
    }
}