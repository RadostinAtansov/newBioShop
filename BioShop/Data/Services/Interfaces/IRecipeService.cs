
namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IRecipeService
    {
        Task AddRecipeToProduct(int productId, int recipeId);

        Task AddRecipeToDatabase(RecipeViewModel recipe);

        Task<ICollection<RecipeViewModel>> ShowAllRecipes();

        Task DeleteRecipe(int id);

        Task<RecipeViewModel> GetRecipeById(int id);
    }
}
