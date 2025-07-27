using Newtonsoft.Json;

namespace CheckOut;

public class ProductDatabase
{
    public List<Product> Products { get; set; }

    public ProductDatabase()
    {
        Products = new List<Product>()
        {
            new Product("BOT10", "Shortbot", 34.99m),
            new Product("BOT50", "Mixbot", 84.99m),
            new Product("BOT90", "Megabot", 299.99m),
        };
    }

    public void Add(Product product)
    {
        Products.Add(product);
    }

    public void Clear()
    {
        Products.Clear();
    }

    /// <summary>
    /// Find a product in the product database by looking up the code. If we don't find a product,
    /// just return `null`.
    /// </summary>
    public Product? Find(string code)
    {
        foreach (var product in Products)
            if (product.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase))
                return product;

        return null;
    }

    public void LoadFromFile()
    {
        if (!File.Exists("products.json"))
            return;
    
        Products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText("products.json"));
        Products = Products
            .OrderBy(x => x.Code)
            .ToList();
    }

    public void SaveToFile()
    {
        var json = JsonConvert.SerializeObject(Products, Formatting.Indented);
        File.WriteAllText("products.json", json);
    }
}