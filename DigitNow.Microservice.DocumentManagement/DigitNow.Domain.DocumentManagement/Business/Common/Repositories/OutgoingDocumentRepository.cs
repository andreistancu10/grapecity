using DigitNow.Domain.DocumentManagement.Business.Common.Repositories;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Repositories
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
