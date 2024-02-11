namespace budgetManager.Dto.UserDto
{
    public class UserForDetailedDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string Telephone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Status { get; set; } = default!;
    }
}
