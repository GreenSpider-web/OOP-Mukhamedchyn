using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork11
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var (products, customers, sales) = GenerateTestData();

            Console.WriteLine("PIPELINE 1: TOP PRODUCTS BY CATEGORY");
            RunPipeline1(sales, products);

            Console.WriteLine("\nPIPELINE 2: REVENUE BY CUSTOMER CITY");
            RunPipeline2(sales, products, customers);

            Console.WriteLine("\nPIPELINE 3: SALES BY YEAR AND MONTH");
            RunPipeline3(sales, products);

            Console.WriteLine("\nPIPELINE 4: VIP CUSTOMERS ANALYTICS");
            RunPipeline4(sales, products, customers);
        }

        static (List<Product>, List<Customer>, List<Sale>) GenerateTestData()
        {
            Random rand = new Random(42);

            string[] categories = { "Electronics", "Books", "Clothing", "Home & Kitchen" };
            string[] cities = { "Kyiv", "Lviv", "Odesa", "Kharkiv", "Dnipro" };

            var products = Enumerable.Range(1, 20).Select(i => new Product
            {
                Id = i,
                Name = $"Product_{i}",
                Category = categories[rand.Next(categories.Length)],
                UnitPrice = rand.Next(10, 500)
            }).ToList();

            var customers = Enumerable.Range(1, 15).Select(i => new Customer
            {
                Id = i,
                Name = $"Customer_{i}",
                City = cities[rand.Next(cities.Length)]
            }).ToList();

            DateTime startDate = new DateTime(2024, 1, 1);
            var sales = Enumerable.Range(1, 150).Select(i => new Sale
            {
                Id = i,
                ProductId = products[rand.Next(products.Count)].Id,
                Quantity = rand.Next(1, 10),
                SaleDate = startDate.AddDays(rand.Next(0, 365)),
                CustomerId = customers[rand.Next(customers.Count)].Id
            }).ToList();

            return (products, customers, sales);
        }

        static void RunPipeline1(List<Sale> sales, List<Product> products)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var result = sales
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { p.Name, p.Category, s.TotalQuantity })
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TopProducts = g.OrderByDescending(x => x.TotalQuantity).Take(3).ToList()
                })
                .ToList();

            sw.Stop();

            foreach (var cat in result)
            {
                Console.WriteLine($"Category: {cat.Category}");
                foreach (var item in cat.TopProducts)
                {
                    Console.WriteLine($"  - {item.Name}: {item.TotalQuantity} pcs sold");
                }
            }
            Console.WriteLine($"Execution Time: {sw.Elapsed.TotalMilliseconds:F3} ms");
        }

        static void RunPipeline2(List<Sale> sales, List<Product> products, List<Customer> customers)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var result = sales
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
                .Join(customers, s => s.CustomerId, c => c.Id, (s, c) => new { c.City, s.Revenue })
                .GroupBy(c => c.City)
                .Select(g => new { City = g.Key, TotalRevenue = g.Sum(x => x.Revenue) })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();

            sw.Stop();

            foreach (var item in result)
            {
                Console.WriteLine($"City: {item.City,-10} | Revenue: {item.TotalRevenue,10:C}");
            }
            Console.WriteLine($"Execution Time: {sw.Elapsed.TotalMilliseconds:F3} ms");
        }

        static void RunPipeline3(List<Sale> sales, List<Product> products)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var result = sales
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new
                {
                    s.SaleDate.Year,
                    s.SaleDate.Month,
                    s.Quantity,
                    Revenue = s.Quantity * p.UnitPrice
                })
                .GroupBy(x => new { x.Year, x.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalUnits = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Revenue)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            sw.Stop();

            foreach (var item in result)
            {
                Console.WriteLine($"Period: {item.Year}-{item.Month:D2} | Units: {item.TotalUnits,4} | Revenue: {item.TotalRevenue,10:C}");
            }
            Console.WriteLine($"Execution Time: {sw.Elapsed.TotalMilliseconds:F3} ms");
        }

        static void RunPipeline4(List<Sale> sales, List<Product> products, List<Customer> customers)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var result = sales
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
                .GroupBy(s => s.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalSpent = g.Sum(x => x.Revenue),
                    OrdersCount = g.Count()
                })
                .Where(x => x.TotalSpent > 2000m)
                .Join(customers, x => x.CustomerId, c => c.Id, (x, c) => new
                {
                    c.Name,
                    c.City,
                    x.TotalSpent,
                    x.OrdersCount,
                    AvgPerOrder = x.TotalSpent / x.OrdersCount
                })
                .OrderByDescending(x => x.TotalSpent)
                .ToList();

            sw.Stop();

            foreach (var vip in result)
            {
                Console.WriteLine($"VIP: {vip.Name,-12} ({vip.City}) | Spent: {vip.TotalSpent,9:C} | Orders: {vip.OrdersCount} | Avg: {vip.AvgPerOrder,8:C}");
            }
            Console.WriteLine($"Execution Time: {sw.Elapsed.TotalMilliseconds:F3} ms");
        }
    }
}