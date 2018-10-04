namespace Async.Task3.Models
{
    public class BasketItem
    {
        public ShopItem ShopItem { get; set; }

        public int Count { get; set; }

        public override string ToString()
        {
            return $"{ShopItem.Name}: {Count}";
        }
    }
}
