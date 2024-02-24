using budgetManager.Models;

namespace budgetManager.Repositories.Interfaces
{
    public interface IHtmlTemplateRepository
    {
        Task<HtmlTemplate> GetTemplateByName(string templateName);
    }
}
