using System.
Collections.Generic;
using System.Text;

public class Address
{
    private string _street;
    private string _city;
    private string _stateOrProvince;
    private string _country;

    public Address(string street, string city, string stateOrProvince, string country)
    {
        _street = street;
        _city = city;
        _stateOrProvince = stateOrProvince;
        _country = country;
    }

    public string Street
    {
        get { return _street; }
        set { _street = value; }
    }

    public string City
    {
        get { return _city; }
        set { _city = value; }
    }

    public string StateOrProvince
    {
        get { return _stateOrProvince; }
        set { _stateOrProvince = value; }
    }

    public string Country
    {
        get { return _country; }
        set { _country = value; }
    }

    public bool IsInUsa()
    {
        return _country.Trim().Equals("USA", System.StringComparison.OrdinalIgnoreCase) ||
               _country.Trim().Equals("United States", System.StringComparison.OrdinalIgnoreCase) ||
               _country.Trim().Equals("United States of America", System.StringComparison.OrdinalIgnoreCase);
    }

    public string GetFormattedAddress()
    {
        return $"{_street}\n{_city}, {_stateOrProvince}\n{_country}";
    }
}

public class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Address Address
    {
        get { return _address; }
        set { _address = value; }
    }

    public bool LivesInUsa()
    {
        return _address.IsInUsa();
    }
}

public class Product
{
    private string _name;
    private string _productId;
    private decimal _price;
    private int _quantity;

    public Product(string name, string productId, decimal price, int quantity)
    {
        _name = name;
        _productId = productId;
        _price = price;
        _quantity = quantity;
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string ProductId
    {
        get { return _productId; }
        set { _productId = value; }
    }

    public decimal Price
    {
        get { return _price; }
        set { _price = value; }
    }

    public int Quantity
    {
        get { return _quantity; }
        set { _quantity = value; }
    }

    public decimal GetTotalCost()
    {
        return _price * _quantity;
    }
}


public class Order
{
    private List<Product> _products;
    private Customer _customer;

    public Order(Customer customer)
    {
        _customer = customer;
        _products = new List<Product>();
    }

    public List<Product> Products
    {
        get { return _products; }
    }

    public Customer Customer
    {
        get { return _customer; }
        set { _customer = value; }
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public decimal CalculateTotalCost()
    {
        decimal productTotal = 0;
        foreach (var product in _products)
        {
            productTotal += product.GetTotalCost();
        }

        // $5 shipping if in USA, $35 if international
        decimal shippingCost = _customer.LivesInUsa() ? 5.0m : 35.0m;
        return productTotal + shippingCost;
    }

    public string GetPackingLabel()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--- Packing Label ---");
        foreach (var product in _products)
        {
            sb.AppendLine($"Product: {product.Name} (ID: {product.ProductId})");
        }
        return sb.ToString().TrimEnd();
    }

    public string GetShippingLabel()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--- Shipping Label ---");
        sb.AppendLine(_customer.Name);
        sb.AppendLine(_customer.Address.GetFormattedAddress());
        return sb.ToString().TrimEnd();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // --- Order 1: USA Customer ---
        Address address1 = new Address("123 Maple Street", "Springfield", "IL", "USA");
        Customer customer1 = new Customer("John Doe", address1);

        Product p1 = new Product("Wireless Mouse", "WM-01", 25.50m, 2);
        Product p2 = new Product("Mechanical Keyboard", "KB-02", 79.99m, 1);

        Order order1 = new Order(customer1);
        order1.AddProduct(p1);
        order1.AddProduct(p2);

        // --- Order 2: International Customer ---
        Address address2 = new Address("456 Queen St W", "Toronto", "ON", "Canada");
        Customer customer2 = new Customer("Jane Smith", address2);

        Product p3 = new Product("USB-C Hub", "UC-03", 19.99m, 3);
        Product p4 = new Product("Monitor Stand", "MS-04", 45.00m, 1);
        Product p5 = new Product("Mouse Pad", "MP-05", 12.50m, 2);

        Order order2 = new Order(customer2);
        order2.AddProduct(p3);
        order2.AddProduct(p4);
        order2.AddProduct(p5);

        // --- Display Results for Order 1 ---
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine();
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine();
        Console.WriteLine($"Total Price: ${order1.CalculateTotalCost():F2}");
        Console.WriteLine();
        Console.WriteLine(new string('=', 40));
        Console.WriteLine();

        // --- Display Results for Order 2 ---
        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine();
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine();
        Console.WriteLine($"Total Price: ${order2.CalculateTotalCost():F2}");
    }
}