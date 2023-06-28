namespace BioShop.Data.Services
{
    using BioShop.Data.Models;
    using BioShop.Data.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using BioShop.Data.Services.Interfaces;

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

            this._dataContext.SaveChangesAsync();
        }

        public async Task AddRecipeToDatabase(RecipeViewModel recipe)
        {

            ArgumentNullException.ThrowIfNull(recipe);

            var newRecipe = new Recipe()
            {
                Size = recipe.Size,
                Portions = recipe.Portions,
                ProductName = recipe.RecipeName,
                TimeYouNeedToBeMade = recipe.TimeYouNeedToBeMade,
                NecesseryProductsAndQuantity = recipe.NecesseryProductsAndQuantity,
                DesciptionStepByStepHowToBeMade = recipe.DesciptionStepByStepHowToBeMade,
                
            };

            await _dataContext.Recipes.AddAsync(newRecipe);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<ICollection<RecipeViewModel>> ShowAllRecipes()
        {
            var allRecipes = await _dataContext.Recipes.Select(n => new RecipeViewModel()
            {
                RecipeName = n.ProductName,
                Size = n.Size,
                Portions = n.Portions,
                DesciptionStepByStepHowToBeMade = n.DesciptionStepByStepHowToBeMade,
                TimeYouNeedToBeMade = n.TimeYouNeedToBeMade,
                NecesseryProductsAndQuantity = n.NecesseryProductsAndQuantity,
                WhitchProductBelongThisRecipe = n.CurrentProduct.Name,
            }).ToListAsync();

            return allRecipes;
        }

        public async Task<RecipeViewModel> GetRecipeById(int id)
        {
            var recipe = await _dataContext.Recipes.Where(i => i.Id == id).Select(n => new RecipeViewModel()
            {
                Size = n.Size,
                Portions = n.Portions,
                RecipeName = n.ProductName,
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