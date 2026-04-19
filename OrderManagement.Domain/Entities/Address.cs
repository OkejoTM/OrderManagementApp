using System.Text.RegularExpressions;

namespace OrderManagement.Domain.Entities;

public class Address
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;

    public Guid AreaId { get; private set; }
    public Area Area { get; private set; } = null!;

    private readonly List<AddressHistory> _histories = [];
    public IReadOnlyCollection<AddressHistory> Histories => _histories.AsReadOnly();

    private Address() { }

    public Address(Guid areaId, string name)
    {
        Id = Guid.NewGuid();
        AreaId = areaId;
        SetName(name);
    }

    public void UpdateName(string name)
    {
        SetName(name);
    }

    public void AddHistory(AddressHistory history)
    {
        _histories.Add(history);
    }

    public void RemoveHistory(AddressHistory history)
    {
        _histories.Remove(history);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Address name cannot be empty.", nameof(name));

        Name = CollapseSpaces(name.Trim());
        NormalizedName = Name.ToLowerInvariant();
    }

    public static string Normalize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        return CollapseSpaces(name.Trim()).ToLowerInvariant();
    }

    private static string CollapseSpaces(string value)
    {
        return Regex.Replace(value, @"\s+", " ");
    }
}