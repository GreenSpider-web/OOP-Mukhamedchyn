using System;
using System.Collections.Generic;

namespace Lab5
{
    public class Alert
    {
        private string _message;

        public string Message => _message;
        public string TriggerDescription { get; protected set; } = string.Empty;

        public Alert(string message)
        {
            _message = message;
        }

        public virtual void Trigger()
        {
            TriggerDescription = $"Сповіщення: {_message}";
            Console.WriteLine(TriggerDescription);
        }
    }

    public class EmailAlert : Alert
    {
        private string _recipientEmail;

        public string RecipientEmail => _recipientEmail;

        public EmailAlert(string message, string recipientEmail)
            : base(message)
        {
            _recipientEmail = recipientEmail;
        }

        public override void Trigger()
        {
            TriggerDescription = $"Email на {_recipientEmail}: {Message}";
            Console.WriteLine($"[EmailAlert] {TriggerDescription}");
        }
    }

    public class SMSAlert : Alert
    {
        private string _phoneNumber;

        public string PhoneNumber => _phoneNumber;

        public SMSAlert(string message, string phoneNumber)
            : base(message)
        {
            _phoneNumber = phoneNumber;
        }

        public override void Trigger()
        {
            TriggerDescription = $"SMS на {_phoneNumber}: {Message}";
            Console.WriteLine($"[SMSAlert] {TriggerDescription}");
        }
    }

    public class PushAlert : Alert
    {
        private string _appId;

        public string AppId => _appId;

        public PushAlert(string message, string appId)
            : base(message)
        {
            _appId = appId;
        }

        public override void Trigger()
        {
            TriggerDescription = $"Push-повідомлення для застосунку {_appId}: {Message}";
            Console.WriteLine($"[PushAlert] {TriggerDescription}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрація поліморфних сповіщень\n");

            List<Alert> alerts = new List<Alert>
            {
                new EmailAlert("Виявлено новий вхід до облікового запису.", "student@example.com"),
                new SMSAlert("Потрібно підтвердити операцію.", "+380501234567"),
                new PushAlert("Доступне нове повідомлення.", "oop-mobile-app")
            };

            var triggeredAlerts = new List<string>();

            Console.WriteLine("Виклик virtual/override через колекцію Alert:");
            foreach (Alert alert in alerts)
            {
                alert.Trigger();
                triggeredAlerts.Add(alert.TriggerDescription);
            }

            Console.WriteLine("\nАгрегація спрацьованих сповіщень:");
            Console.WriteLine($"Усього сповіщень: {triggeredAlerts.Count}");
            foreach (string triggeredAlert in triggeredAlerts)
            {
                Console.WriteLine($"- {triggeredAlert}");
            }
        }
    }
}
