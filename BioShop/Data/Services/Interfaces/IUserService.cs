
namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IUserService
    {
        Task<UserDTO> Register(UserDTO userRequest);

        Task<string> Login(UserDTO userRequest);
    }
}