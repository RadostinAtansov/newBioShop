namespace BioShop.Test.Controller
{
    using Xunit;
    using BioShop.Controllers;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;

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
                .GetClintByIdFromDb(1))
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
                .ReturnsAsync(clientList);
            ClientController clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.ViewAllClientProducts(3);
            var clientResultModel = ((ObjectResult)clientResult).Value as List<ClientViewModel>;

            //Assert
            Assert.NotNull(clientResult);
            Assert.IsType<OkObjectResult>(clientResult);
            Assert.True(clientResultModel[2].Products.Count > 0);
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
            Assert.NotNull(clientResult);
            Assert.Equal(clientResultModel.Id, fakeClient.Id);
            Assert.True(clientResultModel.Id == fakeClient.Id);
        }

        [Fact]
        public async Task AddProductToClientAndReturnOkResult()
        {
            //Arrange
             List<ClientViewModel> clientList = await GetClientsData();
            _clientServiceFake.Setup(x => x
            .AddProductToClient(clientList[2].Products[0], clientList[2].Id));
            var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.AddProductToClient(clientList[2].Products[0], clientList[2].Id);

            //Assert
            Assert.IsType<OkResult>(clientResult);

        }

        [Fact]
        public async Task RemoveProductFromClientAndReturnOkResult()
        {
            //Arrange
            List<ClientViewModel> clientList = await GetClientsData();
            var product = clientList[2].Products[0].Id;

            _clientServiceFake.Setup(x => x
            .RemoveProductFromClient(product, clientList[2].Id));
            var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.RemoveProductFromClient(product, clientList[2].Id);

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