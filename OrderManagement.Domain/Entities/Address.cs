namespace OrderManagement.Domain.Entities;

public class Address
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public Guid AreaId { get; private set; }
    public Area Area { get; private set; } = null!;

    private readonly List<AddressHistory> _histories = [];
    public IReadOnlyCollection<AddressHistory> Histories => _histories.AsReadOnly();

    private Address() { }

    public Address(Guid areaId, string name)
    {
        Id = Guid.NewGuid();
        AreaId = areaId;
        Name = name;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void AddHistory(AddressHistory history)
    {
        _histories.Add(history);
    }

    public void RemoveHistory(AddressHistory history)
    {
        _histories.Remove(history);
    }
}