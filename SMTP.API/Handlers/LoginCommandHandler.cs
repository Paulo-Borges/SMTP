using MediatR;
using SMTP.API.Controllers; // Garante o acesso ao LoginCommand

namespace SMTP.API.Handlers
{
    // 1. Alterado de "AppController.LoginCommand" para apenas "LoginCommand"
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        // 2. Removido o ponto e vírgula ';' após a assinatura do método e adicionadas as chaves { }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            return await Task.FromResult("TokenJWTDeExemploAqui12345");


            //// 1. Lógica de autenticação--------X------MOCKADO--------------X-------------------
            //if (request.Email == "admin@email.com" && request.Password == "123456")
            //{
            //    return "TokenJWTDeExemploAqui12345";
            //}

            //throw new UnauthorizedAccessException();
        }
    }
}
