namespace Lab8Mvvm.Models;

public abstract class BusinessObject
{
    public string Owner { get; protected set; } = string.Empty;
    public abstract uint Password { get; }
}
