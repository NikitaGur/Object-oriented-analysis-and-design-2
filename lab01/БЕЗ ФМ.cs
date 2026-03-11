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
    }

    // Класс Letter (Письмо)
    public class Letter : Document
    {
        public string LetterType { get; private set; }
        public string Correspondent { get; private set; }

        public Letter(int number, DateTime date, string content,
                      string letterType, string correspondent)
            : base(number, date, content)
        {
            LetterType = letterType;
            Correspondent = correspondent;
        }

        public override string GetDetails()
        {
            return $"Письмо №{Number} от {Date.ToShortDateString()}\n" +
                   $"Тип: {LetterType}\n" +
                   $"Корреспондент: {Correspondent}\n" +
                   $"Содержание: {Content}";
        }
    }

    // Класс Order (Приказ)
    public class Order : Document
    {
        public string Department { get; private set; }
        public DateTime Deadline { get; private set; }
        public string Responsible { get; private set; }

        public Order(int number, DateTime date, string content,
                     string department, DateTime deadline, string responsible)
            : base(number, date, content)
        {
            Department = department;
            Deadline = deadline;
            Responsible = responsible;
        }

        public override string GetDetails()
        {
            return $"Приказ №{Number} от {Date.ToShortDateString()}\n" +
                   $"Подразделение: {Department}\n" +
                   $"Срок выполнения: {Deadline.ToShortDateString()}\n" +
                   $"Ответственный: {Responsible}\n" +
                   $"Содержание: {Content}";
        }
    }

    // Класс BusinessTripOrder (Распоряжение о командировке)
    public class BusinessTripOrder : Document
    {
        public string Employee { get; private set; }
        public string Period { get; private set; }
        public string Destination { get; private set; }

        public BusinessTripOrder(int number, DateTime date, string content,
                                 string employee, string period, string destination)
            : base(number, date, content)
        {
            Employee = employee;
            Period = period;
            Destination = destination;
        }

        public override string GetDetails()
        {
            return $"Распоряжение о командировке №{Number} от {Date.ToShortDateString()}\n" +
                   $"Сотрудник: {Employee}\n" +
                   $"Период: {Period}\n" +
                   $"Место назначения: {Destination}\n" +
                   $"Содержание: {Content}";
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
                               string letterType = null, string correspondent = null,
                               string department = null, DateTime? deadline = null,
                               string responsible = null,
                               string employee = null, string period = null,
                               string destination = null)
        {
            Document newDoc = null;

            if (docType == "letter")
            {
                newDoc = new Letter(
                    currentNumber,
                    date,
                    content,
                    letterType ?? "входящее",
                    correspondent ?? "Неизвестный"
                );
            }
            else if (docType == "order")
            {
                newDoc = new Order(
                    currentNumber,
                    date,
                    content,
                    department ?? "Неизвестное подразделение",
                    deadline ?? DateTime.Today,
                    responsible ?? "Неизвестный"
                );
            }
            else if (docType == "business_trip")
            {
                newDoc = new BusinessTripOrder(
                    currentNumber,
                    date,
                    content,
                    employee ?? "Неизвестный сотрудник",
                    period ?? "Неизвестный период",
                    destination ?? "Неизвестное место"
                );
            }
            else
            {
                throw new ArgumentException("Неизвестный тип документа");
            }

            documents.Add(newDoc);
            currentNumber++;
        }

        public void ShowAllDocuments()
        {
            if (documents.Count == 0)
            {
                Console.WriteLine("Список документов пуст.");
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
            while (true)
            {
                Console.WriteLine("\n=== Система управления документами ===");
                Console.WriteLine("1. Добавить документ");
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

            switch (typeChoice)
            {
                case "1":
                    Console.Write("Тип письма (входящее/исходящее): ");
                    string letterType = Console.ReadLine();
                    Console.Write("Корреспондент: ");
                    string correspondent = Console.ReadLine();
                    AddDocument("letter", date, content, letterType, correspondent);
                    Console.WriteLine("Письмо добавлено!");
                    break;

                case "2":
                    Console.Write("Подразделение: ");
                    string department = Console.ReadLine();
                    Console.Write("Срок выполнения (дд.мм.гггг): ");
                    DateTime? deadline = null;
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null,
                        System.Globalization.DateTimeStyles.None, out DateTime dl))
                    {
                        deadline = dl;
                    }
                    else
                    {
                        deadline = DateTime.Today;
                    }
                    Console.Write("Ответственный исполнитель: ");
                    string responsible = Console.ReadLine();
                    AddDocument("order", date, content,department: department,deadline: deadline,responsible: responsible);
                    Console.WriteLine("Приказ добавлен!");
                    break;

                case "3":
                    Console.Write("Сотрудник: ");
                    string employee = Console.ReadLine();
                    Console.Write("Период: ");
                    string period = Console.ReadLine();
                    Console.Write("Место назначения: ");
                    string destination = Console.ReadLine();
                    AddDocument("business_trip", date, content, employee: employee,
                               period: period, destination: destination);
                    Console.WriteLine("Распоряжение о командировке добавлено!");
                    break;

                default:
                    Console.WriteLine("Неверный выбор типа документа.");
                    break;
            }
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