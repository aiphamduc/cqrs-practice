using MediatR;
using OrderAPI.Dtos;

public record GetOrderByIdQuery(int OrderId) : IRequest<OrderDto?>;