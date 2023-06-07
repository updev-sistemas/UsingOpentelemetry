namespace Domain.ValuesObjects;

public abstract class ObjectBaseValueObject
{
    public virtual long? Id { get; set; }
    public virtual DateTime? CreatedAt { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
}
