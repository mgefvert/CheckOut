/// <summary>
/// Define a "class", or "record", called Cart which contains a ProductCode and a unit count.
/// </summary>
public class Cart
{
    public Cart(string code, int units)
    {
        Code = code;
        Count = units;
    }

    public string Code { get; set; }
    public int Count { get; set; }
}