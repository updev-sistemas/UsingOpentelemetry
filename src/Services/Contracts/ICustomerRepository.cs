using Domain.ValuesObjects;

namespace Services.Contracts
{
    public interface ICustomerRepository
    {
        Task<CustomerValueObject?> FindByDocumentAsync(string document, CancellationToken cancellationToken);
        Task<CustomerValueObject?> FindByIdAsync(long id, CancellationToken cancellationToken);
        Task<ICollection<CustomerValueObject>?> ListAsync(int currentPage, int perPage, CancellationToken cancellationToken);
        Task<CustomerValueObject?> RegisterAsync(CustomerValueObject customerValueObject, CancellationToken cancellationToken);
        Task<CustomerValueObject?> UpdateAsync(long id, CustomerValueObject customerValueObject, CancellationToken cancellationToken);
    }
}