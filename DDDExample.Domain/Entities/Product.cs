using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DDDExample.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Constructor
        public Product(string name, string description, decimal price, int stock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }

        // Métodos de negocio
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Price cannot be negative.");
            Price = newPrice;
        }

        public void Restock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            Stock += quantity;
        }

        public bool IsInStock()
        {
            return Stock > 0;
        }
    }
}


        