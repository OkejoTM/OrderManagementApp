using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.DTOs;

public class AddressHistoryDto
{
    public Guid Id { get; set; }
    public DateOnly PumpingDate { get; set; }
    public double CubeAmount { get; set; }
    public PaymentType PaymentType { get; set; }
    public double Price { get; set; }
    public Guid AddressId { get; set; }
}