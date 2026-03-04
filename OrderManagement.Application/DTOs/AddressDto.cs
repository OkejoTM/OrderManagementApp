namespace OrderManagement.Application.DTOs;

public class AddressDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AddressHistoryDto? LastOrder { get; set; }
}