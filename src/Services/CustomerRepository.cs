using AutoMapper;
using Database.Contexts;
using Domain.Entities;
using Domain.ValuesObjects;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace Services;

public class CustomerRepository : ICustomerRepository
{
    private readonly PoCDbContext db;
    private readonly IMapper mapper;

    public CustomerRepository(PoCDbContext dbContext, IMapper mapper)
    {
        this.db = dbContext;
        this.mapper = mapper;
    }

    public async Task<CustomerValueObject?> RegisterAsync(CustomerValueObject customerValueObject, CancellationToken cancellationToken)
    {
        var customer = await this.FindByDocumentAsync(customerValueObject.Document!, CancellationToken.None).ConfigureAwait(false);

        if (customer == null)
        {
            var customerToSave = this.mapper.Map<Customer>(customerValueObject);

            customerToSave.CreatedAt = DateTime.Now;
            customerToSave.UpdatedAt = DateTime.Now;

            db.Add(customerToSave);

            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return this.mapper.Map<CustomerValueObject>(customerToSave);
        }

        return this.mapper.Map<CustomerValueObject>(customer);
    }

    public async Task<CustomerValueObject?> UpdateAsync(long id, CustomerValueObject customerValueObject, CancellationToken cancellationToken)
    {
        var customer = await this.FindByIdAsync(id, CancellationToken.None).ConfigureAwait(false);

        if (customer != null)
        {
            customer.Document = customerValueObject.Document ?? customer.Document;
            customer.Name = customerValueObject.Name ?? customer.Name;
            customer.Email = customerValueObject.Email ?? customer.Email;
            customer.Phone = customerValueObject.Phone ?? customer.Phone;

            customer.UpdatedAt = DateTime.Now;

            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return mapper.Map<CustomerValueObject>(customer);
        }

        return default;
    }

    public async Task<ICollection<CustomerValueObject>?> ListAsync(int currentPage, int perPage, CancellationToken cancellationToken)
    {
        if (currentPage < 0)
        {
            currentPage = 1;
        }

        if (perPage < 5 || perPage > 100)
        {
            currentPage = 10;
        }

        var dataResult = await db.Customers.OrderBy(c => c.Name).Skip((currentPage - 1) * perPage).Take(perPage).ToListAsync(cancellationToken).ConfigureAwait(false);

        var result = this.mapper.Map<ICollection<CustomerValueObject>>(dataResult);

        return result ?? Array.Empty<CustomerValueObject>();
    }

    public async Task<CustomerValueObject?> FindByIdAsync(long id, CancellationToken cancellationToken)
    {
        var customer = await db.Customers.Where(c => c.Id == id).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (customer == null)
        {
            return default;
        }

        return this.mapper.Map<CustomerValueObject>(customer);
    }

    public async Task<CustomerValueObject?> FindByDocumentAsync(string document, CancellationToken cancellationToken)
    {
        var customer = await db.Customers.Where(c => c.Document == document).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (customer == null)
        {
            return default;
        }

        return this.mapper.Map<CustomerValueObject>(customer);
    }
}
