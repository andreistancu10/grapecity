using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Repositories
{
    public interface IDocumentResolutionService
    {
        Task<DocumentResolution> AddAsync(DocumentResolution newDocumentResolution, CancellationToken cancellationToken);
    }

    public class DocumentResolutionService : IDocumentResolutionService
    {
        private readonly IDocumentResolutionRepository _repo;

        public DocumentResolutionService(
            IDocumentResolutionRepository repo)
        {
            _repo = repo;
        }

        public async Task<DocumentResolution> AddAsync(DocumentResolution newDocumentResolution, CancellationToken cancellationToken)
        {
            await _repo.InsertAsync(newDocumentResolution, cancellationToken);
            await _repo.CommitAsync(cancellationToken);
            return newDocumentResolution;
        }
    }
}
