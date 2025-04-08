using ClothingStore.Session;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using ClothingStore.Service;
using ClothingStore.Models;
using System;
using Microsoft.EntityFrameworkCore;


namespace ClothingStore.Service
{

    public class CartService
    {
        private readonly ClothingStoreContext _context;

        public CartService(ClothingStoreContext context)
        {
            _context = context;
        }

        public Cart GetCartByUserId(int userId)
        {
            return _context.Carts.Include(c => c.Items)
                                 .FirstOrDefault(c => c.UserId == userId);
        }

        public void AddToCart(int userId, int productId, string productName, string productType, string imageUrl, decimal price, int stock)
        {
            Console.WriteLine($"AddToCart called with: userId={userId}, productId={productId}, productName={productName}, imageUrl={imageUrl}, price={price}, stock={stock}");
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            Console.WriteLine($"AddToCart called with: userId={userId}, productId={productId}, productName={productName}, productType={productType}, imageUrl={imageUrl}, price={price}, stock={stock}");
            // Kiểm tra nếu imageUrl null thì gán giá trị mặc định
            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = "/images/default.jpg"; // Thay bằng ảnh mặc định
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity < stock)
                {
                    cartItem.Quantity++;
                }
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    ImageUrl = imageUrl,
                    Quantity = 1,
                    Price = price,
                    Stock = stock
                });
            }

            _context.SaveChanges();
        }

        public void RemoveFromCart(int userId, int productId)
        {
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            var item = cart?.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                _context.SaveChanges();
            }
        }

        public void Checkout(int userId)
        {
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public void UpdateQuantity(int userId, int productId, int quantity)
        {
            var cart = _context.Carts.Include(c => c.Items)
                                     .FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null && quantity > 0 && quantity <= item.Stock)
                {
                    item.Quantity = quantity;
                    _context.SaveChanges();
                }
            }
        }

        public void CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }


    }

}
