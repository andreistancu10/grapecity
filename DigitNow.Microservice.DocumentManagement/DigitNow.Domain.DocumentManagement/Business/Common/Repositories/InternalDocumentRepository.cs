using DigitNow.Domain.DocumentManagement.Business.Common.Repositories;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Repositories
{
    public interface IInternalDocumentRepository : IEntityRepository<InternalDocument>
    {
    }

    public class InternalDocumentRepository : EntityRepository<InternalDocument>, IInternalDocumentRepository
    {
        private readonly DocumentManagementDbContext _dbContext;

        public InternalDocumentRepository(DocumentManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
