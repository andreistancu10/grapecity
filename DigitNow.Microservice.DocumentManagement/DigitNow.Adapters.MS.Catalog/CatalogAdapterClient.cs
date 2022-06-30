using DigitNow.Adapters.MS.Catalog.Poco;

namespace DigitNow.Adapters.MS.Catalog
{
    public interface ICatalogAdapterClient
    {
        Task<DocumentType> GetDocumentTypeById(int id, CancellationToken cancellationToken);
    }
    public class CatalogAdapterClient : ICatalogAdapterClient
    {
        private readonly CatalogHttpClient _catalogHttpClient;

        public CatalogAdapterClient(CatalogHttpClient catalogHttpClient)
        {
            _catalogHttpClient = catalogHttpClient;
        }
        public Task<DocumentType> GetDocumentTypeById(int id, CancellationToken cancellationToken) => _catalogHttpClient.GetAsync<DocumentType>($"api/DocumentTypes/{id}", cancellationToken);
        
    }
}
