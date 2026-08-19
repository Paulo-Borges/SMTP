using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SMTP.API.Controllers;
using SMTP.API.DataContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text; // Garante o acesso ao LoginCommand

namespace SMTP.API.Handlers
{
    // 1. Alterado de "AppController.LoginCommand" para apenas "LoginCommand"
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public LoginCommandHandler(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var claims = new[]
            {
            new Claim("nome", user.Nome),
            new Claim("email", user.Email),
            new Claim("cpf", user.Cpf),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
    
}
