namespace to_do_michelin.DTOs
{
    public class UsuarioUpdateDTO
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
} 