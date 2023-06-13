using AutoMapper;
using Database.Contexts;
using Domain.Entities;
using Domain.ValuesObjects;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace Services;

public class ProductRepository : IProductRepository
{
    private readonly PoCDbContext db;
    private readonly IMapper mapper;

    public ProductRepository(PoCDbContext dbContext, IMapper mapper)
    {
        this.db = dbContext;
        this.mapper = mapper;
    }

    public async Task<ProductValueObject?> RegisterAsync(ProductValueObject productValueObject, CancellationToken cancellationToken)
    {
        var product = this.mapper.Map<Product>(productValueObject);

        if (product != null)
        {
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            db.Add(product);

            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return this.mapper.Map<ProductValueObject>(product);
        }

        return default;
    }

    public async Task<ProductValueObject?> UpdateAsync(long id, ProductValueObject productValueObject, CancellationToken cancellationToken)
    {
        var product = await db.Products.Where(c => c.Id == id).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (product != null)
        {
            product.Code = productValueObject.Code;
            product.Description = productValueObject.Description;
            product.Price = productValueObject.Price;

            product.UpdatedAt = DateTime.Now;

            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        return default;
    }

    public async Task<ICollection<ProductValueObject>?> ListAsync(int currentPage, int perPage, CancellationToken cancellationToken)
    {
        if (currentPage < 0)
        {
            currentPage = 1;
        }

        if (perPage < 5 || perPage > 100)
        {
            currentPage = 10;
        }

        var dataResult = await db.Products.OrderBy(c => c.Description).Skip((currentPage - 1) * perPage).Take(perPage).ToListAsync(cancellationToken).ConfigureAwait(false);

        var result = this.mapper.Map<ICollection<ProductValueObject>>(dataResult);

        return result ?? Array.Empty<ProductValueObject>();
    }

    public async Task<ProductValueObject?> FindByIdAsync(long id, CancellationToken cancellationToken)
    {
        var product = await db.Products.Where(c => c.Id == id).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (product == null)
        {
            return default;
        }

        return this.mapper.Map<ProductValueObject>(product);
    }

    public async Task<ProductValueObject?> FindByCodeAsync(string code, CancellationToken cancellationToken)
    {
        var product = await db.Products.Where(c => c.Code == code).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (product == null)
        {
            return default;
        }

        return this.mapper.Map<ProductValueObject>(product);
    }
}
