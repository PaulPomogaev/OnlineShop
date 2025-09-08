namespace OnlineShopWebApp.Models
{
    public class Product
    {
        private static int _idCounter = 1;
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public  string Description { get; set; }

        public Product() { }

        public Product(string name, decimal cost, string description)
        {
            Id = _idCounter++;
            Name = name;
            Cost = cost;
            Description = description;
        }
    }
}
