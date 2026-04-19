using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities;

public class AddressHistory
{
    public Guid Id { get; private set; }
    public DateOnly PumpingDate { get; private set; }
    public double CubeAmount { get; private set; }
    public PaymentType PaymentType { get; private set; }
    public double Price { get; private set; }
    public string? PumpedBy { get; private set; }

    public Guid AddressId { get; private set; }
    public Address Address { get; private set; } = null!;

    private AddressHistory() { }

    public AddressHistory(
        Guid addressId,
        DateOnly pumpingDate,
        double cubeAmount,
        PaymentType paymentType,
        double price,
        string? pumpedBy)
    {
        Id = Guid.NewGuid();
        AddressId = addressId;
        PumpingDate = pumpingDate;
        CubeAmount = cubeAmount;
        PaymentType = paymentType;
        Price = price;
        PumpedBy = NormalizePumpedBy(pumpedBy);
    }

    public void Update(
        DateOnly pumpingDate,
        double cubeAmount,
        PaymentType paymentType,
        double price,
        string? pumpedBy)
    {
        PumpingDate = pumpingDate;
        CubeAmount = cubeAmount;
        PaymentType = paymentType;
        Price = price;
        PumpedBy = NormalizePumpedBy(pumpedBy);
    }

    private static string? NormalizePumpedBy(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var trimmed = value.Trim();
        return trimmed.Length == 0 ? null : trimmed;
    }
}