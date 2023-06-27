namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IClientService
    {
        Task AddProductToClient(ProductViewModel product, int id);

        Task RemoveProductFromClient(int clientId, int productId);

        Task<ClientViewModel> ViewAllClientProducts(int id);

        Task<ClientViewModel> AddClient(ClientViewModel client);

        Task<ClientViewModel> GetClientByIdFromDb(int id);
    }
}
