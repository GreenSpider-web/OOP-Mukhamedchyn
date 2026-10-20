using Lab7v17;
using Xunit;

namespace Lab7v17.Tests;

public class RepositoryTests
{
    [Fact]
    public void Add_AddsNewComment()
    {
        var repository = new InMemoryRepository<Comment>();
        var comment = new Comment(1, "Текст", "Автор", 10);

        repository.Add(comment);

        Assert.Same(comment, repository.GetById(1));
    }

    [Fact]
    public void Add_ThrowsForDuplicateId()
    {
        var repository = new InMemoryRepository<Comment>();
        repository.Add(new Comment(1, "Перший", "Автор", 10));

        Assert.Throws<InvalidOperationException>(() =>
            repository.Add(new Comment(1, "Дублікат", "Інший автор", 11)));
    }

    [Fact]
    public void GetById_ReturnsCommentById()
    {
        var repository = CreateRepository();

        Comment result = repository.GetById(2);

        Assert.Equal("Другий коментар", result.Text);
        Assert.Equal("Марія", result.AuthorName);
    }

    [Fact]
    public void GetById_ThrowsForMissingId()
    {
        var repository = CreateRepository();

        Assert.Throws<KeyNotFoundException>(() => repository.GetById(99));
    }

    [Fact]
    public void GetAll_ReturnsAllComments()
    {
        var repository = CreateRepository();

        Comment[] comments = repository.GetAll().ToArray();

        Assert.Equal(2, comments.Length);
        Assert.Equal(new[] { 1, 2 }, comments.Select(comment => comment.Id));
    }

    [Fact]
    public void Update_ReplacesExistingComment()
    {
        var repository = CreateRepository();
        var updated = new Comment(1, "Оновлений коментар", "Олена", 12);

        repository.Update(updated);

        Assert.Same(updated, repository.GetById(1));
    }

    [Fact]
    public void Update_ThrowsForMissingId()
    {
        var repository = CreateRepository();

        Assert.Throws<KeyNotFoundException>(() =>
            repository.Update(new Comment(99, "Новий", "Автор", 10)));
    }

    [Fact]
    public void Delete_RemovesExistingComment()
    {
        var repository = CreateRepository();

        repository.Delete(1);

        Assert.Single(repository.GetAll());
        Assert.Throws<KeyNotFoundException>(() => repository.GetById(1));
    }

    [Fact]
    public void Delete_ThrowsForMissingId()
    {
        var repository = CreateRepository();

        Assert.Throws<KeyNotFoundException>(() => repository.Delete(99));
    }

    private static InMemoryRepository<Comment> CreateRepository()
    {
        var repository = new InMemoryRepository<Comment>();
        repository.Add(new Comment(1, "Перший коментар", "Олена", 10));
        repository.Add(new Comment(2, "Другий коментар", "Марія", 10));
        return repository;
    }
}
