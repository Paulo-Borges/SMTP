namespace SMTP.API.Models
{
    public class MembroModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cargo { get; set; }


        // ---------X----------------X--Conectando com User---X--------ForeignKey---------------
        public int UserId { get; set; }
        public UserModel User { get; set; } = null!;

    }
}
