using System.ComponentModel.DataAnnotations;

namespace budgetManager.Models
{
    public class Seat
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; } = default!;
        public DateTime Date { get; set; }
        public double ExchangeRate { get; set; }
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public virtual ICollection<Entry> Entries { get; set; } = default!;
    }
}
