using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries
{
    public class FilterFormsQuery:  IQuery<ResultPagedList<FilterFormsResponse>>
    {
        
    }
}