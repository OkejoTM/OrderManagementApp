namespace OrderManagement.Application.DTOs;

public class AddressSearchResultDto
{
    public Guid AddressId { get; set; }
    public string AddressName { get; set; } = string.Empty;
    public Guid AreaId { get; set; }
    public string AreaName { get; set; } = string.Empty;
    public AddressHistoryDto? LastOrder { get; set; }
}