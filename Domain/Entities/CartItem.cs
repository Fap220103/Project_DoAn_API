﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; }
        public ShoppingCart()
        {
            this.Items = new List<CartItem>();
        }
        public void AddToCart(CartItem item, int quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExits != null)
            {
                checkExits.Quantity += quantity;
                checkExits.TotalPrice = checkExits.Quantity * checkExits.Price;
            }
            else
            {
                Items.Add(item);

            }
        }
        public void Remove(string id)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId.Equals(id));
            if (checkExits != null)
            {
                Items.Remove(checkExits);

            }

        }
        public void UpdateQuantity(string id, int quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId.Equals(id));
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
            }
        }
        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TotalPrice);
        }
        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }
    }
}
