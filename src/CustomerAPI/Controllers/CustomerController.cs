using Domain.ValuesObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using Services.Contracts;
using System.Diagnostics;

namespace CustomerAPI.Controllers;

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
        using var activity = LocalDynatrace.MyActivitySource.StartActivity("GetAllAsync", ActivityKind.Producer);
        try
        {
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetAllAsync)), new("Controller", nameof(CustomerController)));
            activity!.SetTag("GetAllAsync", $"Search all products from {page} with quantity {perPage}");

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
        catch (Exception ex)
        {
            activity!.RecordException(ex);

            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    [Route("/get-by-id")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        using var activity = LocalDynatrace.MyActivitySource.StartActivity("GetByIdAsync", ActivityKind.Producer);
        try
        {
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByIdAsync)), new("Controller", nameof(CustomerController)));
            activity!.SetTag("GetByIdAsync", $"Search Customer By Id {id}");

            var result = await this.repository.FindByIdAsync(id, CancellationToken.None).ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            activity!.RecordException(ex);

            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("/get-by-document")]
    public async Task<IActionResult> GetByDocumentAsync(string target)
    {
        using var activity = LocalDynatrace.MyActivitySource.StartActivity("GetByDocumentAsync", ActivityKind.Producer);
        try
        {
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByDocumentAsync)), new("Controller", nameof(CustomerController)));
            activity!.SetTag("GetByDocumentAsync", $"Search Customer By Document {target}");

            var result = await this.repository.FindByDocumentAsync(target, CancellationToken.None).ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            activity!.RecordException(ex);

            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("/register-new-customer")]
    public async Task<IActionResult> Registercustomer([FromBody] CustomerValueObject customerCreate)
    {
        using var activity = LocalDynatrace.MyActivitySource.StartActivity("Registercustomer", ActivityKind.Consumer);
        try
        {
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(Registercustomer)), new("Controller", nameof(CustomerController)));
            activity!.SetTag("Registercustomer", JsonConvert.SerializeObject(customerCreate));

            var customer = await this.repository.RegisterAsync(customerCreate, CancellationToken.None).ConfigureAwait(false);

            if (customer is null)
            {
                return BadRequest($"customer {customerCreate.Document} exists.");
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            activity!.RecordException(ex);

            return BadRequest(ex.Message);
        }
    }


    [HttpPut]
    [Route("/update-customer")]
    public async Task<IActionResult> UpdateCustomer(long id, [FromBody] CustomerValueObject customerChange)
    {
        using var activity = LocalDynatrace.MyActivitySource.StartActivity("UpdateCustomer", ActivityKind.Consumer);
        try
        {
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(UpdateCustomer)), new("Controller", nameof(CustomerController)));
            activity!.SetTag("UpdateCustomer", $"Update Customer {id} with {JsonConvert.SerializeObject(customerChange)}");

            var customer = await this.repository.UpdateAsync(id, customerChange, CancellationToken.None).ConfigureAwait(false);

            if (customer is null)
            {
                return BadRequest($"customer {id} not exists.");
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            activity!.RecordException(ex);

            return BadRequest(ex.Message);
        }
    }
}