using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounter;

namespace DigitNow.Domain.DocumentManagement.Business._Common.Documents.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        public DocumentService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AssignRegNumberAndSaveDocument<T>(T document) where T : class
        {
            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            try
            {
                var maxRegNumber = await _dbContext.RegistrationNumberCounter
                    .Where(reg => reg.RegistrationDate.Year == DateTime.Now.Year)
                    .Select(reg => reg.RegistrationNumber)
                    .DefaultIfEmpty()
                    .MaxAsync();

                var newRegNumber = ++maxRegNumber;

                var type = document.GetType();
                var property = type.GetProperty("RegistrationNumber");

                property.SetValue(document, newRegNumber);

                await _dbContext.AddAsync(document);
                await _dbContext.RegistrationNumberCounter.AddAsync(new RegistrationNumberCounter { RegistrationNumber = newRegNumber, RegistrationDate = DateTime.Now });
                await _dbContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();
            }
            catch
            {
                await dbContextTransaction.RollbackAsync();
            }
        }
    }
}
