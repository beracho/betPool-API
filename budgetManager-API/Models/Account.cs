using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budgetManager.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Level { get; set; }
        public double Balance { get; set; }
        public bool Active { get; set; }
        public Guid? FatherAccountId { get; set; }
        public virtual ICollection<Entry>? Entries { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
