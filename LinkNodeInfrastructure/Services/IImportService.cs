using LinkNodeDomain.Model;

namespace LinkNodeInfrastructure.Services
{

    public interface IImportService<TEntity>
        where TEntity : Entity
    {
        Task ImportFromStreamAsync(Stream stream, int clientId, CancellationToken cancellationToken);
    }
}
