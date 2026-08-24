namespace SMTP.API.SendEmail
{
    /// <summary>
    /// Interface de serviço de email
    /// Define o contrato para implementações de envio de email
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envia um email de forma assíncrona
        /// </summary>
        /// <param name="request">Dados do email a ser enviado</param>
        /// <returns>Resposta com status do envio</returns>
        Task<EmailResponse> EnviarEmailAsync(EmailRequest request);
    }
}
