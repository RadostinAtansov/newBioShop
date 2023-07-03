namespace BioShop.Test.Services
{
    using Moq;
    using Xunit;
    using AutoMapper;
    using BioShop.Data.Models;
    using BioShop.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.RecipeModel;
    using BioShop.Data.ViewModels.ProductModel;
    using BioShop.Data.ViewModels.ClientModels;
    using BioShop.Data.ViewModels.ProductModels;
    using FakeItEasy;

    public class ClientServiceTest
    {

        private Mock<IClientService> _clientService;
        private readonly IMapper _mapper;

        public ClientServiceTest()
        {
            _clientService = new Mock<IClientService>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task AddClientToDatabaseReturnSameObject()
        {
            //Arrange
            List<Client> clientList = await GetClientsData();
            var fakeClient = new AddClientToShopViewModel()
            {
                Id = 4,
                Name = "Ivana",
                Car = "Opela",
                City = "Shumena",
                Money = 123,
            };

            Client mapFakeClient = _mapper.Map<Client>(fakeClient);

            _clientService
                .Setup(x => x.AddClient(fakeClient))
                .Callback(() => clientList.Add(mapFakeClient))
                .ReturnsAsync(fakeClient);
            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.AddClientToShop(fakeClient);
            var clientResultModel = ((ObjectResult)clientResult).Value as AddClientToShopViewModel;

            //Assert
            Assert.Equal(clientList[3], mapFakeClient);
        }

        [Fact]
        public async Task AddProductToClientShouldReturnCorrectResult()
        {
            //Arrange
            List<Client> clientList = await GetClientsData();
            AddProductViewModel newFakeClient = new AddProductViewModel()
            {
                Id = 3,
                Name = "Ivailo",
                Expires = DateTime.UtcNow,
                Price = 100,
                MadeInCountry = "Bg",
                Ingredients = "Sugar, Cocoa, Coconut"
            };
            ClientProduct fakeProduct = new ClientProduct()
            {
                ClientId = 3,
                Product = new Product()
                {
                    Id = 4,
                    Name = "Torta4",
                    Expires = DateTime.Now.AddDays(31),
                    Ingredients = "Choco, Milk, Eggs1",
                    Price = 12,
                    MadeInCountry = "Bg1",
                }
            };
            _clientService
                .Setup(x => x.AddProductToClient(newFakeClient, clientList[2].Id))
                .Callback(() =>
                {
                    var client = clientList.Find(x => x.Id == 3);
                    client.Clients_Products.Add(fakeProduct);
                });

            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.AddProductToClient(newFakeClient, clientList[2].Id);   

            //Assert
            Assert.Equal(clientList[2].Clients_Products[0].Id, fakeProduct.Id);
        }

        [Fact]
        public async Task AddProductToClientShouldReturnArgumentNullExceptionIfClientDoesNotExist()
        {
            //Arrange
            List<Client> clientList = await GetClientsData();
            int clientId = 0;
            AddProductViewModel newFakeClient = new AddProductViewModel()
            {
                Id = 3,
                Name = "Ivailo",
                Expires = DateTime.UtcNow,
                Price = 100,
                MadeInCountry = "Bg",
                Ingredients = "Sugar, Cocoa, Coconut",
            };
            _clientService.Setup(x => x.AddProductToClient(newFakeClient, clientId))
                .Callback(() =>
                {
                    var client = clientList.Find(x => x.Id == clientId);
                    ArgumentNullException.ThrowIfNull(client);
                });

            var clientController = new ClientController(_clientService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.AddProductToClient(newFakeClient, clientId));
        }

        [Fact]
        public async Task GetClientByIdFromDatabaseAndReturnClientWithSameId()
        {
            //Arrange
            var clientList = await GetClientsData();
            int clientId = 1;
            var fakeClient = clientList[0];
            var clientMap = _mapper.Map<GetClientIdViewModel>(fakeClient);
            _clientService.Setup(x => x.GetClientByIdFromDb(clientId))
                .ReturnsAsync(clientMap);
            var clientController = new ClientController(_clientService.Object);


            //Act
            var clientResult = await clientController.GetClientById(clientId);
            var clientResultModel = ((ObjectResult)clientResult).Value as GetClientIdViewModel;

            //Assert
            Assert.Equal(clientMap, clientResultModel);
        }

        [Fact]
        public async Task GetClientByIdFromDatabaseWrongIdShouldThrowArgumentNullException()
        {
            //Arrange
            var clientList = await GetClientsData();
            _clientService.Setup(x => x.GetClientByIdFromDb(0))
                .Throws<ArgumentNullException>();
            var clientController = new ClientController(_clientService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.GetClientById(0));
        }

        [Fact]
        public async Task RemoveProductFromClient()
        {
            //Arrange
            List<Client> clientList = await GetClientsData();
            int clientid = 3;
            int productId = 1;
            GetClientAndAllHisProductsViewModel asd = new GetClientAndAllHisProductsViewModel()
            {
                Id = 1,
                Name = "Ivan",
                Car = "Opel",
                City = "Shumen",
                Money = 0,
                Products = new List<AllRecipesProductViewModel>(),
            };

            _clientService.Setup(x => x
            .RemoveProductFromClient(clientid, productId))
                .Callback(() => 
                {
                    var client = clientList.Find(x => x.Id == clientid);
                    var product = client.Clients_Products.Find(x => x.Id == productId);
                    client.Clients_Products.Remove(product);
                });

            var clientController = new ClientController(_clientService.Object);
            //Act

            var clientResult = await clientController
                .RemoveProductFromClient(clientid, productId);

            //Assert
            Assert.NotEqual(1, clientList[2].Clients_Products[0].Id);
        }

        [Fact]
        public async Task ViewAllClientProductsByClientIdIfClientNotExistThrowArgumentNullException()
        {
            //Assert
            List<Client> clientList = await GetClientsData();
            int clientId = 0;
            _clientService.Setup(x => x.ViewAllClientProducts(clientId))
                .Callback(() =>
                {
                    var client = clientList.Find(x => x.Id == clientId);
                    ArgumentNullException.ThrowIfNull(client);
                });
            var clientController = new ClientController(_clientService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.ViewAllClientProducts(clientId));
        }

        [Fact]
        public async Task ViewAllClientProductsByClientIdReturnCorrectAllProductClient()
        {
            //Assert
            var clientList = await GetClientsData();
            int clientId = 1;
            var returnFakeClient = new GetClientAndAllHisProductsViewModel()
            {
                Id = 1,
                Name = "Ivan",
                Car = "Opel",
                City = "Shumen",
                Money = 0,
                Products = new List<AllRecipesProductViewModel>()
                {
                    new AllRecipesProductViewModel()
                    {
                           Name = "Ekler"
                    },
                     new AllRecipesProductViewModel()
                    {
                           Name = "Skalichka"
                    },
                }

            };
            var returnClient = _mapper.Map<GetClientAndAllHisProductsViewModel>(clientList[1]);
            _clientService.Setup(x => x.ViewAllClientProducts(clientId))
                .ReturnsAsync(returnFakeClient);
            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.ViewAllClientProducts(clientId);
            var clientResultModel = ((ObjectResult)clientResult).Value as GetClientAndAllHisProductsViewModel;

            //Assert
            Assert.True(returnFakeClient.Products.Count > 0);
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
                    {
                        new ClientProduct()
                        {
                              ClientId = 3,
                              ProductId = 1,
                        }
                    },
                    
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