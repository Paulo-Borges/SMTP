namespace SMTP.API.DTOs
{
    public class DTOCreateMembroRequest
    {
        public required string Nome { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public required int UserId { get; set; }
    }
}
