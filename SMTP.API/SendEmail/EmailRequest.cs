namespace SMTP.API.SendEmail
{
    /// <summary>
    /// Modelo de requisição para envio de email
    /// </summary>
    public class EmailRequest
    {
        /// <summary>
        /// Lista de emails destinatários
        /// </summary>
        public List<string> Destinatarios { get; set; } = new();

        /// <summary>
        /// Assunto do email
        /// </summary>
        public string Assunto { get; set; } = string.Empty;

        /// <summary>
        /// Corpo do email (suporta HTML)
        /// </summary>
        public string Corpo { get; set; } = string.Empty;

        /// <summary>
        /// Lista de caminhos dos arquivos a anexar (opcional)
        /// </summary>
        public List<string> Anexos { get; set; } = new();

        /// <summary>
        /// Email remetente (opcional). Se não informado, usa o configurado em SMTP:EmailOrigem
        /// </summary>
        public string? EmailOrigem { get; set; }
    }
}
