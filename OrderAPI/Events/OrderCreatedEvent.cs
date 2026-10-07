using MediatR;

namespace OrderAPI.Events;
    public record OrderCreatedEvent
(
    int OrderId,
    string FirstName,
    string LastName,
    decimal TotalCost
) : INotification;
