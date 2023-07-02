namespace BioShop.Test.Controller
{
    using Moq;
    using Xunit;
    using BioShop.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.UserModels;

    public class AuthorizationControllerTest
    {

        private readonly Mock<IAuthorizationUserService> _authoUserService;

        public AuthorizationControllerTest() => _authoUserService = new Mock<IAuthorizationUserService>();

        [Fact]
        public async Task RegisterUserReturnOkObjectResult()
        {
            //Arrange
            var userList = await UserData();
            var fakeUser = new UserDTO()
            {
                Username = "Radul",
                Password = "Adminuf",
                Role = "User"
            };
            _authoUserService.Setup(x => x.Register(fakeUser))
                .Callback(() =>
                {
                    userList.Add(fakeUser);
                });
            var authUserController = new AuthorizationController(_authoUserService.Object);

            //Act
            var authUserResult = await authUserController.Register(fakeUser);

            //Assert
            Assert.IsType<OkObjectResult>(authUserResult);
            Assert.Equal(fakeUser, userList[3]);
        }

        [Fact]
        public async Task LoginUserReturnOkObjectResult()
        {
            //Arrange
            var userList = await UserData();

            var fakeUser = new UserDTO()
            {
                Username = "Admin",
                Password = "Admin123",
                Role = "User"
            };
            _authoUserService.Setup(x => x.Login(fakeUser));
            var authUserController = new AuthorizationController(_authoUserService.Object);

            //Act
            var authUserResult = await authUserController.Login(fakeUser);

            //Assert
            Assert.IsType<OkObjectResult>(authUserResult);
        }

        private async Task<List<UserDTO>> UserData()
        {
            List<UserDTO> usersDb = new List<UserDTO>() 
            {
                new UserDTO()
                {
                    Username = "Admin",
                    Password = "Admin123",
                    Role = "User"
                },
                new UserDTO()
                {
                    Username = "Metla",
                    Password = "Metla123",
                    Role = "User"
                },
                new UserDTO()
                {
                    Username = "Tigan",
                    Password = "Tigan123",
                    Role = "Manager"
                },
            };
            return usersDb;
        }
    }
}