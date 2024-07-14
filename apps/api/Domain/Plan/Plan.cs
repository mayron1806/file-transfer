namespace Domain.Plan;

public class Plan(string name, string? description, decimal price, Limits limits)
{
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public decimal Price { get; } = price;
    public Limits Limits { get; } = limits;
}