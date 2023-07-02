namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels.UserModels;

    public interface IAuthorizationUserService
    {
        Task<UserDTO> Login(UserDTO userRequest);

        Task<UserDTO> Register(UserDTO userRequest);
    }
}