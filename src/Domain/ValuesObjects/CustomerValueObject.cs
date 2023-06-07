namespace Domain.ValuesObjects;

public class CustomerValueObject : ObjectBaseValueObject
{
    public virtual string? Document { get; set; }
    public virtual string? Name { get; set; }    
    public virtual string? Email { get; set; }
    public virtual string? Phone { get; set; }
}
