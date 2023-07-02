namespace BioShop.Data
{
    using AutoMapper;
    using BioShop.Data.Models;
    using BioShop.Data.ViewModels.ClientModels;
    using BioShop.Data.ViewModels.ProductModel;
    using BioShop.Data.ViewModels.ProductModels;
    using BioShop.Data.ViewModels.RecipeModel;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Client
            CreateMap<AddClientToShopViewModel, Client>(); // Yes
            CreateMap<Client, AddClientToShopViewModel>(); // Yes
            CreateMap<Client, GetClienttyIdViewModel>(); // yes

            //Product
            //CreateMap<ProductViewModelAddProduct, Product>(); // Yes
            //CreateMap<List<Product>, List<ProductViewModelAddProduct>>(); // Yes
            CreateMap<AllRecipesProductViewModel, Product>();
            CreateMap<Product, AllRecipesProductViewModel>();

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Client, ClientViewModel>();
            //    cfg.CreateMap<ClientProduct, ProductViewModel>();
            //});
        }
    }
}