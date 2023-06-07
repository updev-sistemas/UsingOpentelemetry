using Domain.Contracts;

namespace Domain.Entities;

public class Product : IEntity
{
    public virtual long? Id { get; set; }
    public virtual string? Code { get; set; }
    public virtual string? Description { get; set; }
    public virtual decimal? Price { get; set; }
    public virtual DateTime? CreatedAt { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
}