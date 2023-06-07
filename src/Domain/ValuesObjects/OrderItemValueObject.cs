namespace Domain.ValuesObjects;

public class OrderItemValueObject : ObjectBaseValueObject
{
    public virtual ProductValueObject? Product { get; set; }
    public virtual decimal? Quantity { get; set; }
    public virtual decimal? Cost { get; set; }
}
