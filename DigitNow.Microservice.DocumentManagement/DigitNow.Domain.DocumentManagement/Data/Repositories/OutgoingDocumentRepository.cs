using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Repositories
{
    public interface IOutgoingDocumentRepository : IEntityRepository<OutgoingDocument>
    {
    }

    public class OutgoingDocumentRepository : EntityRepository<OutgoingDocument>, IOutgoingDocumentRepository
    {
        private readonly DocumentManagementDbContext _dbContext;

        public OutgoingDocumentRepository(DocumentManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
