using LinkNodeDomain.Model;

namespace LinkNodeInfrastructure.Services
{
    public class CategoryDataPortServiceFactory
        : IDataPortServiceFactory<Category>
    {
        private readonly DbLinkNodeContext _context;
        public CategoryDataPortServiceFactory(DbLinkNodeContext context)
        {
            _context = context;
        }
        public IImportService<Category> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new CategoryImportService(_context);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
        }
        public IExportService<Category> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new CategoryExportService(_context);
            }
            throw new NotImplementedException($"No export service implemented for categories with content type {contentType}");
        }
    }
}
