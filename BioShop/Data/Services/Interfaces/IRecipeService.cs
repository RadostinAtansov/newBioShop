namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels.RecipeModel;

    public interface IRecipeService
    {
        Task DeleteRecipe(int id);

        Task<AllRecipesOnProductViewModel> GetRecipeById(int id);

        Task AddRecipeToDatabase(AddRecipeViewModel recipe);

        Task<ICollection<AllRecipesOnProductViewModel>> ShowAllRecipes();

        Task AddRecipeToProduct(int productId, int recipeId);
    }
}