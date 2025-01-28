using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.OrderAggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderSpec: BaseSpecifications<Order>
    {
        private int orderId;
        //to get orders for user
        public OrderSpec(string email): base(O=>O.BuyerEmail== email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O => O.OrderDate);

        }
        //to get order for user
        public OrderSpec(string email, int id) : base(O=>O.Id==id && O.BuyerEmail==email )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
