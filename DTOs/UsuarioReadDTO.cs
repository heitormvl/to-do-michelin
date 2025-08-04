namespace to_do_michelin.DTOs
{
    public class UsuarioReadDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public bool Ativo { get; set; }
    }
} 