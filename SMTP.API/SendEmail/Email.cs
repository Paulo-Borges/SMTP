using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SMTP.API.SendEmail
{
    /// <summary>
    /// Serviço de envio de emails com suporte a SMTP configurável
    /// Implementa validações, tratamento de erros e logging
    /// </summary>
    public class Email : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Email> _logger;

        /// <summary>
        /// Construtor com injeção de dependência
        /// </summary>
        public Email(IConfiguration configuration, ILogger<Email> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Envia um email de forma assíncrona
        /// </summary>
        public async Task<EmailResponse> EnviarEmailAsync(EmailRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de envio de email");

                // Validar requisição
                ValidarRequisicao(request);
                _logger.LogInformation("Validação da requisição concluída com sucesso");

                // Preparar mensagem
                var message = PrepararMensagem(request);
                _logger.LogInformation("Mensagem preparada com sucesso");

                // Enviar via SMTP
                await EnviarViaSMTPAsync(message);
                _logger.LogInformation($"Email enviado com sucesso para {string.Join(", ", request.Destinatarios)}");

                return new EmailResponse
                {
                    Sucesso = true,
                    Mensagem = "Email enviado com sucesso!",
                    DataEnvio = DateTime.UtcNow
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Erro de validação ao enviar email: {ex.Message}");
                return new EmailResponse
                {
                    Sucesso = false,
                    Mensagem = ex.Message,
                    DataEnvio = DateTime.UtcNow
                };
            }
            catch (SmtpException ex)
            {
                _logger.LogError($"Erro SMTP ao enviar email: {ex.Message}");
                return new EmailResponse
                {
                    Sucesso = false,
                    Mensagem = "Erro ao conectar com o servidor de email. Verifique as credenciais SMTP.",
                    DataEnvio = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado ao enviar email: {ex.Message}");
                return new EmailResponse
                {
                    Sucesso = false,
                    Mensagem = "Erro inesperado ao enviar email.",
                    DataEnvio = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Valida os dados da requisição de email
        /// </summary>
        private void ValidarRequisicao(EmailRequest request)
        {
            if (request == null)
                throw new ArgumentException("Requisição não pode ser nula.");

            if (request.Destinatarios == null || request.Destinatarios.Count == 0)
                throw new ArgumentException("Deve haver pelo menos um destinatário.");

            if (string.IsNullOrWhiteSpace(request.Assunto))
                throw new ArgumentException("Assunto não pode estar vazio.");

            if (string.IsNullOrWhiteSpace(request.Corpo))
                throw new ArgumentException("Corpo do email não pode estar vazio.");

            if (!string.IsNullOrWhiteSpace(request.EmailOrigem) && !ValidarEmail(request.EmailOrigem))
                throw new ArgumentException($"Email de origem '{request.EmailOrigem}' é inválido.");

            foreach (var email in request.Destinatarios)
            {
                if (!ValidarEmail(email))
                    throw new ArgumentException($"Email '{email}' é inválido.");
            }

            if (request.Anexos != null && request.Anexos.Count > 0)
            {
                foreach (var arquivo in request.Anexos)
                {
                    if (!File.Exists(arquivo))
                        throw new ArgumentException($"Arquivo '{arquivo}' não foi encontrado.");
                }
            }
        }

        /// <summary>
        /// Prepara a mensagem de email com todos os dados
        /// </summary>
        private MailMessage PrepararMensagem(EmailRequest request)
        {
            var emailOrigemPadrao = _configuration["SMTP:EmailOrigem"];
            var emailOrigem = string.IsNullOrWhiteSpace(request.EmailOrigem)
                ? emailOrigemPadrao
                : request.EmailOrigem;

            if (string.IsNullOrWhiteSpace(emailOrigem))
                throw new ArgumentException("Email de origem não foi configurado.");

            var mail = new MailMessage
            {
                From = new MailAddress(emailOrigem),
                Subject = request.Assunto,
                Body = request.Corpo,
                IsBodyHtml = true
            };

            foreach (var email in request.Destinatarios)
            {
                mail.To.Add(new MailAddress(email));
            }

            // Adicionar anexos se houver
            if (request.Anexos != null && request.Anexos.Count > 0)
            {
                foreach (var arquivo in request.Anexos)
                {
                    try
                    {
                        var attachment = new Attachment(arquivo, MediaTypeNames.Application.Octet);
                        var disposition = attachment.ContentDisposition;

                        if (disposition != null)
                        {
                            disposition.CreationDate = File.GetCreationTime(arquivo);
                            disposition.ModificationDate = File.GetLastWriteTime(arquivo);
                            disposition.ReadDate = File.GetLastAccessTime(arquivo);
                        }

                        mail.Attachments.Add(attachment);
                        _logger.LogInformation($"Anexo '{arquivo}' adicionado com sucesso");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Erro ao adicionar anexo '{arquivo}': {ex.Message}");
                    }
                }
            }

            return mail;
        }

        /// <summary>
        /// Envia a mensagem através do servidor SMTP
        /// </summary>
        private async Task EnviarViaSMTPAsync(MailMessage message)
        {
            var smtpHost = _configuration["SMTP:Host"];
            var smtpPort = int.Parse(_configuration["SMTP:Port"] ?? "587");
            var smtpUsername = _configuration["SMTP:Username"];
            var smtpPassword = _configuration["SMTP:Password"];
            var enableSSL = bool.Parse(_configuration["SMTP:EnableSSL"] ?? "true");

            _logger.LogInformation($"Conectando ao servidor SMTP: {smtpHost}:{smtpPort}");

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = smtpHost;
                smtpClient.Port = smtpPort;
                smtpClient.EnableSsl = enableSSL;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.Timeout = 30000;

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation("Email enviado com sucesso pelo SMTP");
            }
        }

        /// <summary>
        /// Valida se um endereço de email é válido
        /// </summary>
        private bool ValidarEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
