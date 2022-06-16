
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Contracts.Documents
{
    public interface IDocumentService
    {
        public Task AssignRegNumberAndSaveDocument<T>(T document) where T : class;
    }
}
