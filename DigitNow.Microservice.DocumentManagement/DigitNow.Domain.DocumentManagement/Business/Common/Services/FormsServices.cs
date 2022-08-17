using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IFormsServices
    {
        Task<List<FormFieldMapping>> GetFormFieldMappingsByFormIdAsync(long formId, CancellationToken cancellationToken);
    }

    public class FormsServices: IFormsServices
    {
        private readonly DocumentManagementDbContext _dbContext;

        public FormsServices(DocumentManagementDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<FormFieldMapping>> GetFormFieldMappingsByFormIdAsync(long formId, CancellationToken cancellationToken)
        {
            return await _dbContext.FormFieldMappings
                .Where(c => c.FormId == formId)
                .Include(c => c.FormField)
                .ToListAsync(cancellationToken);
        }
    }
}