namespace BioShop.Controllers
{
    using BioShop.Data.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("get-recipe-by-id/{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _recipeService.GetRecipeById(id);

            return Ok(recipe);
        }

        [HttpGet("show-all-recipes")]
        public async Task<IActionResult> ShowAllRecipes()
        {
            var allRecipes = await _recipeService.ShowAllRecipes();

            return Ok(allRecipes);
        }

        [HttpGet("add-recipe-to-product-by-id/{productId}/{recipeId}")]
        public async Task<IActionResult> AddRecipeToProduct(int productId, int recipeId)
        {
            await _recipeService.AddRecipeToProduct(productId, recipeId);

            return Ok();
        }

        [HttpPost("add-recipe-to-database")]
        public async Task<IActionResult> AddRecipeToDatabase([FromBody] RecipeViewModel recipe)
        {
            await _recipeService.AddRecipeToDatabase(recipe);

            return Ok();
        }

        [HttpDelete("delete-recipe-by-id/{id}")]
        public async Task<IActionResult> DeleteRecipeById(int id)
        {
            await _recipeService.DeleteRecipe(id);

            return Ok();
        }
    }
}