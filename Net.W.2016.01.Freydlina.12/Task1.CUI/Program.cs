using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Task2;

namespace Task1.CUI
{
    class Program
    {
        private static BookListService service;
        private static Logger.Logger logger = new Logger.Logger();
        private static string fileName =
                @"C:\Users\Polina\Documents\git\Net.W.2016.01.Freydlina.12\Net.W.2016.01.Freydlina.12\Net.W.2016.01.Freydlina.12\test";
        private static BooksBinaryFileStorage storage = new BooksBinaryFileStorage(logger,fileName);

        static void Main(string[] args)
        {
            while (true)
            {
                MainMenu();
            }
        }

        private static void MainMenu()
        {
            OutMainMenu();
            string action = Console.ReadLine();
            logger.Debug(action + " selected by user");
            switch (action)
            {
                case "1": CreateBookService();
                    break;
                case "2": service = new BookListService(logger);
                    service.OpenFrom(storage);
                    break;
                case "3": Console.WriteLine(service.PrintBooks());
                    break;
                case "4": BooksMenu(AddBook);
                    break;
                case "5": BooksMenu(RemoveBook);
                    break;
                case "6": SortTagsMenu();
                    break;
                case "7": FindTagsMenu();
                    break;
                case "8": service.SaveTo(storage);
                    break;
                case "0": Environment.Exit(0);
                    break;
                default: Console.WriteLine("Invalid command");
                    break;
            }
        }

        private static void OutMainMenu()
        {
            Console.WriteLine("Menu:"+
                "\n1. Create BookService" +
                "\n2. Load BookService from file" +
                "\n---" +
                "\n3. Print books from BookService" +
                "\n4. Add book to BookService ..." +
                "\n5. Remove book from BookService ..." +
                "\n6. Sort books in BookService by ..." +
                "\n7. Find books in BookService by ..." +
                "\n---" +
                "\n8. Save books from BookService to file" +
                "\n---" +
                "\n0. Exit");
            Console.Write(">>");
        }

        private static void FindTagsMenu()
        {
            OutTagsMenu("Find");
            string tag = Console.ReadLine();
            switch (tag)
            {
                case "1":
                    Console.WriteLine("Enter author");
                    string author = Console.ReadLine();
                    PrintBooks(service.FindBooksByTag(b => b.Authors.Contains(author)));
                    break;
                case "2":
                    Console.WriteLine("Enter title");
                    string title = Console.ReadLine();
                    PrintBooks(service.FindBooksByTag(b => b.Title == title));
                    break;
                case "3":
                    Console.WriteLine("Enter publisher");
                    string publisher = Console.ReadLine();
                    PrintBooks(service.FindBooksByTag(b => b.Publisher == publisher));
                    break;
                case "4":
                    Console.WriteLine("Enter year of publishing");
                    int year = int.Parse(Console.ReadLine());
                    PrintBooks(service.FindBooksByTag(b => b.PublishingYear == year));
                    break;
                case "0": MainMenu();
                    break;
                default: Console.WriteLine("Invalid command");
                    break;
            }

        }

        private static void SortTagsMenu()
        {
            OutTagsMenu("Sort");
            string tag = Console.ReadLine();
            switch (tag)
            {
                case "1":
                    service.SortBooksByTag(b => b.Authors?[0]);
                    Console.WriteLine(service.PrintBooks());
                    break;
                case "2":
                    service.SortBooksByTag(b => b.Title);
                    Console.WriteLine(service.PrintBooks());
                    break;
                case "3":
                    service.SortBooksByTag(b => b.Publisher);
                    Console.WriteLine(service.PrintBooks());
                    break;
                case "4":
                    service.SortBooksByTag(b => b.PublishingYear);
                    Console.WriteLine(service.PrintBooks());
                    break;
                case "0":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }

        private static void OutTagsMenu(string action)
        {
            Console.WriteLine("Choose tag for "+action+":" +
                              "\n1. default (Author)" +
                              "\n2. Title" +
                              "\n3. Publisher" +
                              "\n4. Year of publishing" +
                              "\n---" +
                              "\n0. Cancel");
            Console.Write(">>");
        }

        private static void BooksMenu(Action<Book> action)
        {
            OutBooksMenu(action.Method.Name);
            string tag = Console.ReadLine();
            switch (tag)
            {
                case "1":
                    action(new Book("Haffner, Stacey. Announcing.NET Framework 4.6.2", "Microsoft", 2016));
                    break;
                case "2":
                    action(new Book("Understanding .NET Just-In-Time Compilation", "Telerik"));
                    break;
                case "3":
                    action(new Book("Understanding Garbage Collection in .NET"));
                    break;
                case "4":
                    action(new Book("CrossNet", "Codeplex.com", 2012));
                    break;
                case "5":
                    action(GetCustomBook());
                    break;
                case "0":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }

        private static void OutBooksMenu(string action)
        {
            Console.WriteLine(action + " book:" +
                              "\n1. Haffner, Stacey. Announcing.NET Framework 4.6.2 - Microsoft, 2016" +
                              "\n2. Understanding.NET Just-In-Time Compilation - Telerik" +
                              "\n3. Understanding Garbage Collection in .NET" +
                              "\n4. CrossNet - Codeplex.com, 2012" +
                              "\n5. Custom ..." +
                              "\n---" +
                              "\n0. Cancel");
            Console.Write(">>");
        }

        private static Book GetCustomBook()
        {
            Console.Write("Enter book Title: ");
            string title = Console.ReadLine();
            string authors = "";
            while (true)
            {
                Console.Write("Enter book author: ");
                string author = Console.ReadLine();
                Console.WriteLine("Add another author? Y/N");
                string add = Console.ReadLine()?.ToUpper();
                if (add == "N") break;
                authors += author + "|";
            }
            Console.Write("Enter book Publisher: ");
            string publisher = Console.ReadLine();
            Console.Write("Enter book Year of publishing: ");
            int year;
            if(!int.TryParse(Console.ReadLine(),out year)) year = 1000;
            return new Book(title,publisher,year,authors.Split('|'));
        }

        private static void CreateBookService()
        {
            CustomSet<Book> books = new CustomSet<Book>(3)
            {
                new Book("Announcing.NET Framework 4.6.2", "Microsoft", 2016, "Haffner, Stacey"),
                new Book(".NET Core - .NET Goes Cross - Platform with.NET Core", "Microsoft", 2016,
                    "Carter, Phillip", "Knezevic, Zlatko"),
                new Book("Understanding .NET Just-In-Time Compilation", "Telerik")
            };
            service = new BookListService(books, logger);
            Console.WriteLine(service.PrintBooks());
        }

        private static void PrintBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine(book.ToString());
        }

        private static void AddBook(Book book)
        {
            service.AddBook(book);
        }

        private static void RemoveBook(Book book)
        {
            if(!service.RemoveBook(book))
                Console.WriteLine("Book not found");
        }
    }
}
