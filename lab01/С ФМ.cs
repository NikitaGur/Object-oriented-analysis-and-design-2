using System;
using System.Collections.Generic;

namespace DocumentSystem
{
    // Абстрактный класс Document
    public abstract class Document
    {
        public int Number { get; protected set; }
        public DateTime Date { get; protected set; }
        public string Content { get; protected set; }

        public Document(int number, DateTime date, string content)
        {
            Number = number;
            Date = date;
            Content = content;
        }

        public abstract string GetDetails();
        public abstract void SaveDocumentInput(Dictionary<string, string> parameters);
    }

    // Класс Letter (Письмо)
    public class Letter : Document
    {
        public string LetterType { get; private set; }
        public string Correspondent { get; private set; }

        public Letter(int number, DateTime date, string content)
            : base(number, date, content)
        {
        }

        public override string GetDetails()
        {
            return $"Письмо №{Number} от {Date.ToShortDateString()}\n" +
                   $"Тип: {LetterType}\n" +
                   $"Корреспондент: {Correspondent}\n" +
                   $"Содержание: {Content}";
        }

        public override void SaveDocumentInput(Dictionary<string, string> parameters)
        {
            LetterType = parameters.ContainsKey("letter_type") ? parameters["letter_type"] : "входящее";
            Correspondent = parameters.ContainsKey("correspondent") ? parameters["correspondent"] : "Неизвестный";
        }
    }

    // Класс Order (Приказ)
    public class Order : Document
    {
        public string Department { get; private set; }
        public DateTime Deadline { get; private set; }
        public string Responsible { get; private set; }

        public Order(int number, DateTime date, string content)
            : base(number, date, content)
        {
        }

        public override string GetDetails()
        {
            return $"Приказ №{Number} от {Date.ToShortDateString()}\n" +
                   $"Подразделение: {Department}\n" +
                   $"Срок выполнения: {Deadline.ToShortDateString()}\n" +
                   $"Ответственный: {Responsible}\n" +
                   $"Содержание: {Content}";
        }

        public override void SaveDocumentInput(Dictionary<string, string> parameters)
        {
            Department = parameters.ContainsKey("department") ? parameters["department"] : "Неизвестное подразделение";
            Deadline = parameters.ContainsKey("deadline") && DateTime.TryParse(parameters["deadline"], out var dl)
                ? dl : DateTime.Today;
            Responsible = parameters.ContainsKey("responsible") ? parameters["responsible"] : "Неизвестный";
        }
    }

    // Класс BusinessTripOrder (Распоряжение о командировке)
    public class BusinessTripOrder : Document
    {
        public string Employee { get; private set; }
        public string Period { get; private set; }
        public string Destination { get; private set; }

        public BusinessTripOrder(int number, DateTime date, string content)
            : base(number, date, content)
        {
        }

        public override string GetDetails()
        {
            return $"Распоряжение о командировке №{Number} от {Date.ToShortDateString()}\n" +
                   $"Сотрудник: {Employee}\n" +
                   $"Период: {Period}\n" +
                   $"Место назначения: {Destination}\n" +
                   $"Содержание: {Content}";
        }

        public override void SaveDocumentInput(Dictionary<string, string> parameters)
        {
            Employee = parameters.ContainsKey("employee") ? parameters["employee"] : "Неизвестный сотрудник";
            Period = parameters.ContainsKey("period") ? parameters["period"] : "Неизвестный период";
            Destination = parameters.ContainsKey("destination") ? parameters["destination"] : "Неизвестное место";
        }
    }

    // Фабрика для создания писем
    public class LetterFactory
    {
        public static Dictionary<string, string> AddDocumentInput()
        {
            Console.WriteLine("\n=== Ввод данных для письма ===");
            var parameters = new Dictionary<string, string>();

            Console.Write("Тип письма (входящее/исходящее): ");
            parameters["letter_type"] = Console.ReadLine();

            Console.Write("Корреспондент: ");
            parameters["correspondent"] = Console.ReadLine();

            return parameters;
        }
    }

    // Фабрика для создания приказов
    public class OrderFactory
    {
        public static Dictionary<string, string> AddDocumentInput()
        {
            Console.WriteLine("\n=== Ввод данных для приказа ===");
            var parameters = new Dictionary<string, string>();

            Console.Write("Подразделение: ");
            parameters["department"] = Console.ReadLine();

            Console.Write("Срок выполнения (дд.мм.гггг): ");
            parameters["deadline"] = Console.ReadLine();

            Console.Write("Ответственный исполнитель: ");
            parameters["responsible"] = Console.ReadLine();

            return parameters;
        }
    }

    // Фабрика для создания распоряжений о командировке
    public class BusinessTripOrderFactory
    {
        public static Dictionary<string, string> AddDocumentInput()
        {
            Console.WriteLine("\n=== Ввод данных для распоряжения о командировке ===");
            var parameters = new Dictionary<string, string>();

            Console.Write("Сотрудник: ");
            parameters["employee"] = Console.ReadLine();

            Console.Write("Период: ");
            parameters["period"] = Console.ReadLine();

            Console.Write("Место назначения: ");
            parameters["destination"] = Console.ReadLine();

            return parameters;
        }
    }

    // Фабричный метод для создания документов различных типов
    public class DocumentFactory
    {
        public static Dictionary<string, string> AddDocumentInput(string docType)
        {
            switch (docType.ToLower())
            {
                case "letter":
                case "письмо":
                    return LetterFactory.AddDocumentInput();

                case "order":
                case "приказ":
                    return OrderFactory.AddDocumentInput();

                case "business_trip":
                case "командировка":
                    return BusinessTripOrderFactory.AddDocumentInput();

                default:
                    throw new ArgumentException("Неизвестный тип документа");
            }
        }

        public static Document CreateDocument(string docType, int number, DateTime date, string content,
                                             Dictionary<string, string> parameters)
        {
            Document doc = null;

            switch (docType.ToLower())
            {
                case "letter":
                case "письмо":
                    doc = new Letter(number, date, content);
                    break;

                case "order":
                case "приказ":
                    doc = new Order(number, date, content);
                    break;

                case "business_trip":
                case "командировка":
                    doc = new BusinessTripOrder(number, date, content);
                    break;

                default:
                    throw new ArgumentException("Неизвестный тип документа");
            }

            doc.SaveDocumentInput(parameters);
            return doc;
        }
    }

    // Основной класс приложения для управления документами
    public class DocumentManagementApp
    {
        private List<Document> documents;
        private int currentNumber;

        public DocumentManagementApp()
        {
            documents = new List<Document>();
            currentNumber = 1;
        }

        public void AddDocument(string docType, DateTime date, string content,
                               Dictionary<string, string> parameters)
        {
            Document doc = DocumentFactory.CreateDocument(docType, currentNumber, date, content, parameters);
            documents.Add(doc);
            currentNumber++;
        }

        public void ShowAllDocuments()
        {
            if (documents.Count == 0)
            {
                Console.WriteLine("\nСписок документов пуст.");
                return;
            }

            Console.WriteLine("\n=== Полный перечень документов ===");
            foreach (var doc in documents)
            {
                Console.WriteLine(doc.GetDetails());
                Console.WriteLine(new string('-', 50));
            }
        }

        public void FindDocumentByNumber(int number)
        {
            Console.WriteLine($"\n=== Поиск документа №{number} ===");

            foreach (var doc in documents)
            {
                if (doc.Number == number)
                {
                    Console.WriteLine(doc.GetDetails());
                    return;
                }
            }

            Console.WriteLine($"Документ с номером {number} не найден.");
        }

        public void Run()
        {
            Console.WriteLine("=== Система управления документами (с фабричным методом) ===");

            while (true)
            {
                Console.WriteLine("\n1. Добавить документ");
                Console.WriteLine("2. Показать все документы");
                Console.WriteLine("3. Найти документ по номеру");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddDocumentMenu();
                        break;
                    case "2":
                        ShowAllDocuments();
                        break;
                    case "3":
                        Console.Write("Введите номер документа: ");
                        if (int.TryParse(Console.ReadLine(), out int number))
                        {
                            FindDocumentByNumber(number);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат номера.");
                        }
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        private void AddDocumentMenu()
        {
            Console.WriteLine("\n=== Добавление нового документа ===");
            Console.WriteLine("1. Письмо");
            Console.WriteLine("2. Приказ");
            Console.WriteLine("3. Распоряжение о командировке");
            Console.Write("Выберите тип документа: ");

            string typeChoice = Console.ReadLine();
            string docType = "";

            switch (typeChoice)
            {
                case "1":
                    docType = "letter";
                    break;
                case "2":
                    docType = "order";
                    break;
                case "3":
                    docType = "business_trip";
                    break;
                default:
                    Console.WriteLine("Неверный выбор типа документа.");
                    return;
            }

            Console.Write("Дата (дд.мм.гггг или Enter для сегодня): ");
            string dateInput = Console.ReadLine();
            DateTime date = DateTime.Today;

            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                if (!DateTime.TryParseExact(dateInput, "dd.MM.yyyy", null,
                    System.Globalization.DateTimeStyles.None, out date))
                {
                    Console.WriteLine("Неверный формат даты. Используется сегодняшняя дата.");
                    date = DateTime.Today;
                }
            }

            Console.Write("Содержание: ");
            string content = Console.ReadLine();

            // Используем фабричный метод для получения параметров
            var parameters = DocumentFactory.AddDocumentInput(docType);
            parameters["content"] = content;

            AddDocument(docType, date, content, parameters);
            Console.WriteLine("Документ успешно добавлен!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new DocumentManagementApp();
            app.Run();
        }
    }
}