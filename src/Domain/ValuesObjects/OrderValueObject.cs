using Domain.Enums;

namespace Domain.ValuesObjects;

public class OrderValueObject : ObjectBaseValueObject
{
    public virtual CustomerValueObject? Customer { get; set; }
    public virtual IEnumerable<OrderItemValueObject>? Items { get; set; }
    public virtual PaymentMethod? Payment { get; set; }
}
