using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Service;
using PaymentApiTest.Models;
using System.Security.Claims;

namespace PaymentApi.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult MakePayment()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var success = _paymentService.MakePayment(userId);

            if (!success)
            {
                return BadRequest("Failed to make payment");
            }

            return Ok();
        }
    }
}
