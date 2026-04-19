namespace OrderManagement.Application.Common.Exceptions;

public class DuplicateAddressException(string name)
    : Exception($"Адрес \"{name}\" уже существует в этом районе.")
{
    public string Name { get; } = name;
}