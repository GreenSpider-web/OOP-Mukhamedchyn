using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace IndependentWork5
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"[Product] Назва: {Name} | Ціна: {Price} грн";
        }
    }

    public class Customer
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public Customer(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }

        public override string ToString()
        {
            return $"[Customer] ПІБ: {FullName} | Email: {Email}";
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }

        public Order(int orderId, decimal totalAmount)
        {
            OrderId = orderId;
            TotalAmount = totalAmount;
        }

        public override string ToString()
        {
            return $"[Order] Номер: #{OrderId} | Сума: {TotalAmount} грн";
        }
    }

    public class MyCustomCollection : IEnumerable<string>
    {
        private readonly List<string> _items = new List<string>();

        public void Add(string item)
        {
            _items.Add(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        public static void ProcessStream(Stream stream)
        {
            Console.WriteLine($"\nТип потоку: {stream.GetType().Name}");
            Console.WriteLine($"CanRead={stream.CanRead}, CanSeek={stream.CanSeek}, Довжина={stream.Length} байт");

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            byte[] buffer = new byte[stream.Length];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            string content = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Прочитано: \"{content}\"");
        }

        static void Main(string[] args)
        {
            List<object> objects = new List<object>
            {
                new Product("Ноутбук", 35000m),
                new Customer("Іван Петренко", "ivan@example.com"),
                new Order(1001, 35000m),
                "Звичайний рядок",
                42
            };

            foreach (object obj in objects)
            {
                Console.WriteLine(obj.ToString());
            }

            MyCustomCollection customCol = new MyCustomCollection();
            customCol.Add("Перший елемент");
            customCol.Add("Другий елемент");
            customCol.Add("Третій елемент");

            foreach (string item in customCol)
            {
                Console.WriteLine($"Елемент колекції: {item}");
            }

            string tempFile = "temp_test.txt";
            File.WriteAllText(tempFile, "Тестові дані для FileStream");

            using (FileStream fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
            {
                ProcessStream(fileStream);
            }

            byte[] memoryBytes = System.Text.Encoding.UTF8.GetBytes("Тестові дані для MemoryStream");
            using (MemoryStream memoryStream = new MemoryStream(memoryBytes))
            {
                ProcessStream(memoryStream);
            }

            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}