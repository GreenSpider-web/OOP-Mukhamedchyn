using System;
using System.Collections.Generic;

namespace IndependentWork8
{
    public class FileMetadata
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; }
    }

    public class FileMetadataCache
    {
        private readonly Dictionary<string, FileMetadata> _cache = new Dictionary<string, FileMetadata>(StringComparer.OrdinalIgnoreCase);

        public void Add(string path, FileMetadata metadata)
        {
            _cache[path] = metadata;
        }

        public FileMetadata Get(string path)
        {
            if (_cache.TryGetValue(path, out var metadata))
            {
                return metadata;
            }
            return null;
        }

        public bool Remove(string path)
        {
            return _cache.Remove(path);
        }

        public bool Contains(string path)
        {
            return _cache.ContainsKey(path);
        }
    }

    public class TagManager
    {
        private readonly Dictionary<string, HashSet<string>> _articleTags = new Dictionary<string, HashSet<string>>();

        public void AddTag(string articleId, string tag)
        {
            if (!_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            _articleTags[articleId].Add(tag);
        }

        public void RemoveTag(string articleId, string tag)
        {
            if (_articleTags.TryGetValue(articleId, out var tags))
            {
                tags.Remove(tag);
            }
        }

        public HashSet<string> GetAllUniqueTags()
        {
            HashSet<string> allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var tags in _articleTags.Values)
            {
                allTags.UnionWith(tags);
            }
            return allTags;
        }

        public HashSet<string> FindCommonTags(string articleId1, string articleId2)
        {
            if (!_articleTags.TryGetValue(articleId1, out var tags1) || !_articleTags.TryGetValue(articleId2, out var tags2))
            {
                return new HashSet<string>();
            }

            HashSet<string> common = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            common.IntersectWith(tags2);
            return common;
        }
    }

    public class SearchIndex
    {
        private readonly Dictionary<string, HashSet<int>> _invertedIndex = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);

        public void AddDocument(int docId, string content)
        {
            string[] words = content.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (!_invertedIndex.ContainsKey(word))
                {
                    _invertedIndex[word] = new HashSet<int>();
                }
                _invertedIndex[word].Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            if (_invertedIndex.TryGetValue(word, out var docIds))
            {
                return new HashSet<int>(docIds);
            }
            return new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words == null || words.Length == 0)
            {
                return new HashSet<int>();
            }

            HashSet<int> result = null;

            foreach (var word in words)
            {
                var docs = Search(word);
                if (result == null)
                {
                    result = new HashSet<int>(docs);
                }
                else
                {
                    result.IntersectWith(docs);
                }
            }

            return result ?? new HashSet<int>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var fileCache = new FileMetadataCache();
            fileCache.Add("C:/file1.txt", new FileMetadata { FileName = "file1.txt", Size = 1024, LastModified = DateTime.Now, Hash = "abc123" });
            Console.WriteLine($"Contains C:/file1.txt: {fileCache.Contains("C:/file1.txt")}");
            Console.WriteLine($"File size: {fileCache.Get("C:/file1.txt")?.Size}");

            var tagManager = new TagManager();
            tagManager.AddTag("art1", "C#");
            tagManager.AddTag("art1", "NET");
            tagManager.AddTag("art2", "NET");
            tagManager.AddTag("art2", "Architecture");

            Console.WriteLine($"All unique tags: {string.Join(", ", tagManager.GetAllUniqueTags())}");
            Console.WriteLine($"Common tags (art1 & art2): {string.Join(", ", tagManager.FindCommonTags("art1", "art2"))}");

            var searchIndex = new SearchIndex();
            searchIndex.AddDocument(1, "C# is a great programming language");
            searchIndex.AddDocument(2, "Java and C# are popular language options");

            Console.WriteLine($"Search 'language': {string.Join(", ", searchIndex.Search("language"))}");
            Console.WriteLine($"Search 'C#' AND 'popular': {string.Join(", ", searchIndex.SearchMultiple("C#", "popular"))}");
        }
    }
}