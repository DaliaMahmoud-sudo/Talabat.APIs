using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        //get or recreate
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket (string BasketId)
        {
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) 
            {
                return new CustomerBasket(BasketId);
            }
            return Ok(Basket);

            
        }


        //update or create

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket (CustomerBasket Basket)
        {
           var CreateOrUpdatedBasket= await _basketRepository.UpdateBasketAsync(Basket);
            if (CreateOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreateOrUpdatedBasket);
        }



        //delete basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
           return await _basketRepository.DeleteTaskAsync(BasketId);  
        }
    }
}
