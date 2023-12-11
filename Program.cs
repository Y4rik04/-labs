using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace lab4
{
    public class Library
    {
        enum BooksOperations
        {
            DirectTo = 1,
            ReturnBack,
        }
        enum BookOptions
        {
            Availability = 1,
            ShowReaders,
            DeleteBook,
        }
        class Books
        {
            private LinkedList<string> Path { get; set; }
            public string Title { get; set; }
            public DateTime TakingTime { get; set; }
            public DateTime ReturningTime { get; set; }
            public Books()
            {
                Title = String.Empty;
                TakingTime = new DateTime();
                ReturningTime = new DateTime();
                Path = new LinkedList<string>();
            }
            public void GiveTo(string name)
            {
                Path.AddLast(name);
            }
            public void ReturnBack()
            {
                Path.RemoveLast();
            }
            public void ShowPath()
            {
                foreach (var name in Path)
                {
                    Console.Write($"{name} -> ");
                }
            }
            public override string ToString()
            {
                return $"\tКнига: {Title}" + $" взята {TakingTime.ToString("dd.MM.yyyy")}";
            }
        }
        private Dictionary<Books, string> Readers { get; set; }
        private Queue<Books> TakenBooks { get; set; }
        public Library()
        {
            // Створити порожні колекції
            Readers = new Dictionary<Books, string>();
            TakenBooks = new Queue<Books>();
        }
        private string BookReader()
        {
            Console.WriteLine($"Хто бере книжку");
            return Console.ReadLine();
        }

        private void OutputReaders()
        {
            int index = 1;
            foreach (var reader in Readers)
            {
                Console.WriteLine($"{index}.)\tІм'я:\t{reader.Value}");
                Console.WriteLine($"{reader.Key}");
                index++;
            }
        }
        private Books SelectBook()
        {
            Console.WriteLine($"Оберіть книгу:");
            OutputReaders();
            int selected_book;
            while (!int.TryParse(Console.ReadLine(), out selected_book))
            {
                Console.WriteLine($"ПОМИЛКА, СПРОБУЙТЕ ЗНОВУ");
            }
            if (selected_book > Readers.Keys.Count)
                return null;
            return Readers.Keys.ElementAt(selected_book - 1);
        }
        private void ReturnBook()
        {
            var book = SelectBook();
            book?.ReturnBack();
            Console.WriteLine($"Книжку повернуто");
        }
        private void TakeBook()
        {
            var rand = new Random();
            var titles = new StringCollection() { $"Буквар", $"Дюна", $"Керрі", $"Ідіот", $"Зелена миля" };
            int randomTitle = rand.Next(titles.Count);
            var newBook = new Books()
            {
                Title = $"{titles[randomTitle]}",
                TakingTime = DateTime.Now,
            };
            Console.WriteLine($"Доступна книга:");
            Console.WriteLine($"{newBook}");
            var reader = BookReader();
            newBook.GiveTo(reader);
            TakenBooks.Enqueue(newBook);
            Readers.TryAdd(newBook, reader);
        }
        public void Do()
        {
            Console.WriteLine($"Оберіть дію з книгою:");
            Console.WriteLine($"1.) Взяти книгу");
            Console.WriteLine($"2.) Переглянути, у кого книга");
            Console.WriteLine($"3.) Віддати книгу");
            Byte what_to_do;
            while (!Byte.TryParse(Console.ReadLine(), out what_to_do))
            {
                Console.WriteLine($"ПОМИЛКА, СПРОБУЙТЕ ЗНОВУ");
            }
            BookOptions selectedOption = (BookOptions)what_to_do;
            Console.WriteLine(Environment.NewLine);
            switch (selectedOption)
            {
                case BookOptions.Availability:
                    TakeBook();
                    break;
                case BookOptions.ShowReaders:
                    OutputReaders();
                    break;
                case BookOptions.DeleteBook:
                    ReturnBook();
                    break;
                default:
                    break;
            }
        }
    }
    class Program
    {
        static Library library = new Library();
        enum ProgrammOptions : byte
        {
            Execute = 1,
            Exit,
        }
        static void Programm()
        {
            Byte programmOptions;
            Console.WriteLine($"Введіть вашу дію:");
            Console.WriteLine($"1.) Розпочати роботу");
            Console.WriteLine($"2.) Завершити роботу");

            while (!Byte.TryParse(Console.ReadLine(), out programmOptions))
            {
                Console.WriteLine($"ПОМИЛКА, СПРОБУЙТЕ ЗНОВУ");
            }
            ProgrammOptions selectedOption = (ProgrammOptions)programmOptions;
            switch (selectedOption)
            {
                case ProgrammOptions.Execute:
                    library.Do();
                    Console.ReadLine();
                    break;
                case ProgrammOptions.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Programm();
            }
        }
    }
}