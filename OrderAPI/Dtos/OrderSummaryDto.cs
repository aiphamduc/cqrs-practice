namespace OrderAPI.Dtos;

public record OrderSummaryDto
(
int OrderId,
string CustomerName,
string Status,
Decimal TotalCost
);

