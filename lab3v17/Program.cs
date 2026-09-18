using System;

namespace Lab3
{
    public class PrinterConnection : IDisposable
    {
        private bool _disposed = false;
        private bool _isConnected;
        private string _printerName;

        public string PrinterName => _printerName;
        public bool IsConnected => _isConnected;

        public PrinterConnection(string printerName)
        {
            _printerName = printerName;
            _isConnected = true;
            Console.WriteLine($"[Конструктор]: Встановлено з'єднання з принтером '{_printerName}'");
        }

        public void Print(string document)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(PrinterConnection), "Спроба звернутися до об'єкта, який вже знищено.");
            }

            if (_isConnected)
            {
                Console.WriteLine($"[Друк {_printerName}]: Друкується документ '{document}'");
            }
            else
            {
                Console.WriteLine($"[Помилка]: З'єднання з принтером '{_printerName}' закрите.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine($"[Dispose(true)]: Звільнення керованих ресурсів для '{_printerName}'");
                }

                if (_isConnected)
                {
                    Console.WriteLine($"[Dispose]: Закриття з'єднання з принтером '{_printerName}'");
                    _isConnected = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Деструктор (фіналізатор)
        ~PrinterConnection()
        {
            Console.WriteLine($"[Деструктор]: Автоматичний виклик для '{_printerName}'");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Сценарій 1: Використання оператора using");
            using (var printer1 = new PrinterConnection("HP LaserJet Pro"))
            {
                printer1.Print("Звіт_2026.pdf");
            }

            Console.WriteLine("\nСценарій 2: Явний виклик Dispose()");
            var printer2 = new PrinterConnection("Canon PIXMA");
            printer2.Print("Курсова_робота.docx");
            printer2.Dispose();

            Console.WriteLine("\n Сценарій 3: Без Dispose() (робота деструктора через GC) ");
            CreateAndAbandonObject();

            // Примусовий виклик збирача сміття та очікування фіналізаторів
            Console.WriteLine("Викликаємо GC.Collect() та очікуємо роботу деструктора...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nПрограму завершено.");
        }

        static void CreateAndAbandonObject()
        {
            var printer3 = new PrinterConnection("Epson L3150");
            printer3.Print("Тестова_сторінка.png");
        }
    }
}