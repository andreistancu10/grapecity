using DigitNow.Adapters.MS.Catalog.Poco;

namespace DigitNow.Adapters.MS.Catalog
{
    public interface ICatalogAdapterClient
    {
        Task<DocumentType> GetDocumentTypeByIdAsync(int id, CancellationToken cancellationToken);
        Task<IList<DocumentType>> GetDocumentTypesAsync(CancellationToken cancellationToken);
        Task<IList<DocumentType>> GetInternalDocumentTypesAsync(CancellationToken cancellationToken);
        Task<Department> GetDepartmentByCodeAsync(string code, CancellationToken cancellationToken);
    }
    public class CatalogAdapterClient : ICatalogAdapterClient
    {
        private readonly CatalogHttpClient _catalogHttpClient;

        public CatalogAdapterClient(CatalogHttpClient catalogHttpClient)
        {
            _catalogHttpClient = catalogHttpClient;
        }

        public async Task<Department> GetDepartmentByCodeAsync(string code, CancellationToken cancellationToken)
        {
            var pagedResult = await _catalogHttpClient.GetAsync<ResultPagedList<Department>>($"Departments/filter?Code={code}", cancellationToken);
            return pagedResult.Items.First();
        }

        public Task<DocumentType> GetDocumentTypeByIdAsync(int id, CancellationToken cancellationToken) => _catalogHttpClient.GetAsync<DocumentType>($"DocumentTypes/{id}", cancellationToken);
        
        public async Task<IList<DocumentType>> GetDocumentTypesAsync(CancellationToken cancellationToken)
        {
            var pagedResult = await _catalogHttpClient.GetAsync<ResultPagedList<DocumentType>>("DocumentTypes/filter", cancellationToken);

            var items = new List<DocumentType>(pagedResult.Items);
            while (pagedResult.PagingHeader.PageNumber < pagedResult.PagingHeader.TotalPages)
            {
                pagedResult = await _catalogHttpClient.GetAsync<ResultPagedList<DocumentType>>("DocumentTypes/filter", cancellationToken);
                items.AddRange(pagedResult.Items);
            }

            return items;
        }
        
        public async Task<IList<DocumentType>> GetInternalDocumentTypesAsync(CancellationToken cancellationToken)
        {
            var pagedResult = await _catalogHttpClient.GetAsync<ResultPagedList<DocumentType>>("internalDocumentTypes/filter", cancellationToken);

            var items = new List<DocumentType>(pagedResult.Items);
            while(pagedResult.PagingHeader.PageNumber < pagedResult.PagingHeader.TotalPages)
            {
                pagedResult = await _catalogHttpClient.GetAsync<ResultPagedList<DocumentType>>("internalDocumentTypes/filter", cancellationToken);
                items.AddRange(pagedResult.Items);
            }

            return items;
        }
    }
}
