namespace OrderManagement.Domain.Entities;

public class Area
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private readonly List<Address> _addresses = [];
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    private Area() { }

    public Area(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}