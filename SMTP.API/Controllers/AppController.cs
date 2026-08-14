using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SMTP.API.DataContext;
using SMTP.API.DTOs;

namespace SMTP.API.Controllers
{
        public record LoginCommand(string Email, string Password) : IRequest<string>;

    [ApiController]
    [Route("api/auth")]
    public class AppController : ControllerBase
    {

        private readonly ISender _sender;
        public AppController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost("login")]
        [AllowAnonymous]

        public async Task<IActionResult> Login([FromBody] DTOLoginRequest request, CancellationToken ct)
        {
            try
            {
                var response = await _sender.Send(new LoginCommand(request.Email, request.Password), ct);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Credencias inválidas" });
            }
        }
    }
}
