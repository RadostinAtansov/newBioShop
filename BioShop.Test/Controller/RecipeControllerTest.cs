namespace BioShop.Test.Controller
{
    using Moq;
    using Xunit;
    using BioShop.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.RecipeModel;
    using AutoMapper;
    using FakeItEasy;

    public class RecipeControllerTest
    {
        private readonly Mock<IRecipeService> _recipeServiceFake;
        private readonly IMapper _mapper;

        public RecipeControllerTest()
        {
            _recipeServiceFake = new Mock<IRecipeService>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task GetRecipesByIdReturnRecipeAndOkResult()
        {
            //Assert
            var recipesList = await GetRecipeData();
            var fakeRecipe = new GetRecipeByIdViewModel()
            {
                Id = 1,
                ProductName = "Cake1",
                Size = 1,
                Portions = 12,
                DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                TimeYouNeedToBeMade = 1.14,
                NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
            };
            _recipeServiceFake.Setup(x => x.GetRecipeById(fakeRecipe.Id))
                .ReturnsAsync(fakeRecipe);
            var recipesController = new Controllers.RecipeController(_recipeServiceFake.Object);

            //Act
            var repiceResult = await recipesController.GetRecipeById(1);
            var recipeRecultModel = ((ObjectResult)repiceResult).Value as GetRecipeByIdViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(repiceResult);
        }

        [Fact]
        public async Task ReturnAllRecipesAndOkObjectResult()
        {
            //Assert
            var recipesList = await GetRecipeData();
            var returnRecipes = _mapper.Map<List<AllRecipesOnProductViewModel>>(recipesList);
            _recipeServiceFake.Setup(x => x.ShowAllRecipes())
                .ReturnsAsync(returnRecipes);
            var recipesController = new Controllers.RecipeController(_recipeServiceFake.Object);

            //Act
            var repiceResult = await recipesController.ShowAllRecipes();
            var reciperRecultModel = ((ObjectResult)repiceResult).Value as List<AllRecipesOnProductViewModel>;

            //Assert
            Assert.IsType<OkObjectResult>(repiceResult);
        }

        [Fact]
        public async Task AddRecipeToProductReturnOkResult()
        {
            //Assert
            var recipesList = await GetRecipeData();
            _recipeServiceFake.Setup(x => x
                .AddRecipeToProduct(recipesList[1].Id, recipesList[0].CurrentProduct.Id));
            var recipesController = new Controllers.RecipeController(_recipeServiceFake.Object);

            //Act
            var repiceResult = await recipesController.AddRecipeToProduct(2, 1);   

            //Assert
            Assert.IsType<OkResult>(repiceResult);
        }

        [Fact]
        public async Task AddRecipeToDatabaseReturnOkResult()
        {
            //Assert
            var recipesList = await GetRecipeData();
            var fakeRecipe = new AddRecipeViewModel()
            {
                Id = 3,
                ProductName = "Cake3",
                Size = 3,
                Portions = 12,
                DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                TimeYouNeedToBeMade = 3.14,
                NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
            };

            _recipeServiceFake.Setup(x => x
                .AddRecipeToDatabase(fakeRecipe));
            var recipesController = new Controllers.RecipeController(_recipeServiceFake.Object);

            //Act
            var repiceResult = await recipesController.AddRecipeToDatabase(fakeRecipe);

            //Assert
            Assert.IsType<OkResult>(repiceResult);
        }

        [Fact]
        public async Task DeleteRecipeToDatabaseReturnOkResult()
        {
            //Assert
            var recipesList = await GetRecipeData();
            _recipeServiceFake.Setup(x => x
                .DeleteRecipe(recipesList[1].Id));
            var recipesController = new Controllers.RecipeController(_recipeServiceFake.Object);

            //Act
            var repiceResult = await recipesController.DeleteRecipeById(recipesList[1].Id);

            //Assert
            Assert.IsType<OkResult>(repiceResult);
        }

        private async Task<List<Recipe>> GetRecipeData()
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
                    CurrentProduct = new Product()
                    {
                        Id = 1,
                        Name = "Cake",
                        Expires = DateTime.UtcNow,
                        Price = 123,
                        Ingredients = "Sugar, Cocoa, Tea Powder",
                        MadeInCountry = "Bg",
                        Recipes = new List<Recipe>(),
                        Clients_Products = new List<ClientProduct>(),
                    }
                    
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
    }
}