using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Commands;
using OrderAPI.Data;
using OrderAPI.Dtos;
using OrderAPI.Events;
using OrderAPI.Models;

namespace OrderAPI.Handlers
{
    //public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto?>
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto?>
    {
        private readonly WriteDbContext _context;
        private readonly IValidator<CreateOrderCommand> _validator;
        //private readonly IEventPublisher _eventPublisher;
        private readonly IMediator _mediator;
        //IEventPublisher eventPublisher,
        public CreateOrderCommandHandler(WriteDbContext context, IValidator<CreateOrderCommand> validator, 
            IMediator mediator)
        {
            _context = context;
            _validator = validator;
            //_eventPublisher = eventPublisher;
            _mediator = mediator;
        }

        //public static async Task<Order?> Handle(CreateOrderCommand request, AppDbContext context)
        //{
        //    var order = new Order
        //    {
        //        FirstName = request.FirstName,
        //        LastName = request.LastName,
        //        Status = request.Status,
        //        CreatedAt = DateTime.Now,
        //        TotalCost = request.TotalCost
        //    };
        //    await context.Orders.AddAsync(order);
        //    await context.SaveChangesAsync();
        //    return order;
        //}

        //public async Task<OrderDto?> HandleAsync(CreateOrderCommand command)
        //{
        //    var validationResult = await _validator.ValidateAsync(command);
        //    if (!validationResult.IsValid)
        //    {
        //        throw new FluentValidation.ValidationException(validationResult.Errors);
        //    }

        //    var order = new Order
        //    {
        //        FirstName = command.FirstName,
        //        LastName = command.LastName,
        //        Status = command.Status,
        //        CreatedAt = DateTime.Now,
        //        TotalCost = command.TotalCost
        //    };
        //    await _context.Orders.AddAsync(order);
        //    await _context.SaveChangesAsync();

        //    var emittedEvent = new OrderCreatedEvent
        //    (
        //        order.Id,
        //        order.FirstName,
        //        order.LastName,
        //        order.TotalCost
        //    );

        //    await _eventPublisher.PublishAsync(emittedEvent);

        //    return new OrderDto
        //    (
        //        order.Id,
        //        order.FirstName,
        //        order.LastName,
        //        order.Status,
        //        order.CreatedAt,
        //        order.TotalCost
        //    );
        //}

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var order = new Order
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Status = request.Status,
                CreatedAt = DateTime.Now,
                TotalCost = request.TotalCost
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var orderCreatedEvent = new OrderCreatedEvent
            (
                order.Id,
                order.FirstName,
                order.LastName,
                order.TotalCost
            );

            //await _eventPublisher.PublishAsync(orderCreatedEvent);

            await _mediator.Publish(orderCreatedEvent);

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
