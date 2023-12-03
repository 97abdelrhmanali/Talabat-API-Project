using AutoMapper;
using Talabat.Core.Entities.Order_Entity;
using TalabatAPIs.DTOs;

namespace TalabatAPIs.Helper
{
    internal class OrderItemPictureResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["AppBaseUrl"]}/{source.Product.PictureUrl}";

            return string.Empty;            
        }
    }
}