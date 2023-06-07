using Domain.ValuesObjects;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> logger;
        private readonly ICustomerRepository repository;

        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        [Route("/get-all-customers")]
        public async Task<IActionResult> GetAllAsync(int? page = 1, int? perPage = 10)
        {
            //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("get-all-customers");
            //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetAllAsync)), new("Controller", nameof(customerController)));

            if (!page.HasValue || page.HasValue && page.Value < 1)
            {
                page = 1;
            }

            if (!perPage.HasValue || perPage.HasValue && perPage.Value < 1)
            {
                perPage = 10;
            }

            var result = await this.repository.ListAsync(page.Value, perPage.Value, CancellationToken.None).ConfigureAwait(false);

            return Ok(result);
        }


        [HttpGet]
        [Route("/get-by-id")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("get-by-id");
            //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByIdAsync)), new("Controller", nameof(customerController)));

            var result = await this.repository.FindByIdAsync(id, CancellationToken.None).ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("/get-by-document")]
        public async Task<IActionResult> GetByDocumentAsync(string target)
        {
            //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("get-by-code-or-ean");
            //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByCodeOrEanAsync)), new("Controller", nameof(customerController)));

            var result = await this.repository.FindByDocumentAsync(target, CancellationToken.None).ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("/register-new-customer")]
        public async Task<IActionResult> Registercustomer([FromBody] CustomerValueObject customerCreate)
        {
            //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("register-new-customer");
            //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(Registercustomer)), new("Controller", nameof(customerController)));

            var customer = await this.repository.RegisterAsync(customerCreate, CancellationToken.None).ConfigureAwait(false);

            if (customer is null)
            {
                return BadRequest($"customer {customerCreate.Document} exists.");
            }

            return Ok(customer);
        }


        [HttpPut]
        [Route("/update-customer")]
        public async Task<IActionResult> UpdateCustomer(long id, [FromBody] CustomerValueObject customerChange)
        {
            //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("update-customer");
            //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(Updatecustomer)), new("Controller", nameof(customerController)));

            var customer = await this.repository.UpdateAsync(id, customerChange, CancellationToken.None).ConfigureAwait(false);

            if (customer is null)
            {
                return BadRequest($"customer {id} not exists.");
            }

            return Ok(customer);
        }
    }
}