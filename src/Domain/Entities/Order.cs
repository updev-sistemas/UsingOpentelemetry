using Domain.Contracts;
using Domain.Enums;

namespace Domain.Entities;

public class Order : IEntity
{
    public virtual long? Id { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual long? CustomerId { get; set; }
    public virtual IEnumerable<OrderItem>? Items { get; set; }
    public virtual PaymentMethod? Payment { get; set; }
    public virtual DateTime? CreatedAt { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
}
