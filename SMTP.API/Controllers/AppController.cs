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

        [HttpPost("membros")]
        public async Task<IActionResult> CreateMembro([FromBody] DTOCreateMembroRequest request)
        {
            var userExiste = await _context.Users.AnyAsync(u => u.Id == request.UserId);
            if(!userExiste)
            {
                return BadRequest(new { message = $"UserId {request.UserId} não existe." });
            }
            var membro = new Models.MembroModel
            {
                Nome = request.Nome,
                Cargo = request.Cargo,
                UserId = request.UserId
            };

            _context.Membros.Add(membro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembros), new { id = membro.Id }, new
            {
                membro.Id,
                membro.Nome,
                membro.Cargo,
                membro.UserId
            });
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
