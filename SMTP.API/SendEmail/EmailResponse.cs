namespace SMTP.API.SendEmail
{
    /// <summary>
    /// Modelo de resposta para envio de email
    /// </summary>
    public class EmailResponse
    {
        /// <summary>
        /// Indica se o email foi enviado com sucesso
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem de retorno (sucesso ou erro)
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora do envio (UTC)
        /// </summary>
        public DateTime DataEnvio { get; set; }
    }
}
