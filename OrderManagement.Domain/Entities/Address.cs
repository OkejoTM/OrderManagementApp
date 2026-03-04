namespace OrderManagement.Domain.Entities;

public class Address
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private readonly List<AddressHistory> _histories = [];
    public IReadOnlyCollection<AddressHistory> Histories => _histories.AsReadOnly();

    private Address() { }

    public Address(string name)
    {
        Id = Guid.NewGuid();
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