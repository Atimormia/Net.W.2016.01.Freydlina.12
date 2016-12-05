using System;
using System.Collections.Generic;
using System.Linq;
using Task1.Logger;
using Task1.Storage;
using Task2;

namespace Task1
{
    public class BookListService: IEquatable<BookListService>
    {
        private CustomSet<Book> books;
        private ILogger logger;

        public BookListService(ILogger logger)
        {
            if(logger==null) throw new ArgumentNullException(nameof(logger));
            books = new CustomSet<Book>();
            this.logger = logger;
        }

        public BookListService(IEnumerable<Book> books,ILogger logger): this(logger)
        {
            if (books == null) throw new ArgumentNullException();
            foreach (var book in books)
                this.books.Add(book);
        }

        public void AddBook(Book newBook)
        {
            if (newBook == null) throw new ArgumentNullException();
            books.Add(newBook);
            logger.Debug(newBook + " added");
        }

        public bool RemoveBook(Book book)
        {
            if (book == null) throw new ArgumentNullException();
            if (!books.Remove(book)) return false;
            logger.Debug(book + " removed");
            return true;
        }

        public Book FindBookByTag(Predicate<Book> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            return books.FirstOrDefault(book => tag(book));
        }

        public IEnumerable<Book> FindBooksByTag(Predicate<Book> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            return from Book book in books where tag(book) select book;
        }

        public void SortBooksByTag<TKey>(Func<Book,TKey> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            books.Collection.SortBy(tag);
        }

        public void SaveTo(IBooksStorage storage) => storage.Save(books);

        public void OpenFrom(IBooksStorage storage) => books = new CustomSet<Book>(storage.Open());

        public bool Equals(BookListService other)
        {
            if (other == null) return false;
            IEquatable<CustomSet<Book>> equatableBooks = other.books;
            return equatableBooks.Equals(books);
        }

        public string[] PrintBooks()
        {
            string[] booksStrings = new string[books.Count];
            int i = 0;
            foreach (var book in books)
            {
                booksStrings[i] = book.ToString();
                i++;
            }
            return booksStrings;
        }
    }
}
