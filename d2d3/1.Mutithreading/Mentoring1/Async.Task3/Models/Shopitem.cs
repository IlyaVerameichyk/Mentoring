namespace Async.Task3.Models
{
    public class ShopItem
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Name} ${Price:0.00}";
        }
    }
}