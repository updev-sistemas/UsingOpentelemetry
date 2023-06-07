using Domain.Contracts;

namespace Domain.Entities;

public class OrderItem : IEntity
{
    public virtual long? Id { get; set; }
    public virtual Order? Order { get; set; }
    public virtual long? OrderId { get; set; }
    public virtual Product? Product { get; set; }
    public virtual long? ProductId { get; set; }
    public virtual decimal? Quantity { get; set; }
    public virtual decimal? Cost { get; set; }
    public virtual DateTime? CreatedAt { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
}