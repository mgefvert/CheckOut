/// <summary>
/// Define a class or record called Product which contains a ProductCode, ProductName, and a Unit Price.
/// </summary>
public class Product
{
    public Product(string code, string name, decimal price)
    {
        Code = code;
        Name = name;
        Price = price;
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}