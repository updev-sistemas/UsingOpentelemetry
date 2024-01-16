using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using Services.Contracts;
using System.Diagnostics;

namespace ProductAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> logger;
    private readonly IProductRepository repository;

    public ProductController(
        ILogger<ProductController> logger,
        IProductRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    [HttpGet]
    [Route("/get-all-products")]
    public async Task<IActionResult> GetAllAsync(int? page = 1, int? perPage = 10)
    {
        using var activity = LocalDynatrace.MyActivitySource.StartActivity("GetAllAsync", ActivityKind.Producer);
        try
        {
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetAllAsync)), new("Controller", nameof(ProductController)));
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
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByIdAsync)), new("Controller", nameof(ProductController)));
            activity!.SetTag("GetByIdAsync", $"Search Product ID {id}");

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
    [Route("/get-by-code")]
    public async Task<IActionResult> GetByCodeAsync(string target)
    {
        using var activity = LocalDynatrace.MyActivitySource.StartActivity("GetByCodeAsync", ActivityKind.Producer);
        try
        {
            // DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByCodeAsync)), new("Controller", nameof(ProductController)));
            activity!.SetTag("GetByCodeAsync", $"Search Product Code {target}");

            var result = await this.repository.FindByCodeAsync(target, CancellationToken.None).ConfigureAwait(false);

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
}