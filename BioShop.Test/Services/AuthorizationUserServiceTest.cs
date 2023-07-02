namespace BioShop.Test.Services
{
    using Moq;
    using Xunit;
    using BioShop.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.UserModels;

    public class AuthorizationUserServiceTest
    {
        private readonly Mock<IAuthorizationUserService> _authUserService;

        public AuthorizationUserServiceTest() => _authUserService = new Mock<IAuthorizationUserService>();

        [Fact]
        public async Task RegisterUserAddUserToDBAndReturnRegisteredUser()
        {
            //Arrange
            var userList = await UserData();
            var fakeUser = new UserDTO()
            {
                Username = "Metla",
                Password = "Metla123",
                Role = "User"
            };
            int userListCoutBeforRegister = userList.Count;
            _authUserService.Setup(x => x.Register(fakeUser))
                .Callback(() =>
                {
                    userList.Add(fakeUser);
                });
            var authUserController = new AuthorizationController(_authUserService.Object);

            //Act
            var userResult = await authUserController.Register(fakeUser);
            var userResultModel = ((ObjectResult)userResult).Value as UserDTO;

            //Assert

            Assert.NotEqual(userListCoutBeforRegister, userList.Count);
            Assert.Equal(userListCoutBeforRegister + 1, userList.Count);
        }

        [Fact]
        public async Task LoginToDBIfUserDoesNotExistThrowArgumentNullException()
        {
            //Arrange
            var userList = await UserData();
            var fakeUser = new UserDTO()
            {
                Username = "Shushumir",
                Password = "Shushushu123",
                Role = "User"
            };
            _authUserService.Setup(x => x.Login(fakeUser))
                .Callback(() =>
                {
                    var user = userList.Find(u => u.Username == fakeUser.Username);
                    ArgumentNullException.ThrowIfNull(user);
                });
            var userController = new AuthorizationController( _authUserService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => userController.Login(fakeUser));
        }

        [Fact]
        public async Task LoginToDBWithWrongPasswordReturnArgumentExceptionWrongPassword()
        {
            //Arrange
            var userList = await UserData();
            var fakeUser = new UserDTO()
            {
                Username = "Admin",
                Password = "321nimdA",
                Role = "User"
            };
            _authUserService.Setup(x => x.Login(fakeUser))
                .Callback(() =>
                {
                    var user = userList.Find(u => u.Username == fakeUser.Username);
                    if (user.Password != fakeUser.Password)
                    {
                        throw new ArgumentException();
                    }
                });       
            var userController = new AuthorizationController(_authUserService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() => userController.Login(fakeUser));
        }

        [Fact]
        public async Task LoginToDBWithCorrectUserAndReturnUser()
        {
            //Arrange
            var userList = await UserData();
            var fakeUser = new UserDTO()
            {
                Username = "Admin",
                Password = "Admin123",
                Role = "User"
            };
            _authUserService.Setup(x => x.Login(fakeUser))
                .ReturnsAsync(userList[0]);
            var userController = new AuthorizationController(_authUserService.Object);

            //Act
            var userResult = await userController.Login(fakeUser);
            var userResultModel = ((ObjectResult)userResult).Value as UserDTO;

            //Assert
            Assert.Equal(userList[0], userResultModel);
            Assert.True(userList[0].Role == userResultModel.Role);
            Assert.True(userList[0].Username == userResultModel.Username);
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