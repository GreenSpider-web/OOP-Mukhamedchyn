using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedIndexDemo
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public override string ToString() => $"[{Id}] {Title}";
    }

    public static class TextProcessor
    {
        public static IEnumerable<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Enumerable.Empty<string>();
            }

            return Regex.Matches(text.ToLowerInvariant(), @"\w+")
                        .Cast<Match>()
                        .Select(m => m.Value);
        }

        public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
        {
            var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "a", "an", "the", "is", "are", "was", "were", "in", "on", "at", "to", "for", "of", "and", "or"
            };
            return words.Where(w => !stopWords.Contains(w));
        }
    }

    public class InvertedIndex
    {
        private readonly Dictionary<string, HashSet<int>> _index = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<int, Document> _documents = new Dictionary<int, Document>();

        public void AddDocument(Document doc)
        {
            _documents[doc.Id] = doc;
            var tokens = TextProcessor.RemoveStopWords(TextProcessor.Tokenize(doc.Content));

            foreach (var word in tokens)
            {
                if (!_index.ContainsKey(word))
                {
                    _index[word] = new HashSet<int>();
                }
                _index[word].Add(doc.Id);
            }
        }

        public List<Document> SearchSingleWord(string word)
        {
            string normalized = word.ToLowerInvariant();
            if (_index.TryGetValue(normalized, out var docIds))
            {
                return docIds.Select(id => _documents[id]).ToList();
            }
            return new List<Document>();
        }

        public List<Document> SearchMultipleWords(params string[] words)
        {
            if (words == null || words.Length == 0)
            {
                return new List<Document>();
            }

            HashSet<int> resultSet = null;

            foreach (var rawWord in words)
            {
                string word = rawWord.ToLowerInvariant();
                if (_index.TryGetValue(word, out var docIds))
                {
                    if (resultSet == null)
                    {
                        resultSet = new HashSet<int>(docIds);
                    }
                    else
                    {
                        resultSet.IntersectWith(docIds);
                    }
                }
                else
                {
                    return new List<Document>();
                }
            }

            return resultSet != null ? resultSet.Select(id => _documents[id]).ToList() : new List<Document>();
        }

        public List<Document> SearchAnyWord(params string[] words)
        {
            if (words == null || words.Length == 0)
            {
                return new List<Document>();
            }

            HashSet<int> resultSet = new HashSet<int>();

            foreach (var rawWord in words)
            {
                string word = rawWord.ToLowerInvariant();
                if (_index.TryGetValue(word, out var docIds))
                {
                    resultSet.UnionWith(docIds);
                }
            }

            return resultSet.Select(id => _documents[id]).ToList();
        }

        public int DocumentCount => _documents.Count;
        public int UniqueWords => _index.Keys.Count;
    }

    public class PerformanceTest
    {
        public static void CompareSearchMethods(List<Document> documents, string searchWord)
        {
            var sw = new Stopwatch();
            string normalizedWord = searchWord.ToLowerInvariant();

            sw.Start();
            var linearResults = documents.Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord)).ToList();
            sw.Stop();
            long linearTicks = sw.ElapsedTicks;

            var index = new InvertedIndex();
            foreach (var doc in documents)
            {
                index.AddDocument(doc);
            }

            sw.Restart();
            var indexResults = index.SearchSingleWord(searchWord);
            sw.Stop();
            long indexTicks = sw.ElapsedTicks;

            Console.WriteLine($"Docs: {documents.Count} | Linear: {linearTicks} ticks | Index: {indexTicks} ticks | Found: L={linearResults.Count}, I={indexResults.Count}");
        }

        public static List<Document> GenerateDocuments(int count)
        {
            var list = new List<Document>();
            string[] topics = { "C# programming", "Java ecosystem", "Python data science", "Web development", "Cloud microservices" };
            string[] words = { "fast", "scalable", "modern", "async", "performance", "architecture", "code", "system" };

            Random rand = new Random(42);
            for (int i = 1; i <= count; i++)
            {
                string topic = topics[rand.Next(topics.Length)];
                string extra = words[rand.Next(words.Length)] + " " + words[rand.Next(words.Length)];
                list.Add(new Document
                {
                    Id = i,
                    Title = $"Document {i} ({topic})",
                    Content = $"{topic} is {extra} for software development and programming."
                });
            }
            return list;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sampleDocs = new List<Document>
            {
                new Document { Id = 1, Title = "Introduction to C#", Content = "C# is a modern object-oriented programming language developed by Microsoft." },
                new Document { Id = 2, Title = "Java Basics", Content = "Java is a popular programming language known for its portability and performance." },
                new Document { Id = 3, Title = "Python Overview", Content = "Python is a high-level programming language with simple syntax." },
                new Document { Id = 4, Title = "C# Advanced Features", Content = "C# supports LINQ, async/await, and many modern programming paradigms." },
                new Document { Id = 5, Title = "Comparing Languages", Content = "C#, Java, and Python are among the most popular programming languages today." }
            };

            Console.WriteLine("PERFORMANCE BENCHMARKS");
            int[] testSizes = { 10, 100, 1000, 10000 };
            foreach (int size in testSizes)
            {
                var testDocs = PerformanceTest.GenerateDocuments(size);
                PerformanceTest.CompareSearchMethods(testDocs, "programming");
            }

            Console.WriteLine("\nINTERACTIVE DEMO");
            var index = new InvertedIndex();
            foreach (var doc in sampleDocs)
            {
                index.AddDocument(doc);
            }

            Console.WriteLine($"Loaded {index.DocumentCount} documents");
            Console.WriteLine($"Index contains {index.UniqueWords} unique words\n");

            while (true)
            {
                Console.Write("Enter search query (or 'exit'): ");
                var query = Console.ReadLine();
                if (query == "exit" || string.IsNullOrWhiteSpace(query)) break;

                var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine("\nSearch Results (AND)");
                var results = index.SearchMultipleWords(words);
                foreach (var doc in results)
                {
                    Console.WriteLine(doc);
                    Console.WriteLine($"   {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
                }
                Console.WriteLine($"Found {results.Count} documents\n");
            }
        }
    }
}