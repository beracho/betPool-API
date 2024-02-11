using System.ComponentModel.DataAnnotations;

namespace budgetManager.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public byte[] PasswordHash { get; set; } = default!;
        public byte[] PasswordSalt { get; set; } = default!;
        public string Telephone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int LoginAttempts { get; set; }
        public DateTime AttemptsUnblockTime { get; set; }
        public DateTime LastActive { get; set; }
        public virtual ICollection<Account>? Accounts{ get; set; }
    }
}
