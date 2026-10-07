using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Commands;
using OrderAPI.Data;
using OrderAPI.Dtos;
using OrderAPI.Events;
using OrderAPI.Handlers;
using OrderAPI.Models;
using OrderAPI.Projections;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("BaseConnection")));
builder.Services.AddDbContext<ReadDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ReadDbConnection")));
builder.Services.AddDbContext<WriteDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("WriteDbConnection")));

//builder.Services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
//builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto>, GetOrderByIdQueryHandler>();
//builder.Services.AddScoped<IQueryHandler<GetOrderSummariesQuery, List<OrderSummaryDto>>, GetOrderSummariesQueryHandler>();
builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
//builder.Services.AddSingleton<IEventPublisher, ConsoleEventPublisher>();
//builder.Services.AddSingleton<IEventPublisher, InProcessEventPublisher>();
//builder.Services.AddScoped<IEventHandler<OrderCreatedEvent>, OrderCreatedProjectionHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());


var app = builder.Build();

//app.MapPost("/api/orders", async (ICommandHandler<CreateOrderCommand, OrderDto> handler, CreateOrderCommand order) =>
app.MapPost("/api/orders", async (IMediator mediator, CreateOrderCommand order) =>
{
    try
    {
        //var createdOrder = await handler.HandleAsync(order);
        var createdOrder = await mediator.Send(order);
        if (createdOrder == null)
            return Results.BadRequest("Failed to create order");

        return Results.Created($"/api/orders{createdOrder.Id}", createdOrder);
    }
    catch (ValidationException ex)
    {
        var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

        return Results.BadRequest(errors);
    }

    //await context.Orders.AddAsync(order);
    //await context.SaveChangesAsync();
    
   
    //try
    //{
    //    var createdOrder = await mediator.Send(command);
    //    if (createdOrder == null)
    //    {
    //        return Results.BadRequest("Failed to create order");
    //    }

    //    return Results.Created($"/api/orders{createdOrder.Id}", createdOrder);
    //}
    //catch (ValidationException ex)
    //{
    //    var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
    //    return Results.BadRequest(errors);
    //}
});

//app.MapGet("/api/orders/{id}", async (IQueryHandler<GetOrderByIdQuery, OrderDto> handler, int id) =>
app.MapGet("/api/orders/{id}", async (IMediator mediator, int id) =>
{
    //var order = await handler.HandleAsync(new GetOrderByIdQuery(id));
    var order = await mediator.Send(new GetOrderByIdQuery(id));
    if (order == null)
        return Results.NotFound();

    return Results.Ok(order);
});

//app.MapGet("/api/orders", async (IQueryHandler<GetOrderSummariesQuery, List<OrderSummaryDto>> handler) =>
app.MapGet("/api/orders", async (IMediator mediator) =>
{
    //var summaries = await handler.HandleAsync(new GetOrderSummariesQuery());
    var summaries = await mediator.Send(new GetOrderSummariesQuery());
    return Results.Ok(summaries);
});

app.Run();
