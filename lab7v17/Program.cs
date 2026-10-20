using System.Collections.Generic;

namespace Lab7v17;

public interface IEntity
{
    int Id { get; set; }
}

public interface IRepository<T> where T : IEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T item);
    void Update(T item);
    void Delete(int id);
}

public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new();

    public T GetById(int id)
    {
        T? item = _items.FirstOrDefault(candidate => candidate.Id == id);
        return item ?? throw new KeyNotFoundException($"Елемент з ID {id} не знайдено.");
    }

    public IEnumerable<T> GetAll()
    {
        return _items.ToList();
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Any(existing => existing.Id == item.Id))
        {
            throw new InvalidOperationException($"Елемент з ID {item.Id} вже існує.");
        }

        _items.Add(item);
    }

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        int index = _items.FindIndex(existing => existing.Id == item.Id);
        if (index < 0)
        {
            throw new KeyNotFoundException($"Елемент з ID {item.Id} не знайдено.");
        }

        _items[index] = item;
    }

    public void Delete(int id)
    {
        int index = _items.FindIndex(item => item.Id == id);
        if (index < 0)
        {
            throw new KeyNotFoundException($"Елемент з ID {id} не знайдено.");
        }

        _items.RemoveAt(index);
    }
}

public class Comment : IEntity
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string AuthorName { get; set; }
    public int PostId { get; set; }

    public Comment(int id, string text, string authorName, int postId)
    {
        Id = id;
        Text = text;
        AuthorName = authorName;
        PostId = postId;
    }

    public override string ToString()
    {
        return $"#{Id}: {AuthorName}: {Text} (публікація {PostId})";
    }
}

public static class Program
{
    public static void Main()
    {
        var repository = new InMemoryRepository<Comment>();

        repository.Add(new Comment(1, "Дуже корисна публікація.", "Олена", 10));
        repository.Add(new Comment(2, "Дякую за пояснення!", "Андрій", 10));

        Console.WriteLine("Усі коментарі після додавання:");
        PrintComments(repository.GetAll());

        Console.WriteLine("\nОтримання коментаря за ID:");
        Console.WriteLine(repository.GetById(1));

        repository.Update(new Comment(2, "Дякую за детальне пояснення!", "Андрій", 10));
        Console.WriteLine("\nПісля оновлення коментаря:");
        PrintComments(repository.GetAll());

        repository.Delete(1);
        Console.WriteLine("\nПісля видалення коментаря:");
        PrintComments(repository.GetAll());
    }

    private static void PrintComments(IEnumerable<Comment> comments)
    {
        foreach (Comment comment in comments)
        {
            Console.WriteLine(comment);
        }
    }
}
