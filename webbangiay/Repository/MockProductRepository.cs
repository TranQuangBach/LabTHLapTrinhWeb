using webbangiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace webbangiay.Repository
{
    public class MockProductRepository : IProductRepository
    {
        private static List<Product> _products; // Đổi thành static để dùng chung
        private static int _nextId;
        private static readonly object _lock = new object();

        public MockProductRepository()
        {
            lock (_lock)
            {
                if (_products == null)
                {
                    _products = new List<Product>
                    {
                        new Product
                    {
                        Id = 1,
                        Name = "Nike Air Max 270",
                        Description = "Giày thể thao chạy bộ",
                        Price = 2200000,
                        OldPrice = 2750000,
                        Rating = 4.5,
                        ImageUrl = "~/images/sneaker1.png",
                        ColorsString = "#000,#fff,#e74c3c",
                        BadgeText = "-20%",
                        BadgeClass = "",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Adidas Ultraboost",
                        Description = "Giày chạy bộ đế Boost",
                        Price = 3500000,
                        OldPrice = null,
                        Rating = 4.0,
                        ImageUrl = "~/images/sneaker2.png",
                        ColorsString = "#2c3e50,#3498db",
                        BadgeText = null,
                        BadgeClass = "",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Converse Chuck 70",
                        Description = "Giày cổ điển unisex",
                        Price = 1850000,
                        OldPrice = 2100000,
                        Rating = 5.0,
                        ImageUrl = "~/images/sneaker3.png",
                        ColorsString = "#e74c3c,#2c3e50,#f1c40f",
                        BadgeText = "Mới",
                        BadgeClass = "bg-success",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Puma RS-X",
                        Description = "Giày sneaker unisex",
                        Price = 2100000,
                        OldPrice = null,
                        Rating = 3.5,
                        ImageUrl = "~/images/sneaker4.png",
                        ColorsString = "#fff,#e67e22",
                        BadgeText = null,
                        BadgeClass = "",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "New Balance 574",
                        Description = "Giày cổ điển thoải mái",
                        Price = 2450000,
                        OldPrice = 2880000,
                        Rating = 4.5,
                        ImageUrl = "~/images/sneaker5.png",
                        ColorsString = "#95a5a6,#2c3e50,#27ae60",
                        BadgeText = "-15%",
                        BadgeClass = "",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "Vans Old Skool",
                        Description = "Giày trượt ván classic",
                        Price = 1650000,
                        OldPrice = null,
                        Rating = 4.0,
                        ImageUrl = "~/images/sneaker6.png",
                        ColorsString = "#000,#e74c3c,#3498db,#27ae60",
                        BadgeText = null,
                        BadgeClass = "",
                        CategoryId = 1
                    }
                    };
                    _nextId = _products.Max(p => p.Id) + 1;
                }
            }
        }


        
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            // Kiểm tra nếu Id = 0 thì gán Id mới
            if (product.Id == 0)
            {
                product.Id = _nextId++;
            }
            _products.Add(product);

            // Debug: In ra console để kiểm tra
            System.Diagnostics.Debug.WriteLine($"Đã thêm sản phẩm: {product.Name} - ID: {product.Id}");
            System.Diagnostics.Debug.WriteLine($"Tổng số sản phẩm hiện tại: {_products.Count}");
        }

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
                System.Diagnostics.Debug.WriteLine($"Đã cập nhật sản phẩm: {product.Name} - ID: {product.Id}");
            }
        }

        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
                System.Diagnostics.Debug.WriteLine($"Đã xóa sản phẩm ID: {id}");
            }
        }

        public void SaveChanges()
        {
            // Với Mock Repository, không cần thực hiện gì cả
            System.Diagnostics.Debug.WriteLine("Mock: Changes saved successfully");
        }
    }
}