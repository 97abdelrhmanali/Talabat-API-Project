using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Entity;
using TalabatAPIs.DTOs;
using OrderAddress = Talabat.Core.Entities.Order_Entity.Address;
using UserAddress = Talabat.Core.Entities.Identity.Address;

namespace TalabatAPIs.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brands.Name))
                .ForMember(d => d.Categorie , o => o.MapFrom(S => S.Categories.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, OrderAddress>();
            CreateMap<UserAddress , AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl , o => o.MapFrom<OrderItemPictureResolver>());
        }
    }
}
