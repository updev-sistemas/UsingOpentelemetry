using Domain.ValuesObjects;
using Refit;

namespace Services.HttpClient;

public interface IProductApi
{
    [Get("/get-by-code")]
    Task<ProductValueObject?> GetByCodeAsync([Query] string target);
}
