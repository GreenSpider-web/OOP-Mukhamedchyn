using System;
using System.Collections.Generic;

namespace IndependentWork7
{
    public class Cache<TKey, TValue>
    {
        private readonly int _capacity;
        private readonly Dictionary<TKey, TValue> _cache;
        private readonly Queue<TKey> _keysOrder;

        public Cache(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
            }
            _capacity = capacity;
            _cache = new Dictionary<TKey, TValue>();
            _keysOrder = new Queue<TKey>();
        }

        public int Count => _cache.Count;

        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
                return;
            }

            if (_cache.Count >= _capacity)
            {
                TKey oldestKey = _keysOrder.Dequeue();
                _cache.Remove(oldestKey);
            }

            _cache[key] = value;
            _keysOrder.Enqueue(key);
        }

        public TValue Get(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_cache.TryGetValue(key, out TValue value))
            {
                return value;
            }

            throw new KeyNotFoundException($"Key '{key}' was not found in cache.");
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _cache.ContainsKey(key);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cache<string, string> cache = new Cache<string, string>(3);

            cache.Add("item1", "Data 1");
            cache.Add("item2", "Data 2");
            cache.Add("item3", "Data 3");

            Console.WriteLine($"Count: {cache.Count}");
            Console.WriteLine($"Contains item1: {cache.ContainsKey("item1")}");

            cache.Add("item4", "Data 4");

            Console.WriteLine($"Contains item1 after FIFO eviction: {cache.ContainsKey("item1")}");
            Console.WriteLine($"Contains item4: {cache.ContainsKey("item4")}");
            Console.WriteLine($"Count: {cache.Count}");
            Console.WriteLine($"Item 2 value: {cache.Get("item2")}");
        }
    }
}
