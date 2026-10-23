using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace IndependentWork12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. BENCHMARK: LINQ VS PLINQ");

            int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };
            foreach (int size in sizes)
            {
                RunBenchmark(size);
            }

            Console.WriteLine("2. THREAD SAFETY & SIDE EFFECTS DEMONSTRATION");

            DemonstrateSideEffectsIssue();
            DemonstrateSideEffectsFixLock();
            DemonstrateSideEffectsFixInterlocked();
            DemonstrateSideEffectsFixConcurrent();
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            int boundary = (int)Math.Floor(Math.Sqrt(number));
            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        static void RunBenchmark(int count)
        {
            Console.WriteLine($"\n Testing with {count:N0} elements");

            Random rand = new Random(42);
            List<int> data = Enumerable.Range(1, count).Select(_ => rand.Next(1, 100_000)).ToList();

            Stopwatch sw = Stopwatch.StartNew();
            int sequentialCount = data.Where(IsPrime).Select(x => x).Count();
            sw.Stop();
            long seqMs = sw.ElapsedMilliseconds;

            sw.Restart();
            int parallelCount = data.AsParallel().Where(IsPrime).Select(x => x).Count();
            sw.Stop();
            long parMs = sw.ElapsedMilliseconds;

            double speedup = (double)seqMs / parMs;
            Console.WriteLine($"Sequential LINQ : {seqMs,5} ms | Primes found: {sequentialCount}");
            Console.WriteLine($"Parallel PLINQ  : {parMs,5} ms | Primes found: {parallelCount}");
            Console.WriteLine($"Speedup Ratio   : {speedup:F2}x");
        }

        static void DemonstrateSideEffectsIssue()
        {
            int iterations = 1_000_000;
            int unsafeCounter = 0;

            Enumerable.Range(1, iterations).AsParallel().ForAll(_ =>
            {
                unsafeCounter++;
            });

            Console.WriteLine($"Unsafe Shared Counter Result : {unsafeCounter:N0} (Expected: {iterations:N0}) -> RACE CONDITION!");
        }

        static void DemonstrateSideEffectsFixLock()
        {
            int iterations = 1_000_000;
            int lockCounter = 0;
            object lockObj = new object();

            Enumerable.Range(1, iterations).AsParallel().ForAll(_ =>
            {
                lock (lockObj)
                {
                    lockCounter++;
                }
            });

            Console.WriteLine($"Fixed with 'lock' Result     : {lockCounter:N0} (Expected: {iterations:N0})");
        }

        static void DemonstrateSideEffectsFixInterlocked()
        {
            int iterations = 1_000_000;
            int interlockedCounter = 0;

            Enumerable.Range(1, iterations).AsParallel().ForAll(_ =>
            {
                Interlocked.Increment(ref interlockedCounter);
            });

            Console.WriteLine($"Fixed with 'Interlocked'     : {interlockedCounter:N0} (Expected: {iterations:N0})");
        }

        static void DemonstrateSideEffectsFixConcurrent()
        {
            int iterations = 1_000_000;
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Enumerable.Range(1, iterations).AsParallel().ForAll(i =>
            {
                if (i % 100_000 == 0)
                {
                    bag.Add(i);
                }
            });

            Console.WriteLine($"Fixed with 'ConcurrentBag'   : {bag.Count} items safely collected");
        }
    }
}