using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace budgetManager.Models
{
    public class Entry
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Type { get; set; } = default!;
        public string Currency { get; set; } = default!;
        [ForeignKey("Seat")]
        public Guid SeatId { get; set; } = default!;
        public Seat Seat { get; set; } = default!;
        [ForeignKey("Account")]
        public Guid AccountId { get; set; } = default!;
        public Account Account { get; set; } = default!;
    }
}
