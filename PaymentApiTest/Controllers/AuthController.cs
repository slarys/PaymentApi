using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Models;
using PaymentApi.Service;
using PaymentApiTest.Models;
using System.Security.Claims;

namespace PaymentApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            try
            {
                var isValidCredentials = _authService.ValidateCredentials(user.Username, user.PasswordHash);

                if (!isValidCredentials)
                {
                    return BadRequest("Invalid username or password");
                }

                var token = _authService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest request)
        {
            try
            {
                var success = _authService.Logout(request.UserId, request.TransactionId);

                if (!success)
                {
                    return BadRequest("Failed to logout");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
