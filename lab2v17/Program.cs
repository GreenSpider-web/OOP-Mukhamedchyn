using System;

namespace OOP_Mukhamedchyn
{
    public class Course
    {
        // Приватні поля
        private string _title = string.Empty;
        private string _teacherName = string.Empty;
        private int _credits;

        // Властивості з валідацією
        public string Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value) ? "New Course" : value;
        }

        public string TeacherName
        {
            get => _teacherName;
            set => _teacherName = string.IsNullOrWhiteSpace(value) ? "N/A" : value;
        }

        public int Credits
        {
            get => _credits;
            set
            {
                if (value <= 0)
                {
                    Console.WriteLine("Помилка: Кількість кредитів має бути більше 0! Встановлено значення за замовчуванням (1).");
                    _credits = 1;
                }
                else
                {
                    _credits = value;
                }
            }
        }

        // 1. Основний параметризований конструктор
        public Course(string title, string teacherName, int credits)
        {
            Title = title;
            TeacherName = teacherName;
            Credits = credits;
            Console.WriteLine($"[Конструктор] Створено курс: \"{Title}\"");
        }

        // 2. Конструктор за замовчуванням (викликає параметризований через : this())
        public Course() : this("New Course", "N/A", 3)
        {
        }

        // Метод зарахування студента
        public void EnrollStudent(string studentName)
        {
            Console.WriteLine($"Студента {studentName} зараховано на курс \"{Title}\" ({Credits} кр., викл. {TeacherName}).");
        }

        // Деструктор
        ~Course()
        {
            Console.WriteLine($"[Деструктор] Об'єкт курсу \"{_title}\" знищено з пам'яті.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Створення об'єктів ===");

            // 1. Виклик конструктора за замовчуванням
            Course course1 = new Course();
            course1.EnrollStudent("Олексій");

            // 2. Виклик параметризованого конструктора
            Course course2 = new Course("Об'єктно-орієнтоване програмування", "Іванов І.І.", 5);
            course2.EnrollStudent("Марія");

            // 3. Перевірка валідації некоректних даних
            Course course3 = new Course("Тестування ПЗ", "Петров П.П.", -2);
            course3.EnrollStudent("Тарас");

            Console.WriteLine("\n=== Кінець роботи Main, підготовка до GC ===");

            // Обнуляємо посилання, щоб об'єкти стали доступними для збирання сміття
            course1 = null;
            course2 = null;
            course3 = null;

            // Примусовий виклик Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Завершення виконання програми.");
        }
    }
}