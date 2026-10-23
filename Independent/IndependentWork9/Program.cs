using System;
using System.Collections.Generic;

namespace IndependentWork9
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public static class CollectionUtils
    {
        public static IEnumerable<TResult> Map<TSource, TResult>(
            IEnumerable<TSource> collection,
            Func<TSource, TResult> mapper)
        {
            var result = new List<TResult>();
            foreach (var item in collection)
            {
                result.Add(mapper(item));
            }
            return result;
        }

        public static T MaxBy<T, TKey>(
            IEnumerable<T> collection,
            Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            T maxItem = default(T);
            TKey maxKey = default(TKey);
            bool first = true;

            foreach (var item in collection)
            {
                TKey key = keySelector(item);
                if (first || key.CompareTo(maxKey) > 0)
                {
                    maxKey = key;
                    maxItem = item;
                    first = false;
                }
            }
            return maxItem;
        }

        public static IEnumerable<T> Filter<T>(
            IEnumerable<T> collection,
            Predicate<T> predicate)
        {
            var result = new List<T>();
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static void ForEach<T>(
            IEnumerable<T> collection,
            Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 5, 12, 8, 3, 20 };
            var doubled = CollectionUtils.Map(numbers, x => x * 2);
            var maxNumber = CollectionUtils.MaxBy(numbers, x => x);
            var evenNumbers = CollectionUtils.Filter(numbers, x => x % 2 == 0);
            
            Console.WriteLine("--- Numbers Demo ---");
            Console.WriteLine($"Doubled: {string.Join(", ", doubled)}");
            Console.WriteLine($"Max: {maxNumber}");
            Console.Write("Even numbers: ");
            CollectionUtils.ForEach(evenNumbers, x => Console.Write($"{x} "));
            Console.WriteLine("\n");

            List<string> words = new List<string> { "apple", "banana", "cherry", "dragonfruit", "fig" };
            var wordLengths = CollectionUtils.Map(words, w => w.Length);
            var longestWord = CollectionUtils.MaxBy(words, w => w.Length);
            var longWords = CollectionUtils.Filter(words, w => w.Length > 5);

            Console.WriteLine("--- Words Demo ---");
            Console.WriteLine($"Lengths: {string.Join(", ", wordLengths)}");
            Console.WriteLine($"Longest word: {longestWord}");
            Console.Write("Words > 5 chars: ");
            CollectionUtils.ForEach(longWords, w => Console.Write($"{w} "));
            Console.WriteLine("\n");

            List<Product> products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 25000m },
                new Product { Name = "Mouse", Price = 500m },
                new Product { Name = "Keyboard", Price = 1500m },
                new Product { Name = "Monitor", Price = 8000m }
            };

            var productNames = CollectionUtils.Map(products, p => p.Name);
            var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
            var expensiveProducts = CollectionUtils.Filter(products, p => p.Price > 1000m);

            Console.WriteLine("--- Products Demo ---");
            Console.WriteLine($"Names: {string.Join(", ", productNames)}");
            Console.WriteLine($"Most expensive: {mostExpensive.Name} ({mostExpensive.Price} UAH)");
            Console.WriteLine("Products > 1000 UAH:");
            CollectionUtils.ForEach(expensiveProducts, p => Console.WriteLine($" - {p.Name}: {p.Price} UAH"));
        }
    }
}