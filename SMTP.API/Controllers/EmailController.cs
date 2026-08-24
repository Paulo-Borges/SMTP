using Microsoft.AspNetCore.Mvc;
using SMTP.API.SendEmail;

namespace SMTP.API.Controllers
{
    /// <summary>
    /// Controller para gerenciar envio de emails
    /// Endpoints disponíveis para integração com Angular
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        /// <summary>
        /// Construtor com injeção de dependência
        /// </summary>
        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Envia um email
        /// </summary>
        /// <param name="request">Dados do email a ser enviado</param>
        /// <returns>Retorna o status do envio</returns>
        /// <response code="200">Email enviado com sucesso</response>
        /// <response code="400">Erro na validação ou envio do email</response>
        [HttpPost("enviar")]
        [ProducesResponseType(typeof(EmailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EmailResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EnviarEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido recebido na requisição de email.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Requisição de envio de email recebida para {request.Destinatarios.Count} destinatário(s).");

            var response = await _emailService.EnviarEmailAsync(request);

            if (response.Sucesso)
            {
                _logger.LogInformation("Email enviado com sucesso.");
                return Ok(response);
            }

            _logger.LogError($"Erro ao enviar email: {response.Mensagem}");
            return BadRequest(response);
        }

        /// <summary>
        /// Verifica se o serviço de email está funcionando
        /// </summary>
        /// <returns>Status do serviço</returns>
        /// <response code="200">Serviço funcionando normalmente</response>
        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Health()
        {
            _logger.LogInformation("Health check do serviço de email executado.");
            return Ok(new 
            { 
                status = "Email service is running", 
                timestamp = DateTime.UtcNow,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }

        /// <summary>
        /// Valida um endereço de email sem enviar
        /// </summary>
        /// <param name="email">Endereço de email a validar</param>
        /// <returns>Retorna se o email é válido</returns>
        /// <response code="200">Validação concluída</response>
        [HttpPost("validar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ValidarEmail([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { valido = false, mensagem = "Email não pode estar vazio." });
            }

            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                var valido = address.Address == email;

                _logger.LogInformation($"Email '{email}' validado: {valido}");

                return Ok(new 
                { 
                    valido = valido, 
                    mensagem = valido ? "Email válido" : "Email inválido" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Erro ao validar email '{email}': {ex.Message}");
                return Ok(new 
                { 
                    valido = false, 
                    mensagem = "Email inválido" 
                });
            }
        }
    }
}
