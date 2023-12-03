using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Entity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service_Contract;
using Talabat.Core.Specifications.Order_spec;

namespace Talabat.Service
{
    public class OrderServices : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderServices(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string bayerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //Get Basket From Basket Repo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //Get Selected Items at Basket from Products Repo
            var orderItems = new List<OrderItem>();

            if (basket?.Items.Count() > 0 ) 
            {
                var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);

                    var productItemOrder = new ProductItemOrder(item.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                } 
            }


            //Calculate Subtotal
            var subTotal = orderItems.Sum(orderItem => orderItem.Price + orderItem.Quantity);

            //Get Delivery Method from DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //check about the paymentIntentId
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existingOrder = await orderRepo.GetEntityWithSpecAsync(spec);

            if (existingOrder != null)
            {
                orderRepo.Delete(existingOrder);

                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            //Create order
            var order = new Order(bayerEmail,shippingAddress,DeliveryMethod,orderItems,subTotal , basket.PaymentIntentId);
            await orderRepo.AddAsync(order);

            //Save To Database
            var result = await _unitOfWork.Complete();

            if(result <= 0) return null;
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            var orderRepo =  _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var orders = await orderRepo.GetAllWithSpecAsync(spec);

            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string bayerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            
            var spec = new OrderSpecifications(orderId , bayerEmail);

            var order = await orderRepo.GetEntityWithSpecAsync(spec);

            if (order is null) return null; 
            
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
               => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}
