namespace BioShop.Test.Controller
{
    using Moq;
    using Xunit;
    using BioShop.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.RecipeModel;

    public class RecipeControllerTest
    {
        private readonly Mock<IRecipeService> _recipeServiceFake;

        public RecipeControllerTest() => _recipeServiceFake = new Mock<IRecipeService>();

        [Fact]
        public async Task GetRecipesByIdReturnRecipeAndOkResult()
        {
            //Assert
            var recipesList = await GetRecipeData();
            _recipeServiceFake.Setup(x => x.GetRecipeById(1))
                .ReturnsAsync(recipesList[0]);
            var recipesController = new Controllers.RecipeController(_recipeServiceFake.Object);

            //Act
            var repiceResult = await recipesController.GetRecipeById(1);
            var reciperRecultModel = ((ObjectResult)repiceResult).Value as AllRecipesOnProductViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(repiceResult);
        }

        [Fact]
        public async Task ReturnAllRecipesAndOkObjectResult()
        {
            //Assert
            var recipesList = await GetRecipeData();
            _recipeServiceFake.Setup(x => x.ShowAllRecipes())
                .ReturnsAsync(recipesList);
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
                .AddRecipeToProduct(recipesList[1].Id, recipesList[1]
                .Product.Id));
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
            _recipeServiceFake.Setup(x => x
                .AddRecipeToDatabase(recipesList[1]));
            var recipesController = new Controllers.RecipeController(_recipeServiceFake.Object);

            //Act
            var repiceResult = await recipesController.AddRecipeToDatabase(recipesList[1]);

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

        private async Task<List<AllRecipesOnProductViewModel>> GetRecipeData()
        {
            List<AllRecipesOnProductViewModel> recipesList = new List<AllRecipesOnProductViewModel>()
            {
                new AllRecipesOnProductViewModel()
                {
                    Id = 1,
                    RecipeName = "Cake1",
                    Size = 1,
                    Portions = 12,
                    DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                    TimeYouNeedToBeMade = 1.14,
                    NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
                    WhitchProductBelongThisRecipe = "Chocolate Cake",
                    
                },
                new AllRecipesOnProductViewModel()
                {
                    Id = 2,
                    RecipeName = "Cake2",
                    Size = 2,
                    Portions = 12,
                    DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                    TimeYouNeedToBeMade = 2.14,
                    NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
                    WhitchProductBelongThisRecipe = "Chocolate Cake",
                    Product = new Product()
                        { 
                            Id = 123,
                            Name = "Torta"
                        }
                },
                new AllRecipesOnProductViewModel()
                {
                    Id = 3,
                    RecipeName = "Cake3",
                    Size = 3,
                    Portions = 12,
                    DesciptionStepByStepHowToBeMade = "1.Chocolate, 2.Milk, 3.Sugar",
                    TimeYouNeedToBeMade = 3.14,
                    NecesseryProductsAndQuantity = "2 eggs, 1 yogurt, 1kg sugar",
                    WhitchProductBelongThisRecipe = "Chocolate Cake",
                },
            };
            return recipesList;
        }
    }
}