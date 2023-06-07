namespace Domain.ValuesObjects;

public class ProductValueObject : ObjectBaseValueObject
{
    public virtual string? Code { get; set; }
    public virtual string? Description { get; set; }
    public virtual decimal? Price { get; set; }
}
