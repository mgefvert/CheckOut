// Create a product database with a ProductCode, Name, and a Unit Price
var products = new[]
{
    new Product("BOT10", "Shortbot", 34.99m),
    new Product("BOT50", "Mixbot", 84.99m),
    new Product("BOT90", "Megabot", 299.99m),
};

// Create a shopping cart with a list of Cart items
var shoppingCart = new List<Cart>();

// Set the tax rate to 6%
var taxRate      = 0.06;

var done = false;
while (!done)
{
    // Clear the console, display header and the current shopping cart
    Console.Clear();
    DisplayHeader();
    DisplayShoppingCart();

    // Handle the user keypress
    try
    {
        done = HandleUserChoice();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        Pause();
    }
    
}

// Display a small menu, wait for the user to press something, and then do the appropriate action.
// Returns a boolean indicating whether we're done with the purchase or not.
bool HandleUserChoice()
{
    // What does the user want to do?
    Console.WriteLine("SELECT:  A = Add item, C = Clear, L = List products, R = Remove, P = Purchase");
    var key = Console.ReadKey();
    Console.WriteLine();
    Console.WriteLine();

    switch (key.Key)
    {
        case ConsoleKey.A:
            AddToCart();
            return false;

        case ConsoleKey.C:
            ClearCart();
            return false;

        case ConsoleKey.L:
            DisplayProducts();
            Pause();
            return false;

        case ConsoleKey.P:
            Console.WriteLine("THANK YOU FOR YOUR PURCHASE. PLEASE COME AGAIN.");
            return true;

        case ConsoleKey.R:
            RemoveFromCart();
            return false;

        default:
            return false;
    }
}

// Add an item to the shopping cart. Ask for the product code, and a number of units, and then
// add that to the shopping cart.
void AddToCart()
{
    Console.Write("PRODUCT CODE : ");
    var code = Console.ReadLine();

    // Did the user actually enter anything meaningful?
    if (string.IsNullOrWhiteSpace(code))
        return;

    code = code.Trim();

    // Find the product - look it up by the product code we entered
    var product = FindProduct(code);
    if (product == null)
    {
        Console.WriteLine();
        Console.WriteLine("ERROR: Unknown product code");
        Pause();
        return;
    }

    // Write out the product information
    Console.WriteLine();
    Console.WriteLine($"PRODUCT CODE : {product.Code}");
    Console.WriteLine($"PRODUCT NAME : {product.Name}");
    Console.WriteLine($"UNIT PRICE   : {product.Price}");
    Console.WriteLine();

    // Ask for unit count
    Console.Write("NUMBER OF UNITS (1 .. ) : ");
    var unitString = Console.ReadLine() ?? "1";
    var units = int.Parse(unitString);
    if (units <= 0)
    {
        Console.WriteLine();
        Console.WriteLine("ERROR: Invalid number of units");
        Pause();
        return;
    }

    // Add the new code and unit count to the shopping cart
    var cartItem = FindCartItem(code);
    if (cartItem == null)
    {
        shoppingCart.Add(new Cart(code, units));
    }
    else
    {
        cartItem.Count += units;
    }
}

// Clear the shopping cart.
void ClearCart()
{
    shoppingCart.Clear();
}

// Display the header. Not that Console.Clear() is not part of this, this means we can use it
// also for printing the receipt on paper.
void DisplayHeader()
{
    Console.WriteLine("BOT STORE                                     99 Holbrook Ave, Portland, OR");
    Console.WriteLine("===========================================================================");
    Console.WriteLine();
}

// Display a list of all products in the product database.
void DisplayProducts()
{
    Console.WriteLine("CURRENT PRODUCT LIST:");

    foreach (var product in products)
    {
        Console.WriteLine($"{product.Code,10} {product.Name,-30} {product.Price,10}");
    }

    Console.WriteLine();
    Console.WriteLine($"{products.Length} PRODUCTS IN DATABASE");
}

// Display the shopping cart along with subtotal, tax, and grand total.
void DisplayShoppingCart()
{
    Console.WriteLine("CURRENT CHECKOUT CART:");

    var total = 0m;
    foreach (var item in shoppingCart)
    {
        var product = FindProduct(item.Code);
        if (product == null)
            throw new Exception("Undefined product in shopping cart.");

        Console.WriteLine($"{product.Code,10}      {product.Price,10}  {item.Count,5}  {product.Price * item.Count,10}");
        total += product.Price * item.Count;
    }

    Console.WriteLine();
    Console.WriteLine($"SUBTOTAL                           {total,10}");
    Console.WriteLine();

    var tax = (decimal)Math.Round(taxRate * (double)total, 2, MidpointRounding.AwayFromZero);

    Console.WriteLine($"TAX                                {tax,10}");
    Console.WriteLine($"TOTAL                              {tax + total,10}");
    Console.WriteLine();
}

// Find a product in the product database by looking up the code. If we don't find a product,
// just return `null`.
Cart? FindCartItem(string code)
{
    foreach (var cartItem in shoppingCart)
        if (cartItem.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase))
            return cartItem;

    return null;
}

// Find a product in the product database by looking up the code. If we don't find a product,
// just return `null`.
Product? FindProduct(string code)
{
    foreach (var product in products)
        if (product.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase))
            return product;

    return null;
}

// Ask the user which product to remove from the cart, and then remove all instances of that.
void RemoveFromCart()
{
    Console.Write("PRODUCT CODE : ");
    var code    = Console.ReadLine();

    // Did the user actually enter a code?
    if (string.IsNullOrWhiteSpace(code))
        return;

    // Iterate through the shopping cart and remove all instances of the product code.
    var i = 0;
    while (i < shoppingCart.Count)
    {
        if (shoppingCart[i].Code == code)
            shoppingCart.RemoveAt(i);
        else
            i++;
    }
}

// Pause and wait for the user
void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Press ENTER to continue...");
    Console.ReadLine();
}

// Define a "class", or "record", called Cart which contains a ProductCode and a unit count.
class Cart
{
    public Cart(string code, int units)
    {
        Code = code;
        Count = units;
    }

    public string Code { get; set; }
    public int Count { get; set; }
}

// Define a class or record called Product which contains a ProductCode, ProductName, and a Unit Price.
record Product(string Code, string Name, decimal Price);
