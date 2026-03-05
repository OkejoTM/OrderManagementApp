using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Mapping;

public static class ManualMapper
{
    public static AreaDto ToDto(Area area)
    {
        return new AreaDto
        {
            Id = area.Id,
            Name = area.Name
        };
    }

    public static AddressDto ToDto(Address address)
    {
        var lastOrder = address.Histories
            .OrderByDescending(h => h.PumpingDate)
            .FirstOrDefault();

        return new AddressDto
        {
            Id = address.Id,
            Name = address.Name,
            LastOrder = lastOrder is not null ? ToDto(lastOrder) : null
        };
    }

    public static AddressHistoryDto ToDto(AddressHistory history)
    {
        return new AddressHistoryDto
        {
            Id = history.Id,
            PumpingDate = history.PumpingDate,
            CubeAmount = history.CubeAmount,
            PaymentType = history.PaymentType,
            Price = history.Price,
            AddressId = history.AddressId
        };
    }

    public static IReadOnlyList<AreaDto> ToDtoList(IEnumerable<Area> areas)
    {
        return areas.Select(ToDto).ToList().AsReadOnly();
    }

    public static IReadOnlyList<AddressDto> ToDtoList(IEnumerable<Address> addresses)
    {
        return addresses.Select(ToDto).ToList().AsReadOnly();
    }

    public static IReadOnlyList<AddressHistoryDto> ToDtoList(IEnumerable<AddressHistory> histories)
    {
        return histories.Select(ToDto).ToList().AsReadOnly();
    }
}