using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Entity;

namespace Talabat.Core.Specifications.Order_spec
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        public OrderSpecifications(string email)
            : base(O => O.BuyerMail == email)
        {
            AddInclude();
            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderSpecifications(int orderId,string email)
            : base(O => O.Id == orderId && O.BuyerMail == email)
        {
            AddInclude();
        }

        private void AddInclude()
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
