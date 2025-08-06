namespace to_do_michelin.DTOs
{
    public class UserCreateDTO
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? NomeCompleto { get; set; }
    }
}