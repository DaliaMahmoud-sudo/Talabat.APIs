using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository, 
              IUnitOfWork unitOfWork  )
        {
            _basketRepository = basketRepository;
            _unitOfWork=unitOfWork;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //get basket from basket repo
            var Basket= await _basketRepository.GetBasketAsync(BasketId);
            //get selected items at basket from product repo
            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product =await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered= new ProductItemOrdered(Product.Id , Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, Product.Price);
                    OrderItems.Add(OrderItem);
        
                }

            }
            //calculate sub total
            var SubTotal= OrderItems.Sum(item=>item.Price * item.Quantity);

            //get delivery method from delivery method repo
            var DeliveryMethod= await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            //create order
            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems,SubTotal);

            //add order locally
            _unitOfWork.Repository<Order>().AddAsync(Order);

            //save to database

         var Result=  await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;

        }

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
        {
            var Spec= new OrderSpec(BuyerEmail,OrderId);
            var Order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpeicificUserAsync(string BuyerEmail)
        {
            var Spec= new OrderSpec(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }
    }
}
