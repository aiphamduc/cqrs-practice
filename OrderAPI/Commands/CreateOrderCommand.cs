using MediatR;
using OrderAPI.Dtos;

public record CreateOrderCommand(string FirstName, string LastName, string Status, decimal TotalCost) : IRequest<OrderDto>;
