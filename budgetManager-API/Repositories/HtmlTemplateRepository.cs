using budgetManager.Data;
using budgetManager.Models;
using budgetManager.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace budgetManager.Repositories
{
    public class HtmlTemplateRepository : IHtmlTemplateRepository
    {
        private readonly DataContext _context;
        public HtmlTemplateRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<HtmlTemplate> GetTemplateByName(string templateName)
        {
            var htmlTemplateFromRepo = await _context.HtmlTemplates.FirstOrDefaultAsync(x => x.Name == templateName);
            return htmlTemplateFromRepo == null ? new HtmlTemplate() : htmlTemplateFromRepo;
        }
    }
}
