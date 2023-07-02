namespace BioShop.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using BioShop.Data.ViewModels.ProductModels;

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Client")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _productService.GetAllProducts();

            return Ok(allProducts);
        }

        [HttpGet("get-product-by-and-all-his-recipes-id/{id}")]
        public async Task<IActionResult> GetProductByIdAndAllHisRecipes(int id)
        {
            var product = await _productService.GetProductByIdAndAllHisRecipes(id);

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest("Product with this id does not exist");
            }
        }

        [HttpPost("add-product-to-shop")]
        public async Task<IActionResult> AddProductToShop([FromBody] AddProductProductViewModel product)
        {
            ArgumentNullException.ThrowIfNull(product);

            var returnAllProducts = await _productService.AddProduct(product);

            return Ok(returnAllProducts);
        }

        [HttpPut("update-product-by-id/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductViewModel product)
        {
            var updatedProduct = await _productService.UpdateProduct(id, product);

            return Ok(updatedProduct);
        }

        [HttpDelete("delete-product-by-id/{id}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            await _productService.DeleteProductById(id);

            return Ok();
        }

    }
}