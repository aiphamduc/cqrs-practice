using MediatR;
using OrderAPI.Dtos;

public record GetOrderSummariesQuery() : IRequest<List<OrderSummaryDto>>;