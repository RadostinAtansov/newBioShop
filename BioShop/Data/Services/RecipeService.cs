namespace BioShop.Data.Services
{
    using BioShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.RecipeModel;
    using Microsoft.AspNetCore.Mvc;

    public class RecipeService : IRecipeService
    {
        private readonly BioShopDataContext _dataContext;

        public RecipeService(BioShopDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task DeleteRecipe(int id)
        {
            var recipe = await _dataContext.Recipes.FindAsync(id);

            ArgumentNullException.ThrowIfNull(recipe, "This recipe not exist");

             _dataContext.Recipes.Remove(recipe);
            await _dataContext.SaveChangesAsync();
        }
        
        public async Task AddRecipeToProduct(int productId, int recipeId)
        {
            var takeProductById = await _dataContext.Products.FindAsync(productId);
            var takeRecipeById = await _dataContext.Recipes.FindAsync(recipeId);

            ArgumentNullException.ThrowIfNull(takeRecipeById);
            ArgumentNullException.ThrowIfNull(takeProductById);

            takeRecipeById.CurrentProduct = takeProductById;
            takeRecipeById.CurrentProductId = takeProductById.Id;

            _dataContext.SaveChangesAsync();
        }

        public async Task AddRecipeToDatabase([FromBody] AddRecipeViewModel recipe)
        {
            ArgumentNullException.ThrowIfNull(recipe);

            var newRecipe = new Recipe()
            {
                Id = recipe.Id,
                Size = recipe.Size,
                Portions = recipe.Portions,
                ProductName = recipe.ProductName,
                TimeYouNeedToBeMade = recipe.TimeYouNeedToBeMade,
                NecesseryProductsAndQuantity = recipe.NecesseryProductsAndQuantity,
                DesciptionStepByStepHowToBeMade = recipe.DesciptionStepByStepHowToBeMade,
            };

            await _dataContext.Recipes.AddAsync(newRecipe);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<ICollection<AllRecipesOnProductViewModel>> ShowAllRecipes()
        {
            var allRecipes = await _dataContext.Recipes.Select(n => new AllRecipesOnProductViewModel()
            {
                Id = n.Id,
                ProductName = n.ProductName,
                Size = n.Size,
                Portions = n.Portions,
                DesciptionStepByStepHowToBeMade = n.DesciptionStepByStepHowToBeMade,
                TimeYouNeedToBeMade = n.TimeYouNeedToBeMade,
                NecesseryProductsAndQuantity = n.NecesseryProductsAndQuantity,
                //WhitchProductBelongThisRecipe = n.CurrentProduct.Name,
            }).ToListAsync();

            return allRecipes;
        }

        public async Task<GetRecipeByIdViewModel> GetRecipeById(int id)
        {
            var recipe = await _dataContext.Recipes.Where(i => i.Id == id).Select(n => new GetRecipeByIdViewModel()
            {
                Id = n.Id,
                Size = n.Size,
                Portions = n.Portions,
                ProductName = n.ProductName,
                TimeYouNeedToBeMade = n.TimeYouNeedToBeMade,
                NecesseryProductsAndQuantity = n.NecesseryProductsAndQuantity,
                DesciptionStepByStepHowToBeMade = n.DesciptionStepByStepHowToBeMade,
                WhitchProductBelongThisRecipe = n.CurrentProduct.Name,
            }).FirstOrDefaultAsync();

            ArgumentNullException.ThrowIfNull(recipe);

            return recipe;
        }
    }
}