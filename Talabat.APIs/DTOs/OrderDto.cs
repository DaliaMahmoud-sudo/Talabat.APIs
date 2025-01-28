

using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites.OrderAggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { set; get; }
        [Required]
        public int DeliveryMethodId { set; get; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
    }
}
