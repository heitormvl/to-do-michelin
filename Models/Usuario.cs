using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public string? ResetToken { get; set; }
        
        public DateTime? ResetTokenExpiry { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        public bool Ativo { get; set; } = true;
    }
} 