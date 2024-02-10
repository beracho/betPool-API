using System.ComponentModel.DataAnnotations;

namespace budgetManager.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Account>? Accounts{ get; set; }
    }
}
