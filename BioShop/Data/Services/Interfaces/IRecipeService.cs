namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IRecipeService
    {
        Task DeleteRecipe(int id);

        Task<RecipeViewModel> GetRecipeById(int id);

        Task AddRecipeToDatabase(RecipeViewModel recipe);

        Task<ICollection<RecipeViewModel>> ShowAllRecipes();

        Task AddRecipeToProduct(int productId, int recipeId);
    }
}