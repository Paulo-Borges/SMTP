using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMTP.API.DataContext;
using SMTP.API.DTOs;

namespace SMTP.API.Controllers
{
        public record LoginCommand(string Email, string Password) : IRequest<string>;

    [ApiController]
    [Route("api/auth")]
    public class AppController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ISender _sender;
        public AppController(ISender sender, AppDbContext context)
        {
            _sender = sender;
            _context = context;
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

        [HttpGet("membros")]
        public async Task<IActionResult> GetMembros()
        {
            var membros = await _context.Membros.ToListAsync();

            //// Exemplo retornando lista mockada ou vinda do serviço/banco----------X---
            //var membros = new List<object>
            //{
            //    new { id = 1, nome = "Membro 1" },
            //    new { id = 2, nome = "Membro 2" }
            //};

            return Ok(membros);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            //  USANDO O BACKENDO MOCKADO--------------X----------------X---------------
            //var users = new List<object>
            //{
            //    new { id = 1, nome = "User 1" },
            //    new { id = 2, nome = "User 2" }
            //};

            return Ok(users);
        }
    }
}
