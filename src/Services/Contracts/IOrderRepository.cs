using Domain.ValuesObjects;

namespace Services.Contracts
{
    public interface IOrderRepository
    {
        Task<ICollection<OrderValueObject>?> ListAsync(int currentPage, int perPage, CancellationToken cancellationToken);
        Task<OrderValueObject?> RegisterAsync(OrderValueObject order, CancellationToken cancellationToken);
        Task<OrderValueObject?> RegisterNewOrder(OrderValueObject order, CancellationToken cancellationToken);
    }
}