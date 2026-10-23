using System;
using System.Collections.Generic;
using IndependentWork7;
using Xunit;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_And_Get_ReturnsCorrectValue()
        {
            var cache = new Cache<int, string>(3);
            cache.Add(1, "One");

            Assert.Equal("One", cache.Get(1));
        }

        [Fact]
        public void Add_ExceedingCapacity_EvictsOldestItemFIFO()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "One");
            cache.Add(2, "Two");
            cache.Add(3, "Three");

            Assert.Equal(2, cache.Count);
            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
        }

        [Fact]
        public void Get_NonExistingKey_ThrowsKeyNotFoundException()
        {
            var cache = new Cache<string, int>(2);
            cache.Add("A", 100);

            Assert.Throws<KeyNotFoundException>(() => cache.Get("B"));
        }

        [Fact]
        public void ContainsKey_ReturnsExpectedResult()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("K1", "V1");

            Assert.True(cache.ContainsKey("K1"));
            Assert.False(cache.ContainsKey("K2"));
        }

        [Fact]
        public void Count_TracksNumberOfItemsCorrectly()
        {
            var cache = new Cache<int, int>(5);
            Assert.Equal(0, cache.Count);

            cache.Add(1, 10);
            cache.Add(2, 20);

            Assert.Equal(2, cache.Count);
        }
    }
}