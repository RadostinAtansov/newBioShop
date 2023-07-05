namespace BioShop.Test.Controller
{
    using Moq;
    using Xunit;
    using System;
    using AutoMapper;
    using BioShop.Data.Models;
    using BioShop.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.ProductModel;
    using BioShop.Data.ViewModels.ProductModels;
    using BioShop.Data.ViewModels.ClientModels;

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
            List<Client> clientList = await GetClientsData();
            var clientReturn = new GetClientIdViewModel()
            {
                Name = "Ivan",
                Car = "Opel",
                City = "Shumen",
                Money = 0,
            };
            _clientServiceFake.Setup(x => x
                .GetClientByIdFromDb(1))
                .ReturnsAsync(clientReturn);
            ClientController clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.GetClientById(1);
            var clientResultModel = ((ObjectResult)clientResult).Value as GetClientAndAllHisProductsViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(clientResult);
        }

        [Fact]
        public async Task ReturnViewAllClientProductsBySpecificClientId()
        {
            //Arrange
            List<Client> clientList = await GetClientsData();
            var clientReturn = new GetClientAndAllHisProductsViewModel()
            {
                 Name = "Ivan",
                 Car = "Car",
                 City = "Silistra",
                 Money = 0,
                 Products = new List<AllRecipesProductViewModel>() 
                 {
                     new AllRecipesProductViewModel()
                     {
                            Id = 1,
                            Name = "Product",
                            Expires = DateTime.Now,
                            Price = 0,
                            Ingredients = "1,2,3,4",
                            MadeInCountry = "BG",
                     }
                 }
            };

            _clientServiceFake.Setup(x => x
                .ViewAllClientProducts(3))
                .ReturnsAsync(clientReturn);

            ClientController clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.ViewAllClientProducts(3);
            var clientResultModel = ((ObjectResult)clientResult).Value as GetClientAndAllHisProductsViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(clientResult);
        }

        [Fact]
        public async Task AddClientToDBAndReturnOk()
        {
            //Arrange
            AddClientToShopViewModel fakeClient = new AddClientToShopViewModel()
            {
                Id = 4,
                Name = "Radul",
                Car = "Honda",
                City = "Shumen",
                Money = 20,
            };


            List<Client> clientList = await GetClientsData();
            _clientServiceFake.Setup(x => x
                .AddClient(fakeClient))
                .ReturnsAsync(fakeClient);
            var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.AddClientToShop(fakeClient);
            var clientResultModel = ((ObjectResult)clientResult).Value as AddClientToShopViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(clientResult);
        }

        [Fact]
        public async Task AddClientToDBAndReturnArgumentNullException()
        {
            //Arrange
            AddClientToShopViewModel fakeClient = null;
            var clientController = new ClientController(_clientServiceFake.Object);
            
            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.AddClientToShop(fakeClient));
        }

        [Fact]
        public async Task AddProductToClientAndReturnOkResult()
        {
            //Arrange
             List<Client> clientList = await GetClientsData();
            int clientId = 3;
            var fakeProduct = new AddProductViewModel()
            {
                Id = 1,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco, Milk, Eggs1",
                Price = 12,
                MadeInCountry = "Bg1",
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
            List<Client> clientList = await GetClientsData();
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
            List<Client> clientList = await GetClientsData();

            _clientServiceFake.Setup(x => x
            .RemoveProductFromClient(clientId, productId));
            var clientController = new ClientController(_clientServiceFake.Object);

            //Act
            var clientResult = await clientController.RemoveProductFromClient(clientId, productId);

            //Assert
            Assert.IsType<OkResult>(clientResult);
        }

        private async Task<List<Client>> GetClientsData()
        {
            List<Client> clientsData = new List<Client>()
            {
                new Client()
                {
                    Id = 1,
                    Name = "Ivan",
                    Car = "Opel",
                    City = "Shumen",
                    Money = 0,
                    Clients_Products = new List<ClientProduct>()
                },
                new Client()
                {
                    Id = 2,
                    Name = "Nikolai",
                    Car = "Audi",
                    City = "Sofia",
                    Money = 100000,
                    Clients_Products = new List<ClientProduct>()
                },
                new Client()
                {
                    Id = 3,
                    Name = "Ivailo",
                    Car = "BMW",
                    City = "Montana",
                    Money = 10500,
                    Clients_Products = new List<ClientProduct>()
                    {
                        new ClientProduct()
                        {
                              ClientId = 3,
                              ProductId = 1,
                        }
                    }
                },
            };
            return clientsData;
        }
    }
}