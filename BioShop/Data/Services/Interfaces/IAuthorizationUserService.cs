namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels.UserModels;

    public interface IAuthorizationUserService
    {
        Task<UserDTOLogin> Login(UserDTOLogin userRequest);

        Task<UserDTORegister> Register(UserDTORegister userRequest);
    }
}