using Microsoft.AspNetCore.Mvc;
using UsingOpentelemetry.Models;

namespace UsingOpentelemetry.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    // private readonly PoCDbContext? db;

    //public ProductController(PoCDbContext context)
    //{
    //    this.db = context;
    //}

    [HttpGet]
    [Route("/get-all-products")]
    public async Task<IActionResult> GetAllAsync(int? page = 1, int? perPage = 10)
    {
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("get-all-products");

        //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetAllAsync)), new("Controller", nameof(ProductController)));

        //if (!page.HasValue || page.HasValue && page.Value < 1)
        //{
        //    page = 1;
        //}

        //if (!perPage.HasValue || perPage.HasValue && perPage.Value < 1)
        //{
        //    page = 10;
        //}

        //var result = await this.db!.Products!.Take((int)perPage!).Skip(((int)page! - 1) * (int)perPage!).OrderByDescending(x => x.CreatedAt).ToArrayAsync(CancellationToken.None).ConfigureAwait(false);

        //return Ok(result);
        throw new NotImplementedException();
    }


    [HttpGet]
    [Route("/get-by-id")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("get-by-id");

        //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByIdAsync)), new("Controller", nameof(ProductController)));

        //var result = await this.db!.Products!.Where(p => p.Id == id).FirstOrDefaultAsync(CancellationToken.None).ConfigureAwait(false);

        //if (result == null)
        //{
        //    return NotFound();
        //}

        //return Ok(result);
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("/get-by-code-or-ean")]
    public async Task<IActionResult> GetByCodeOrEanAsync(string target)
    {
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("get-by-code-or-ean");

        //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(GetByCodeOrEanAsync)), new("Controller", nameof(ProductController)));

        //if (string.IsNullOrEmpty(target))
        //{
        //    return BadRequest("Não foi fornecido um valor válido para pesquisa.");
        //}

        //var result = await this.db!.Products!.Where(p => p.Code == target || p.Ean == target).FirstOrDefaultAsync(CancellationToken.None).ConfigureAwait(false);

        //if (result == null)
        //{
        //    return NotFound();
        //}

        //return Ok(result);
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("/register-new-product")]
    public async Task<IActionResult> RegisterProduct([FromBody] ProductCreateDto productCreateDto)
    {
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("register-new-product");

        //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(RegisterProduct)), new("Controller", nameof(ProductController)));

        //var product = await this.db!.Products!.Where(x => x.Ean == productCreateDto.Ean || x.Code == productCreateDto.Code).FirstOrDefaultAsync(CancellationToken.None).ConfigureAwait(false);

        //if (product is not null)
        //{
        //    return BadRequest($"Product {productCreateDto.Code} exists.");
        //}

        //product = new Database.Entities.Product
        //{
        //    Code = productCreateDto.Code,
        //    Ean = productCreateDto.Ean,
        //    Description = productCreateDto.Description,
        //    CreatedAt = DateTime.Now,
        //    UpdatedAt = DateTime.Now,
        //};


        //await this.db!.Products.AddAsync(product, CancellationToken.None).ConfigureAwait(false);
        //await this.db!.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);


        //return Ok(product);
        throw new NotImplementedException();
    }


    [HttpPut]
    [Route("/update-product")]
    public async Task<IActionResult> UpdateProduct(long id, [FromBody] ProductChangeDto productChangeDto)
    {
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("update-product");

        //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(UpdateProduct)), new("Controller", nameof(ProductController)));

        //var product = await this.db!.Products!.Where(x => x.Id == id).FirstOrDefaultAsync(CancellationToken.None).ConfigureAwait(false);

        //if (product is null)
        //{
        //    return BadRequest($"Product {id} not exists.");
        //}

        //var exists = await this.db!.Products!.AnyAsync(x => x.Ean == productChangeDto.Ean && x.Id != id, CancellationToken.None).ConfigureAwait(false);
        //if (exists)
        //{
        //    return BadRequest($"Has Product with EAN {productChangeDto.Ean} registered.");
        //}

        //if (!(product.Ean ?? string.Empty).Equals(productChangeDto.Ean, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(productChangeDto.Ean))
        //{
        //    product.Ean = productChangeDto.Ean;
        //}

        //if (!(product.Description ?? string.Empty).Equals(productChangeDto.Description, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(productChangeDto.Description))
        //{
        //    product.Description = productChangeDto.Description;
        //}

        //product.UpdatedAt = DateTime.Now;

        //await this.db!.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);

        //return Ok(product);
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("/remove-by-id")]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("remove-by-id");

        //DiagnosticsConfig.RequestCounter.Add(1, new("Action", nameof(DeleteProduct)), new("Controller", nameof(ProductController)));

        //var product = await this.db!.Products!.Where(x => x.Id == id).FirstOrDefaultAsync(CancellationToken.None).ConfigureAwait(false);

        //if (product is null)
        //{
        //    return BadRequest($"Product {id} not exists.");
        //}

        //this.db!.Products!.Remove(product);

        //await this.db!.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);

        //return Ok(product);
        throw new NotImplementedException();
    }


}
