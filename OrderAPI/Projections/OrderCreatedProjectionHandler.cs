using MediatR;
using OrderAPI.Data;
using OrderAPI.Events;
using OrderAPI.Models;

namespace OrderAPI.Projections
{
    //public class OrderCreatedProjectionHandler : IEventHandler<OrderCreatedEvent>
    public class OrderCreatedProjectionHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly ReadDbContext _context;

        public OrderCreatedProjectionHandler(ReadDbContext context)
        {
            _context = context;
        }


        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Id = notification.OrderId,
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                Status = "Created",
                CreatedAt = DateTime.Now,
                TotalCost = notification.TotalCost
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task HandleAsync(OrderCreatedEvent evt)
        {
           var order = new Order
           {
               Id = evt.OrderId,
               FirstName = evt.FirstName,
               LastName = evt.LastName,
               Status = "Created",
               CreatedAt = DateTime.Now,
               TotalCost = evt.TotalCost
           };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}
