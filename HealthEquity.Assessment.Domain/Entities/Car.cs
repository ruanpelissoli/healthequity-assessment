namespace HealthEquity.Assessment.Domain.Entities;
public class Car : BaseEntity, IAggregateRoot
{
    public Car(string make, string model, int year, int doors, string color, decimal price)
    {
        Make = make;
        Model = model;
        Year = year;
        Doors = doors;
        Color = color;
        Price = price;
    }

    public string Make { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public int Doors { get; private set; }
    public string Color { get; private set; }
    public decimal Price { get; private set; }


    public void SetId(long id) { Id = id; }
    public void SetMake(string make) { Make = make; }
    public void SetModel(string model) { Model = model; }
    public void SetYear(int year) { Year = year; }
    public void SetDoors(int doors) { Doors = doors; }
    public void SetColor(string color) { Color = color; }
    public void SetPrice(decimal price) { Price = price; }
}
