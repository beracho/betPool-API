using System.ComponentModel.DataAnnotations;

namespace budgetManager.Models
{
    public class HtmlTemplate
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Body { get; set; } = default!;
    }
}
