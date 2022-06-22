using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Repositories
{
    public interface IIncomingDocumentRepository : IEntityRepository<IncomingDocument>
    {
    }

    public class IncomingDocumentRepository : EntityRepository<IncomingDocument>, IIncomingDocumentRepository
    {
        private readonly DocumentManagementDbContext _dbContext;

        public IncomingDocumentRepository(DocumentManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
