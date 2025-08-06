namespace to_do_michelin.DTOs
{
    public class UserUpdateDTO
    {
        public required string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? NomeCompleto { get; set; }
        public string? Password { get; set; }
    }
}