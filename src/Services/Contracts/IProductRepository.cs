using Domain.ValuesObjects;

namespace Services.Contracts
{
    public interface IProductRepository
    {
        Task<ProductValueObject?> FindByCodeAsync(string code, CancellationToken cancellationToken);
        Task<ProductValueObject?> FindByIdAsync(long id, CancellationToken cancellationToken);
        Task<ICollection<ProductValueObject>?> ListAsync(int currentPage, int perPage, CancellationToken cancellationToken);
        Task<ProductValueObject?> RegisterAsync(ProductValueObject productValueObject, CancellationToken cancellationToken);
        Task<ProductValueObject?> UpdateAsync(long id, ProductValueObject productValueObject, CancellationToken cancellationToken);
    }
}