using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Entity;
using Talabat.Core.Service_Contract;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string endpointSecret = "whsec_be317dbcf74993cb8f9c2b911036d79c1d7af248a7efd525b768ae16b581f8c6";


        public PaymentsController(
            IPaymentService paymentService,
            ILogger<PaymentsController> logger
            )
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRespone),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) return BadRequest(new ApiRespone(400));

            return Ok(basket);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

            var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;
            Order order;
            // Handle the event
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order = await _paymentService.UpdatePaymentIntentToSuccededOrFailed(paymentIntent.Id, true);
                    _logger.LogInformation("Payment is Succeded",paymentIntent.Id);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdatePaymentIntentToSuccededOrFailed(paymentIntent.Id, false);
                    _logger.LogInformation("Payment is Failed",paymentIntent.Id);
                    break;
            }

            // ... handle other event types

            return Ok();
            

        }

    }
}
