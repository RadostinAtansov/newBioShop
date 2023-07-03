namespace BioShop.Test.Services
{
    using Moq;
    using Xunit;
    using AutoMapper;
    using BioShop.Controllers;
    using BioShop.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.RecipeModel;
    using FakeItEasy;

    public class RecipeServiceTest
    {
        private readonly Mock<IRecipeService> _recipeService;
        private readonly IMapper _mapper;

        public RecipeServiceTest()
        {
            _recipeService = new Mock<IRecipeService>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task GetRecipeByIdIfDoesNotExistThrowArgumentNullException()
        {
            //Arrange
            var recipeList = await RecipeData();
            int recipeId = 0;
            _recipeService.Setup(x => x.GetRecipeById(recipeId))
                .Callback(() =>
                {
                    var recipe = recipeList.Find(f => f.Id == recipeId);
                    ArgumentNullException.ThrowIfNull(recipe);
                });
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => recipeController.GetRecipeById(recipeId));
        }

        [Fact]
        public async Task GetRecipeByIdIfExistReturnCorrectResult()
        {
            //Arrange
            var recipeList = await RecipeData();
            int recipeId = 1;
            var fakeRecipeViewModel = new GetRecipeByIdViewModel()
            {
                Id = 1,
                ProductName = "Cake1",
                Size = 1,
                Portions = 12,
                DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                TimeYouNeedToBeMade = 1.14,
                NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
            };

            _recipeService.Setup(x => x.GetRecipeById(recipeId))
                          .ReturnsAsync(fakeRecipeViewModel);
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            var recipeResult = await recipeController.GetRecipeById(recipeId);
            var recipeResultModel = ((ObjectResult)recipeResult).Value as GetRecipeByIdViewModel;

            //Assert
            Assert.Equal(fakeRecipeViewModel, recipeResultModel);         
        }

        [Fact]
        public async Task ShowAllRecipesReturnCorrectResultAllRecipes()
        {
            //Arrange
            var recipeList = await RecipeData();
            var fakeRecipeViewModel = _mapper.Map<List<AllRecipesOnProductViewModel>>(recipeList);

            _recipeService.Setup(x => x.ShowAllRecipes())
                .ReturnsAsync(fakeRecipeViewModel);
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            var recipeResult = await recipeController.ShowAllRecipes();
            var recipeResultModel = ((ObjectResult)recipeResult).Value as List<AllRecipesOnProductViewModel>;

            //Assert
            Assert.Equal(fakeRecipeViewModel, recipeResultModel);
        }

        [Fact]
        public async Task AddRecipeToDbIfRecipeIsNullThrowArgumentNullException()
        {
            //Arrange
            var recipeList = await RecipeData();
            AddRecipeViewModel recipe = null;
            _recipeService.Setup(x => x.AddRecipeToDatabase(recipe))
                .Throws<ArgumentNullException>();
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => recipeController.AddRecipeToDatabase(recipe));
        }

        [Fact]
        public async Task AddRecipeToDbAndCheckItIfIsThere()
        {
            //Arrange
            var recipeList = await RecipeData();
            AddRecipeViewModel fakeRecipeViewModel = new AddRecipeViewModel()
            {
                Id = 12345678,
                ProductName = "Cake12345678",
                Size = 12345678,
                Portions = 12345678,
                DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar, 12345678",
                TimeYouNeedToBeMade = 12345678,
                NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar, 12345678",
            };
            Recipe fakeRecipe = new Recipe()
            {
                Id = 12345678,
                ProductName = "Cake12345678",
                Size = 12345678,
                Portions = 12345678,
                DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar, 12345678",
                TimeYouNeedToBeMade = 12345678,
                NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar, 12345678",
            };
            _recipeService.Setup(x => x.AddRecipeToDatabase(fakeRecipeViewModel))
                .Callback(() =>
                {
                    recipeList.Add(fakeRecipe);
                });
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            var recipeResult = await recipeController.AddRecipeToDatabase(fakeRecipeViewModel);           

            //Assert
            Assert.Equal(fakeRecipe, recipeList[3]);
            Assert.Equal(fakeRecipe.Id, recipeList[3].Id);
        }

        [Fact]
        public async Task AddRecipeToProductIfProductIsIsNullThrowArgumentNullException()
        {
            //Arrange
            var recipeLst = await RecipeData();
            var productList = await ProductData();
            int productId = 0;
            int recipeId = 1;

            _recipeService.Setup(x => x.AddRecipeToProduct(productId, recipeId))
                .Callback(() =>
                {
                    var product = productList.Find(p => p.Id == productId);
                    ArgumentNullException.ThrowIfNull(product);
                });
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            //Result
            await Assert.ThrowsAsync<ArgumentNullException>(() => recipeController.AddRecipeToProduct(productId, recipeId));
        }

        [Fact]
        public async Task AddRecipeToProductIfRecipeIsIsNullThrowArgumentNullException()
        {
            //Arrange
            var recipeList = await RecipeData();
            var productList = await ProductData();
            int productId = 1;
            int recipeId = 0;

            _recipeService.Setup(x => x.AddRecipeToProduct(productId, recipeId))
                .Callback(() =>
                {
                    var recipe = recipeList.Find(p => p.Id == recipeId);
                    ArgumentNullException.ThrowIfNull(recipe);
                });
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            //Result
            await Assert.ThrowsAsync<ArgumentNullException>(() => recipeController.AddRecipeToProduct(productId, recipeId));
        }

        [Fact]
        public async Task AddRecipeToProductIfRecipeAndProductIsNotNull()
        {
            //Arrange
            var recipeList = await RecipeData();
            var productList = await ProductData();
            int productId = 1;
            int recipeId = 1;

            _recipeService.Setup(x => x.AddRecipeToProduct(productId, recipeId))
                .Callback(() =>
                {
                    var recipe = recipeList.Find(r => r.Id == recipeId);
                    var product = productList.Find(p => p.Id == productId);
                    product.Recipes.Add(recipe);
                });
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            await recipeController.AddRecipeToProduct(productId, recipeId);

            //Result
            Assert.Equal(productList[0].Id, productId);
            Assert.True(productList[0].Recipes.Count > 0);
            Assert.Equal(productList[0].Recipes[0].Id, recipeId);

        }

        [Fact]
        public async Task DeleteRecipeIfNotExistThrowArgumentNullException()
        {
            //Arrange
            var recipeList = await RecipeData();
            int recipeId = 0;
            _recipeService.Setup(x => x.DeleteRecipe(recipeId))
                .Callback(() => 
                {
                    var recipe = recipeList.Find(r => r.Id == recipeId);
                    ArgumentNullException.ThrowIfNull(recipe, "This recipe not exist");
                });
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>("This recipe not exist", () => recipeController.DeleteRecipeById(recipeId));
        }

        [Fact]
        public async Task DeleteRecipeIfNotNull()
        {
            //Arrange
            var recipeList = await RecipeData();
            int recipeId = 1;
            int recipeListCountBeforDelete = recipeList.Count;
            _recipeService.Setup(x => x.DeleteRecipe(recipeId))
                .Callback(() =>
                {
                    var recipe = recipeList.Find(r => r.Id == recipeId);
                    recipeList.Remove(recipe);
                });
            var recipeController = new RecipeController(_recipeService.Object);

            //Act
            await recipeController.DeleteRecipeById(recipeId);

            //Assert
            Assert.NotEqual(recipeListCountBeforDelete, recipeList.Count);
        }

        private async Task<List<Recipe>> RecipeData()
        {
            List<Recipe> recipesList = new List<Recipe>()
            {
                new Recipe()
                {
                    Id = 1,
                    ProductName = "Cake1",
                    Size = 1,
                    Portions = 12,
                    DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                    TimeYouNeedToBeMade = 1.14,
                    NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
                    
                },
                new Recipe()
                {
                    Id = 2,
                    ProductName = "Cake2",
                    Size = 2,
                    Portions = 12,
                    DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                    TimeYouNeedToBeMade = 2.14,
                    NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
                },
                new Recipe()
                {
                    Id = 3,
                    ProductName = "Cake3",
                    Size = 3,
                    Portions = 12,
                    DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                    TimeYouNeedToBeMade = 3.14,
                    NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
                },
            };
            return recipesList;
        }

        private async Task<List<Product>> ProductData()
        {
            List<Product> productList = new List<Product>()
                {
                    new Product()
                    {
                        Id = 1,
                        Name = "Torta1",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs1",
                        Price = 12,
                        MadeInCountry = "Bg1",
                        Recipes = new List<Recipe>(){}

                    },
                    new Product()
                    {
                        Id = 2,
                        Name = "Torta2",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs2",
                        Price = 12,
                        MadeInCountry = "Bg2",
                        Recipes = new List<Recipe>(),
                    },
                    new Product()
                    {
                        Id = 3,
                        Name = "Torta3",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs3",
                        Price = 12,
                        MadeInCountry = "Bg3",
                        Recipes = new List<Recipe>()
                        {
                            new Recipe()
                            {
                                Id = 1,
                                ProductName = "IceCream"
                            },
                            new Recipe()
                            {
                                Id = 2,
                                ProductName = "Cake"
                            },
                            new Recipe()
                            {
                                Id = 3,
                                ProductName = "Chocolate"
                            },
                        },
                    },
                };
            return productList;
        }
    }
}