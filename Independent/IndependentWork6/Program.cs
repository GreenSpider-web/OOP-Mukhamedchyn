using System;
using System.Collections.Generic;

namespace IndependentWork6
{
    public interface INotifier
    {
        void Send(string message);
    }

    public class EmailNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"EmailNotifier: Відправка e-mail -> {message}");
        }
    }

    public class SmsNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"SmsNotifier: Відправка SMS -> {message}");
        }
    }

    public class PushNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"PushNotifier: Відправка Push-сповіщення -> {message}");
        }
    }

    public abstract class FileProcessor
    {
        public string FilePath { get; private set; }

        protected FileProcessor(string filePath)
        {
            FilePath = filePath;
        }

        public void OpenFile()
        {
            Console.WriteLine($"FileProcessor: Відкриття файлу '{FilePath}'...");
        }

        public void CloseFile()
        {
            Console.WriteLine($"FileProcessor: Закриття файлу '{FilePath}'. Ресурси звільнено.");
        }

        protected abstract void ProcessContent();

        public void Process()
        {
            OpenFile();
            ProcessContent();
            CloseFile();
        }
    }

    public class TextFileProcessor : FileProcessor
    {
        public TextFileProcessor(string filePath) : base(filePath) { }

        protected override void ProcessContent()
        {
            Console.WriteLine(" TextFileProcessor: Зчитано текстовий вміст та видалено зайві пробіли.");
        }
    }

    public class XmlFileProcessor : FileProcessor
    {
        public XmlFileProcessor(string filePath) : base(filePath) { }

        protected override void ProcessContent()
        {
            Console.WriteLine(" XmlFileProcessor: Виконано парсинг XML-структури та перевірку XSD-схеми.");
        }
    }

    public class JsonFileProcessor : FileProcessor
    {
        public JsonFileProcessor(string filePath) : base(filePath) { }

        protected override void ProcessContent()
        {
            Console.WriteLine(" JsonFileProcessor: Виконано десеріалізацію JSON-документа у дерева об'єктів.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<INotifier> notifiers = new List<INotifier>
            {
                new EmailNotifier(),
                new SmsNotifier(),
                new PushNotifier()
            };

            foreach (INotifier notifier in notifiers)
            {
                notifier.Send("Системне сповіщення: Вашу сесію успішно відновлено.");
            }

            List<FileProcessor> processors = new List<FileProcessor>
            {
                new TextFileProcessor("document.txt"),
                new XmlFileProcessor("config.xml"),
                new JsonFileProcessor("data.json")
            };

            foreach (FileProcessor processor in processors)
            {
                processor.Process();
            }
        }
    }
}