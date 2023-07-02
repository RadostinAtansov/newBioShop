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

    public class ClientServiceTest
    {

        private Mock<IClientService> _clientService;
        private readonly IMapper _mapper;

        public ClientServiceTest(IMapper mapper)
        {
            _clientService = new Mock<IClientService>();
            _mapper = mapper;
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
                //Products = new List<ProductViewModel>(),
            };

            Client mapFakeClient = _mapper.Map<Client>(fakeClient);

            _clientService
                .Setup(x => x.AddClient(fakeClient))
                .Callback(() => clientList.Add(mapFakeClient))
                .ReturnsAsync(fakeClient);
            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.AddClientToShop(fakeClient);
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.Equal(clientList[3], mapFakeClient);
        }

        [Fact]
        public async Task AddProductToClientShouldReturnCorrectResult()
        {
            //Arrange
            List<Client> clientList = await GetClientsData();
            Client newFakeClient = new Client()
            {
                Id = 3,
                Name = "Ivailo",
                Car = "BMW",
                City = "Montana",
                Money = 10500,
                Clients_Products = new List<ClientProduct>()
                /*Products = new List<ProductViewModel>(){}*/,
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
                .Setup(x => x.AddProductToClient(fakeProduct, clientList[2].Id))
                .Callback(() =>
                {
                    var client = clientList.Find(x => x.Id == 3);
                    client.Clients_Products.Add(fakeProduct);
                });

            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.AddProductToClient(fakeProduct, clientList[2].Id);   

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
                Products = new List<AllRecipesProductViewModel>(){new AllRecipesProductViewModel()
                    {
                        Id = 4,
                        Name = "Torta4",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs1",
                        Price = 12,
                        MadeInCountry = "Bg1",
                        RecipesProduct = new List<AllRecipesOnProductViewModel>(),
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
                Products = new List<AllRecipesProductViewModel>(),
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
            int clientId = 3;
            _clientService.Setup(x => x.ViewAllClientProducts(clientId))
                .ReturnsAsync(clientList[2]);
            var clientController = new ClientController(_clientService.Object);

            //Act
            var clientResult = await clientController.ViewAllClientProducts(clientId);
            var clientResultModel = ((ObjectResult)clientResult).Value as ClientViewModel;

            //Assert
            Assert.True(clientResultModel.Products.Count > 0);
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
                    //Products = new List<ProductViewModel>(),
                },
                new Client()
                {
                    Id = 2,
                    Name = "Nikolai",
                    Car = "Audi",
                    City = "Sofia",
                    Money = 100000,
                    Clients_Products = new List<ClientProduct>()
                    //Products = new List<ProductViewModel>(),
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
                        //Products = new List<ProductViewModel>()
                        //{
                        //    new ProductViewModel()
                        //    {
                        //        Id = 1,
                        //        Name = "Torta1",
                        //        Expires = DateTime.Now.AddDays(31),
                        //        Ingredients = "Choco, Milk, Eggs1",
                        //        Price = 12,
                        //        MadeInCountry = "Bg1",
                        //        RecipesProduct = new List<RecipeViewModel>(),

                        //    },
                        //    new ProductViewModel()
                        //    {
                        //        Id = 2,
                        //        Name = "Torta2",
                        //        Expires = DateTime.Now.AddDays(31),
                        //        Ingredients = "Choco, Milk, Eggs2",
                        //        Price = 12,
                        //        MadeInCountry = "Bg2",
                        //        RecipesProduct = new List<RecipeViewModel>(),
                        //    },
                        //    new ProductViewModel()
                        //    {
                        //        Id = 3,
                        //        Name = "Torta3",
                        //        Expires = DateTime.Now.AddDays(31),
                        //        Ingredients = "Choco, Milk, Eggs3",
                        //        Price = 12,
                        //        MadeInCountry = "Bg3",
                        //        RecipesProduct = new List<RecipeViewModel>(),
                        //    },
                        //},
                },
            };
                return clientsData;
        }
    }
}