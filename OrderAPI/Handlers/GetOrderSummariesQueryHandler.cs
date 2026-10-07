using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Dtos;

namespace OrderAPI.Handlers
{
    //public class GetOrderSummariesQueryHandler : IQueryHandler<GetOrderSummariesQuery, List<OrderSummaryDto>>
    public class GetOrderSummariesQueryHandler : IRequestHandler<GetOrderSummariesQuery, List<OrderSummaryDto>>
    {
        public readonly ReadDbContext _context;
        //public readonly AppDbContext _context;

        //public GetOrderSummariesQueryHandler(AppDbContext context)
        //{
        //    _context = context;
        //}

        public GetOrderSummariesQueryHandler(ReadDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderSummaryDto>> Handle(GetOrderSummariesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .AsNoTracking()
                .Select(o => new OrderSummaryDto(
                    o.Id,
                    o.FirstName + " " + o.LastName,
                    o.Status,
                    o.TotalCost
                )).ToListAsync(cancellationToken);
        }

        public async Task<List<OrderSummaryDto>?> HandleAsync(GetOrderSummariesQuery query)
        {
            return await _context.Orders
                .AsNoTracking()
                .Select(o => new OrderSummaryDto(
                    o.Id,
                    o.FirstName + " " + o.LastName,
                    o.Status,
                    o.TotalCost
                )).ToListAsync();
        }
    }
}
