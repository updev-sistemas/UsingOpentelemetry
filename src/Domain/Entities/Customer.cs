using Domain.Contracts;

namespace Domain.Entities;

public class Customer : IEntity
{
    public virtual long? Id { get; set; }
    public virtual string? Document { get; set; }
    public virtual string? Name { get; set; }
    public virtual string? Email { get; set; }
    public virtual string? Phone { get; set; }
    public virtual DateTime? CreatedAt { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
}
