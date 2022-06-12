
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business._Common.Documents.Interfaces
{
    public interface IDocumentService
    {
        Task AssignRegNumberAndSaveDocument<T>(T document) where T : class;
    }
}
