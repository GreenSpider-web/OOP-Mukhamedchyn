using System;
using System.Collections.Generic;

namespace EncapsulationAndValidationDemo
{
    public class UserProfile
    {
        private readonly int _id;
        private string _username = string.Empty;
        private int _age;
        private readonly Dictionary<string, string> _attributes = new();

        public int Id => _id;

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                    throw new ArgumentException("Ім'я повинно містити принаймні 3 символи!", nameof(value));

                _username = value.Trim();
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value < 0 || value > 150)
                    throw new ArgumentOutOfRangeException(nameof(value), "Некоректний вік!");

                _age = value;
            }
        }

        public UserProfile(int id, string username, int age)
        {
            if (id <= 0) throw new ArgumentException("ID повинен бути більшим за 0!");
            _id = id;
            Username = username;
            Age = age;
        }

        public string this[string key]
        {
            get => _attributes.TryGetValue(key, out var val) ? val : "Не вказано";
            set => _attributes[key] = value ?? string.Empty;
        }

        public static explicit operator string(UserProfile profile)
        {
            return $"User #{profile.Id}: {profile.Username}, {profile.Age} років";
        }
    }

    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                var user = new UserProfile(1, "Alex_Dev", 20);
                user["Role"] = "Developer";

                Console.WriteLine("Успішно створено:");
                Console.WriteLine((string)user);
                Console.WriteLine($"Роль: {user["Role"]}\n");

                Console.WriteLine("Спроба встановити невалідний вік:");
                user.Age = -5;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ПОМИЛКА ВАЛІДАЦІЇ]: {ex.Message}");
            }
        }
    }
}