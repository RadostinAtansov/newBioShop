namespace BioShop.Test.Services
{
    using Moq;
    using Xunit;
    using BioShop.Controllers;
    using BioShop.Data.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;

    public class ClientServiceTest
    {

        private Mock<IClientService> _clientService;

        public ClientServiceTest()
        {
            _clientService = new Mock<IClientService>();
        }

        [Fact]
        public async Task AddClientToDatabaseReturnSameObject()
        {
            //Arrange
            List<ClientViewModel> clientList = await GetClientsData();
            var newFakeClient = new ClientViewModel()
            {
                Id = 4,
                Name = "Ivana",
                Car = "Opela",
                City = "Shumena",
                Money = 123,
                Products = new List<ProductViewModel>(),
            };
            _clientService
                .Setup(x => x.AddClient(newFakeClient))
                .Callback(() => clientList.Add(newFakeClient))
                .ReturnsAsync(newFakeClient);
            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.AddClientToShop(newFakeClient);
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.Equal(clientList[3], newFakeClient);
        }

        [Fact]
        public async Task AddProductToClientShouldReturnCorrectResult()
        {
            //Arrange
            List<ClientViewModel> clientList = await GetClientsData();
            ClientViewModel newFakeClient = new ClientViewModel()
            {
                Id = 3,
                Name = "Ivailo",
                Car = "BMW",
                City = "Montana",
                Money = 10500,
                Products = new List<ProductViewModel>(){},
            };
            ProductViewModel fakeProduct = new ProductViewModel()
            {
                Id = 4,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco, Milk, Eggs1",
                Price = 12,
                MadeInCountry = "Bg1",
                RecipesProduct = new List<RecipeViewModel>(),
            };
            _clientService
                .Setup(x => x.AddProductToClient(clientList[2].Products[0], clientList[2].Id))
                .Callback(() =>
                {
                    var client = clientList.Find(x => x.Id == 3);
                    client.Products.Add(fakeProduct);
                });

            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.AddProductToClient(clientList[2].Products[0], clientList[2].Id);   

            //Assert
            Assert.Equal(clientList[2].Products[3].Id, fakeProduct.Id);
        }

        [Fact]
        public async Task AddProductToClientShouldReturnArgumentNullExceptionIfClientDoesNotExist()
        {
            //Arrange
            List<ClientViewModel> clientList = await GetClientsData();
            int clientId = 0;
            ClientViewModel newFakeClient = new ClientViewModel()
            {
                Id = 3,
                Name = "Ivailo",
                Car = "BMW",
                City = "Montana",
                Money = 10500,
                Products = new List<ProductViewModel>(){new ProductViewModel()
                    {
                        Id = 4,
                        Name = "Torta4",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs1",
                        Price = 12,
                        MadeInCountry = "Bg1",
                        RecipesProduct = new List<RecipeViewModel>(),
                    }
                }
            };
            _clientService.Setup(x => x.AddProductToClient(newFakeClient.Products[0], clientId))
                .Callback(() =>
                {
                    var client = clientList.Find(x => x.Id == clientId);
                    ArgumentNullException.ThrowIfNull(client);
                });

            var clientController = new ClientController(_clientService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.AddProductToClient(newFakeClient.Products[0], clientId));
        }

        [Fact]
        public async Task GetClientByIdFromDatabaseAndReturnClientWithSameId()
        {
            //Arrange
            var clientList = await GetClientsData();
            int clientId = 1;
            _clientService.Setup(x => x.GetClientByIdFromDb(clientId))
                .ReturnsAsync(clientList[0]);
            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.GetClientById(clientId);
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.Equal(clientList[0], clientResultModel);
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
            List<ClientViewModel> clientList = await GetClientsData();
            int clientid = 3;
            int productId = 1;
            ClientViewModel asd = new ClientViewModel()
            {
                Id = 1,
                Name = "Ivan",
                Car = "Opel",
                City = "Shumen",
                Money = 0,
                Products = new List<ProductViewModel>(),
            };
            _clientService.Setup(x => x
            .RemoveProductFromClient(clientid, productId))
                .Callback(() => 
                {
                    var client = clientList.Find(x => x.Id == clientid);
                    var product = client.Products.Find(x => x.Id == productId);
                    client.Products.Remove(product);
                });

            var clientController = new ClientController(_clientService.Object);
            //Act

            var clientResult = await clientController
                .RemoveProductFromClient(clientid, productId);

            //Assert
            Assert.NotEqual(1, clientList[2].Products[0].Id);
        }

        [Fact]
        public async Task ViewAllClientProductsByClientIdIfClientNotExistThrowArgumentNullException()
        {
            //Assert
            List<ClientViewModel> clientList = await GetClientsData();
            int clientid = 0;
            _clientService.Setup(x => x.ViewAllClientProducts(clientid))
                .Callback(() =>
                {
                    var clietnEx = clientList.Find(x => x.Id == clientid);
                    ArgumentNullException.ThrowIfNull(clietnEx);
                });
            var clientController = new ClientController(_clientService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => clientController.ViewAllClientProducts(clientid));

        }

        [Fact]
        public async Task ViewAllClientProductsByClientIdReturnCorrectAllProductClient()
        {
            //Assert
            var clientList = await GetClientsData();
            int clientid = 3;
            _clientService.Setup(x => x.ViewAllClientProducts(clientid))
                .ReturnsAsync(clientList[2]);
            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.ViewAllClientProducts(clientid);
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.True(clientResultModel.Products.Count > 0);

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
