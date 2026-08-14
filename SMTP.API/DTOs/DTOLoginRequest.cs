namespace SMTP.API.DTOs
{
    public class DTOLoginRequest
    {
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }
}
