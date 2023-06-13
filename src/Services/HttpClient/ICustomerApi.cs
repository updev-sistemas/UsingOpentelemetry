using Domain.ValuesObjects;
using Refit;

namespace Services.HttpClient;

public interface ICustomerApi
{
    [Get("/get-by-document")]
    Task<CustomerValueObject?> GetByDocumentAsync([Query] string target);

    [Post("/register-new-customer")]
    Task<CustomerValueObject?> RegisterCustomerAsync([Body] CustomerValueObject customerCreate);
}
