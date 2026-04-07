using LinkNodeDomain.Model;

namespace LinkNodeInfrastructure.Services
{
    public interface IExportService<TEntity>
    where TEntity : Entity
    {
       Task WriteToAsync(Stream stream, int clientId, CancellationToken cancellationToken);
    }
}
