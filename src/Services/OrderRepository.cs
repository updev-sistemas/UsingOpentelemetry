using AutoMapper;
using Database.Contexts;
using Domain.Entities;
using Domain.ValuesObjects;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.HttpClient;

namespace Services;

public class OrderRepository : IOrderRepository
{
    private readonly PoCDbContext db;
    private readonly IMapper mapper;
    private readonly IProductApi productApi;
    private readonly ICustomerApi customerApi;

    public OrderRepository(
        PoCDbContext dbContext, 
        IMapper mapper,
        IProductApi productApi,
        ICustomerApi customerApi)
    {
        this.db = dbContext;
        this.mapper = mapper;
        this.productApi = productApi;
        this.customerApi = customerApi;
    }

    public async Task<OrderValueObject?> RegisterAsync(OrderValueObject order, CancellationToken cancellationToken)
    {
        using var tx = await db.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var orderToSave = mapper.Map<Order>(order);

            orderToSave.Customer = null;
            
            var items = orderToSave.Items;
            orderToSave.Items = null;

            db.Add(orderToSave);
            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            foreach (var item in items!)
            {
                item.Product = null;
                item.Order = null;
                item.OrderId = orderToSave.Id;

                db.Add(item);
            }

            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await tx.CommitAsync(cancellationToken).ConfigureAwait(false);

            order.Id = orderToSave.Id;

            return mapper.Map<OrderValueObject>(order);
        }
        catch (Exception ex)
        {
            _ = ex;
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);

            throw ex;
        }
    }

    public async Task<ICollection<OrderValueObject>?> ListAsync(int currentPage, int perPage, CancellationToken cancellationToken)
    {
        if (currentPage < 0)
        {
            currentPage = 1;
        }

        if (perPage < 5 || perPage > 100)
        {
            currentPage = 10;
        }

        var dataResult = await db.Orders.OrderByDescending(c => c.CreatedAt!).Skip((currentPage - 1) * perPage).Take(perPage).ToListAsync(cancellationToken).ConfigureAwait(false);

        var result = this.mapper.Map<ICollection<OrderValueObject>>(dataResult);

        return result ?? Array.Empty<OrderValueObject>();
    }

    public async Task<OrderValueObject?> RegisterNewOrder(OrderValueObject order, CancellationToken cancellationToken)
    {
        CustomerValueObject? customer;

        try
        {
            customer = await this.customerApi.GetByDocumentAsync(order!.Customer!.Document!).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _ = ex;
            customer = default;
        }

        if (customer is null)
        {
            customer = await this.customerApi.RegisterCustomerAsync(order!.Customer!).ConfigureAwait(false);

            if (customer == null)
                throw new Exception("Não foi possível registrar o cliente.");
        }

        order.Customer = customer;
        order.CreatedAt = DateTime.Now;
        order.UpdatedAt = DateTime.Now;

        foreach (var item in order!.Items!.ToArray())
        {
            var product = await this.productApi.GetByCodeAsync(item.Product!.Code!).ConfigureAwait(false);

            if (product == null)
                throw new Exception($"Não foi possível recuperar o Produto {item.Product!.Code} - {item.Product!.Description}.");

            item.Product = product;
            item.CreatedAt = DateTime.Now;
            item.UpdatedAt = DateTime.Now;
        }

        var result = await this.RegisterAsync(order, cancellationToken).ConfigureAwait(false);

        if (result == null)
            return default;

        var orderRegistered = await this.db.Orders.Where(o => o.Id == result.Id).Include(p => p.Items).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (orderRegistered == null)
            return default;

        return mapper.Map<OrderValueObject>(orderRegistered);
    }

}
