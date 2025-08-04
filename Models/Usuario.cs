using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.Models
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(50)]
        public required string Username { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Email { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string PasswordHash { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }
        public bool Ativo { get; set; } = true;
    }
} 