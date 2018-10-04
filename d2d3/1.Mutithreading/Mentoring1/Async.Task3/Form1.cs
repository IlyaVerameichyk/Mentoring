using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Async.Task3.Models;

namespace Async.Task3
{
    public partial class Form1 : Form
    {
        private IList<ShopItem> _shopItems = new List<ShopItem>()
        {
            new ShopItem() {Name = "Meat", Price = 10},
            new ShopItem() {Name = "Fruit", Price = 4.5m},
            new ShopItem() {Name = "Fish", Price = 1.1m},
            new ShopItem() {Name = "Mango", Price = 0.5m},
            new ShopItem() {Name = "Milk", Price = 6.8m},
            new ShopItem() {Name = "Surprise", Price = 9.999m},
        };

        public Form1()
        {
            InitializeComponent();
            foreach (var shopItem in _shopItems)
            {
                listBox1.Items.Add(shopItem);
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) { return; }
            var item = (ShopItem)listBox1.SelectedItem;
            var basketItem = await FindItem(item);
            if (basketItem == null)
            {
                listBox2.Items.Add(new BasketItem() { Count = 1, ShopItem = item });
            }
            else
            {
                basketItem.Count++;
                listBox2.Items[listBox2.Items.IndexOf(basketItem)] = basketItem;
            }
            var newPrice = await CalculatePrice();
            label3.Text = newPrice.ToString("0.00");
        }

        public Task<BasketItem> FindItem(ShopItem shopItem)
        {
            return Task.Factory.StartNew(() =>
            {
                Task.Delay(1000).Wait();
                return listBox2.Items.OfType<BasketItem>().SingleOrDefault(item => item.ShopItem == shopItem);
            });
        }

        public Task<decimal> CalculatePrice()
        {
            return Task.Factory.StartNew(() =>
            {
                Task.Delay(1000).Wait();
                return listBox2.Items.OfType<BasketItem>().Select(item => item.Count * item.ShopItem.Price).Sum();
            });
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) { return; }
           
            listBox2.Items.Remove(listBox2.SelectedItem);
            var newPrice = await CalculatePrice();
            label3.Text = newPrice.ToString("0.00");
        }
    }
}
