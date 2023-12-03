using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Entity
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerMail, Address shippingAddress, DeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subTotal , string paymentIntentId)
        {
            BuyerMail = buyerMail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerMail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }

        public DeliveryMethod? DeliveryMethod { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

        public decimal SubTotal { get; set; }

        #region Dervied Attribute
       
        //Dervied Attribute
        //1) First Way : readOnly Property
        //[NotMapped]
        //public decimal Total { get { return SubTotal * DeliveryMethod.Cost; } }
        //public decimal Total => SubTotal * DeliveryMethod.Cost;

        //2) second Way : Method Must it's name start with (GET)
        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

        #endregion

        public string PaymentIntentId { get; set; }

    }
}
