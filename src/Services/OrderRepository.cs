using AutoMapper;
using Database.Contexts;
using Domain.Entities;
using Domain.ValuesObjects;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class OrderRepository
{
    private readonly PoCDbContext db;
    private readonly IMapper mapper;

    public OrderRepository(PoCDbContext dbContext, IMapper mapper)
    {
        this.db = dbContext;
        this.mapper = mapper;
    }

    public async Task<OrderValueObject?> RegisterAsync(OrderValueObject order, CancellationToken cancellationToken)
    {
        var orderToSave = mapper.Map<Order>(order);

        using var tx = await db.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            orderToSave.CustomerId = order.Customer!.Id;
            orderToSave.CreatedAt = DateTime.Now;
            orderToSave.UpdatedAt = DateTime.Now;

            db.Add(orderToSave);

            foreach (var item in order.Items!.ToArray())
            {
                var orderItemToSave = mapper.Map<OrderItem>(item);

                orderItemToSave.ProductId = item.Product!.Id;

                orderItemToSave.OrderId = orderToSave.Id;
                orderItemToSave.Order = orderToSave;

                orderItemToSave.CreatedAt = DateTime.Now;
                orderItemToSave.UpdatedAt = DateTime.Now;

                db.Add(orderItemToSave);
            }

            await tx.CommitAsync(cancellationToken).ConfigureAwait(false);

            return mapper.Map<OrderValueObject>(order);
        }
        catch (Exception ex)
        {
            _ = ex;
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);

            return default;
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

}
