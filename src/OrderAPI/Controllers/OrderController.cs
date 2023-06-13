using Domain.ValuesObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using Services.Contracts;

namespace OrderAPI.Controllers;

public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> logger;
    private readonly IOrderRepository repository;

    public OrderController(
        ILogger<OrderController> logger,
        IOrderRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    [HttpGet]
    [Route("/get-all-orders")]
    public async Task<IActionResult> GetAllAsync(int? page = 1, int? perPage = 10)
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("GetAllAsync");
        try
        {
            DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetAllAsync)), new("Controller", nameof(OrderController)));
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


    [HttpPost]
    [Route("/register-order")]
    public async Task<IActionResult> RegisterNewOrderAsync([FromBody] OrderValueObject orderToRegister)
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("RegisterNewOrderAsync");
        try
        {
            DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(RegisterNewOrderAsync)), new("Controller", nameof(OrderController)));
            activity!.SetTag("RegisterNewOrderAsync", JsonConvert.SerializeObject(orderToRegister));

            var result = await this.repository.RegisterNewOrder(orderToRegister, CancellationToken.None).ConfigureAwait(false);

            return Ok(result);
        }
        catch (Exception ex)
        {
            activity!.RecordException(ex);

            return BadRequest(ex.Message);
        }
    }
}
