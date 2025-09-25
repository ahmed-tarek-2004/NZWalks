using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Forwarding;
using static System.Net.WebRequestMethods;

namespace Testing.AuthServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly stripe stripe;
        private readonly IConfiguration configuration;
        public StripeController(IOptions<stripe> option, IConfiguration configuration)
        {
            stripe = option.Value;
            this.configuration = configuration;
            StripeConfiguration.ApiKey = configuration["stripe:SecretKey"];
        }
        [HttpGet]
        public IActionResult Get()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount =80000,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Test"
                        },
                    },
                    Quantity = 2,
                },
            },
                Mode = "payment",
                SuccessUrl = "https://amars-marvelous-site-305200.webflow.io/",
                CancelUrl = "https://amars-fantabulous-site-16cb2e.webflow.io/"

            };

            var service = new SessionService();
            var session = service.Create(options);

            return Ok(new { url = session.Url, sessionId = session.Id });
        }
    }
}
