using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{

    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(
            IBasketRepository basketRepository,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var CreateOrUpdateBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (CreateOrUpdateBasket == null)
                return BadRequest(new ApiRespone(400));
            return Ok(CreateOrUpdateBasket);
        }


        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return Ok(await _basketRepository.DeleteBasketAsync(id));
        }
    }
}
