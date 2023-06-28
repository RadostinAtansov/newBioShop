namespace BioShop.Test.Controller
{
    using Moq;
    using Xunit;
    using BioShop.Controllers;
    using BioShop.Data.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;

    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _productService;

        public ProductControllerTest()
        {
            _productService = new Mock<IProductService>();
        }

        [Fact]
        public async Task GetAllProductReturnAllProducts()
        {
            //Assert
            var productList = await ProductData();
            _productService
                .Setup(x => x.GetAllProducts())
                .ReturnsAsync(productList);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.GetAllProducts();
            var productResultModel = ((ObjectResult)productResult).Value as List<ProductViewModel>;

            //Assert
            Assert.IsType<OkObjectResult>(productResult);
        }

        [Fact]
        public async Task GetProductByIdReturnOk()
        {
            //Arrange
            var productList = await ProductData();
            _productService
                .Setup(x => x.GetProductByIdAndAllHisRecipes(1))
                .ReturnsAsync(productList[0]);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.GetProductByIdAndAllHisRecipes(1);
            var productResultModel = ((ObjectResult)productResult).Value as ProductViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(productResult);
        }

        [Fact]
        public async Task GetProductByIdReturnOkBadRequest()
        {
            //Arrange
            var productList = await ProductData();
            ProductViewModel product = null;
            _productService
                .Setup(x => x.GetProductByIdAndAllHisRecipes(23)).ReturnsAsync(product);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.GetProductByIdAndAllHisRecipes(23);
            var productResultModel = ((ObjectResult)productResult).Value as ProductViewModel;

            //Assert
            Assert.IsType<BadRequestObjectResult>(productResult);
        }

        [Fact]
        public async Task AddProductToShopReturnOkObject()
        {
            //Arrange
            var productList = await ProductData();
            _productService
                .Setup(x => x.AddProduct(productList[0]))
                .ReturnsAsync(productList);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.AddProductToShop(productList[0]);
            var productResultModel = ((ObjectResult)productResult).Value as List<ProductViewModel>;

            //Assert
            Assert.IsType<OkObjectResult>(productResult);
        }

        [Fact]
        public async Task UpdateProductReturnProductAndOkObjectResult()
        {
            //Arrange
            var productList = await ProductData();

            ProductViewModel product = new ProductViewModel()
            {
                Id = 1,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco4, Milk4, Eggs4",
                Price = 4,
                MadeInCountry = "Bg4",
            };

            _productService
                .Setup(x => x.UpdateProduct(1, productList[0]))
                .ReturnsAsync(product);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.UpdateProduct(1, productList[0]);
            var productResultModel = ((ObjectResult)productResult).Value as ProductViewModel;

            //Assert
            Assert.IsType<OkObjectResult>(productResult);
        }

        [Fact]
        public async Task DeleteProductAndReturnOkObjectResult()
        {
            //Arrange
            var productList = await ProductData(); 

            _productService
                .Setup(x => x.DeleteProductById(1));
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.DeleteProductById(1);

            //Assert
            Assert.IsType<OkResult>(productResult);
        }
        
        private async Task<List<ProductViewModel>> ProductData()
        {
            List<ProductViewModel> productList = new List<ProductViewModel>()
                {
                    new ProductViewModel()
                    {
                        Id = 1,
                        Name = "Torta1",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs1",
                        Price = 12,
                        MadeInCountry = "Bg1",
                        RecipesProduct = new List<RecipeViewModel>(),

                    },
                    new ProductViewModel()
                    {
                        Id = 2,
                        Name = "Torta2",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs2",
                        Price = 12,
                        MadeInCountry = "Bg2",
                        RecipesProduct = new List<RecipeViewModel>(),
                    },
                    new ProductViewModel()
                    {
                        Id = 3,
                        Name = "Torta3",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs3",
                        Price = 12,
                        MadeInCountry = "Bg3",
                        RecipesProduct = new List<RecipeViewModel>(),
                    },
                };
            return productList;
        }
    }
}
