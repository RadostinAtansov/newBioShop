
namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IClientService
    {
        Task AddProductToClient(ProductViewModel product, int id);

        Task RemoveProductFromClient(int clientId, int productId);

        Task<ICollection<ClientViewModel>> ViewAllClientProducts(int id);

        Task<ClientViewModel> AddClient(ClientViewModel client);

        Task<ClientViewModel> GetClintByIdFromDb(int id);
    }
}
