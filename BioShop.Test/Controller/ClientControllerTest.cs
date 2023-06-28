namespace BioShop.Test.Controller
{
    using Moq;
    using Xunit;
    using System;
    using BioShop.Controllers;
    using BioShop.Data.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;

    public class ClientControllerTest
    {
        private readonly Mock<IClientService> _clientServiceFake;

        public ClientControllerTest()
        {
            _clientServiceFake = new Mock<IClientService>();
        }

        [Fact]
        public async Task GetClientByIdAndReturnClientBySpecificId()
        {
            //Arrange
            List<ClientViewModel> clientList = await GetClientsData();
            _clientServiceFake.Setup(x => x
                .GetClientByIdFromDb(1))
                .ReturnsAsync(clientList[0]);
            ClientController clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.GetClientById(1);
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.NotNull(clientResult);
            Assert.IsType<OkObjectResult>(clientResult);
            Assert.True(clientList[0].Id == clientResultModel.Id);
            Assert.Equal(clientList[0].Id, clientResultModel.Id);
        }

        [Fact]
        public async Task ReturnViewAllClientProductsBySpecificClientId()
        {
            //Arrange
            List<ClientViewModel> clientList = await GetClientsData();
            _clientServiceFake.Setup(x => x
                .ViewAllClientProducts(3))
                .ReturnsAsync(clientList[2]);
            ClientController clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.ViewAllClientProducts(3);
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.NotNull(clientResult);
            Assert.IsType<OkObjectResult>(clientResult);
            Assert.True(clientResultModel.Products.Count > 0);
        }

        [Fact]
        public async Task AddClientToDBAndReturnPlusOne()
        {
            //Arrange
            ClientViewModel fakeClient = new ClientViewModel()
            {
                Id = 4,
                Name = "Radul",
                Car = "Honda",
                City = "Shumen",
                Money = 20,
                Products = new List<ProductViewModel>()
            };

            List<ClientViewModel> clientList = await GetClientsData();
            _clientServiceFake.Setup(x => x
                .AddClient(fakeClient))
                .ReturnsAsync(fakeClient);    
             var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.AddClientToShop(fakeClient);           
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(clientResult);
        }

        [Fact]
        public async Task AddClientToDBAndReturnArgumentNullException()
        {
            //Arrange
            ClientViewModel fakeClient = null;
            var clientController = new ClientController(_clientServiceFake.Object);
            
            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.AddClientToShop(fakeClient));
        }

        [Fact]
        public async Task AddProductToClientAndReturnOkResult()
        {
            //Arrange
             List<ClientViewModel> clientList = await GetClientsData();
            int clientId = 3;
            var fakeProduct = new ProductViewModel()
            {
                Id = 1,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco, Milk, Eggs1",
                Price = 12,
                MadeInCountry = "Bg1",
                RecipesProduct = new List<RecipeViewModel>(),

            };
            _clientServiceFake.Setup(x => x
            .AddProductToClient(fakeProduct, clientId));
            var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.AddProductToClient(fakeProduct, clientId);

            //Assert
            Assert.IsType<OkResult>(clientResult);

        }

        [Fact]
        public async Task AddProductToClientAndReturnArgumentNullExceptionIfNull()
        {
            //Arrange
            int clientId = 3;
            List<ClientViewModel> clientList = await GetClientsData();
            var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.AddProductToClient(null, clientId));
        }

        [Fact]
        public async Task RemoveProductFromClientAndReturnOkResult()
        {
            //Arrange
            int clientId = 3;
            var productId = 1;
            List<ClientViewModel> clientList = await GetClientsData();

            _clientServiceFake.Setup(x => x
            .RemoveProductFromClient(clientId, productId));
            var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.RemoveProductFromClient(clientId, productId);

            //Assert
            Assert.IsType<OkResult>(clientResult);
        }

        private async Task<List<ClientViewModel>> GetClientsData()
        {
            List<ClientViewModel> clientsData = new List<ClientViewModel>()
            {
                new ClientViewModel()
                {
                    Id = 1,
                    Name = "Ivan",
                    Car = "Opel",
                    City = "Shumen",
                    Money = 0,
                    Products = new List<ProductViewModel>(),
                },
                 new ClientViewModel()
                {
                    Id = 2,
                    Name = "Nikolai",
                    Car = "Audi",
                    City = "Sofia",
                    Money = 100000,
                    Products = new List<ProductViewModel>(),
                },
                  new ClientViewModel()
                {
                    Id = 3,
                    Name = "Ivailo",
                    Car = "BMW",
                    City = "Montana",
                    Money = 10500, 
                    Products = new List<ProductViewModel>()
                    {
                        new ProductViewModel()
                        {
                            Id = 1,
                            Name = "Torta1",
                            Expires = DateTime.Now.AddDays(31),
                            Ingredients = "Choco, Milk, Eggs1",
                            Price = 12,
                            MadeInCountry = "Bg1",
                            RecipesProduct = new List<RecipeViewModel>(),
                            
                        },
                        new ProductViewModel()
                        {
                            Id = 2,
                            Name = "Torta2",
                            Expires = DateTime.Now.AddDays(31),
                            Ingredients = "Choco, Milk, Eggs2",
                            Price = 12,
                            MadeInCountry = "Bg2",
                            RecipesProduct = new List<RecipeViewModel>(),
                        },
                        new ProductViewModel()
                        {
                            Id = 3,
                            Name = "Torta3",
                            Expires = DateTime.Now.AddDays(31),
                            Ingredients = "Choco, Milk, Eggs3",
                            Price = 12,
                            MadeInCountry = "Bg3",
                            RecipesProduct = new List<RecipeViewModel>(),
                        },
                    },
                },
            };

            return clientsData;
        }
    }
}