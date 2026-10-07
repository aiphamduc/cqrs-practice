using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Dtos;
using OrderAPI.Models;

namespace OrderAPI.Handlers
{
    //public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto?>
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly ReadDbContext _context;
        //private readonly AppDbContext _context;

        public GetOrderByIdQueryHandler(ReadDbContext context)
        {
            _context = context;
        }

        //public GetOrderByIdQueryHandler(AppDbContext context)
        //{
        //    _context = context;
        //}

        //public static async Task<Order?> Handle(GetOrderByIdQuery request, AppDbContext context)
        //{
        //    return await context.Orders
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(o => o.Id == request.OrderId);
        //}

        public async Task<OrderDto?> HandleAsync(GetOrderByIdQuery query)
        {
            var order = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(o => o.Id == query.OrderId);

            if (order == null)
                return null;

            return new OrderDto(order.Id, order.FirstName, order.LastName, order.Status, order.CreatedAt, order.TotalCost);
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order == null)
                return null;

            return new OrderDto(
                order.Id,
                order.FirstName,
                order.LastName,
                order.Status,
                order.CreatedAt,
                order.TotalCost
            );
        }
    }
}
