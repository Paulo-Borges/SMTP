namespace SMTP.API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;

        // ---------X----------------X--Conectando com Membro---X----------------------
        public ICollection<MembroModel> Membros { get; set; } = new List<MembroModel>();
    }
}
