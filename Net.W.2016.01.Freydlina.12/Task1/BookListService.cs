using System;
using System.Collections.Generic;
using System.Linq;
using Task1.Logger;
using Task2;

namespace Task1
{
    public class BookListService
    {
        public CustomSet<Book> Books { get; }
        private ILogger logger;

        public BookListService(ILogger logger)
        {
            if(logger==null) throw new ArgumentNullException(nameof(logger));
            Books = new CustomSet<Book>();
            this.logger = logger;
        }

        public BookListService(IEnumerable<Book> books,ILogger logger): this(logger)
        {
            if (books == null) throw new ArgumentNullException();
            foreach (var book in books)
                Books.Add(book);
        }

        public void AddBook(Book newBook)
        {
            if (newBook == null) throw new ArgumentNullException();
            Books.Add(newBook);
            logger.Debug(newBook + " added");
        }

        public bool RemoveBook(Book book)
        {
            if (book == null) throw new ArgumentNullException();
            if (!Books.Remove(book)) return false;
            logger.Debug(book + " removed");
            return true;
        }

        public Book FindBookByTag(Predicate<Book> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            return Books.FirstOrDefault(book => tag(book));
        }

        public IEnumerable<Book> FindBooksByTag(Predicate<Book> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            return from Book book in Books where tag(book) select book;
        }

        public void SortBooksByTag(Func<Book,Book> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            Books.Collection.SortBy(tag);
        }
    }
}
