using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.OrderAggregate;

namespace Talabat.Core.Services
{
     public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress );
        Task<IReadOnlyList<Order>> GetOrdersForSpeicificUserAsync(string BuyerEmail);
        Task<Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId);
    }
}
