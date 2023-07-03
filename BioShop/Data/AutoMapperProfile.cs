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
            CreateMap<Client, GetClientIdViewModel>(); // yes
            CreateMap<Client, GetClientAndAllHisProductsViewModel>(); // yes

            //Product
            CreateMap<AllRecipesProductViewModel, Product>();
            CreateMap<Product, AllRecipesProductViewModel>();

            //Recipe
            //CreateMap<Recipe, AllRecipesOnProductViewModel>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, GetClientAndAllHisProductsViewModel>();
                cfg.CreateMap<ClientProduct, AllRecipesProductViewModel>();
            });
        }
    }
}