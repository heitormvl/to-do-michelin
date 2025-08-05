namespace to_do_michelin.DTOs
{
    public class ResetPasswordDTO
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
    }
} 