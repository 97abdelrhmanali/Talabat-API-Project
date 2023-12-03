using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Entity;

namespace Talabat.Core.Service_Contract
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string bayerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrderForUserAsync(string bayerEmail);
        Task<Order?> GetOrderByIdForUserAsync(int orderId, string bayerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
