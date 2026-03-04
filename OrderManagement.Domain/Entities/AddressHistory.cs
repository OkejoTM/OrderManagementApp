using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities;

public class AddressHistory
{
    public Guid Id { get; private set; }
    public DateOnly PumpingDate { get; private set; }
    public int CubeAmount { get; private set; }
    public PaymentType PaymentType { get; private set; }
    public double Price { get; private set; }

    public Guid AddressId { get; private set; }
    public Address Address { get; private set; } = null!;

    private AddressHistory() { }

    public AddressHistory(Guid addressId, DateOnly pumpingDate, int cubeAmount, PaymentType paymentType, double price)
    {
        Id = Guid.NewGuid();
        AddressId = addressId;
        PumpingDate = pumpingDate;
        CubeAmount = cubeAmount;
        PaymentType = paymentType;
        Price = price;
    }

    public void Update(DateOnly pumpingDate, int cubeAmount, PaymentType paymentType, double price)
    {
        PumpingDate = pumpingDate;
        CubeAmount = cubeAmount;
        PaymentType = paymentType;
        Price = price;
    }
}