using System;
using System.Collections.Generic;
using System.Linq;
using Task2;

namespace Task1
{
    class BookListService
    {
        public CustomSet<Book> Books { get; private set; }

        public BookListService()
        {
            Books = new CustomSet<Book>();
        }

        public BookListService(IEnumerable<Book> books)
        {
            if (books == null) throw new ArgumentNullException();
            foreach (var book in books)
                Books.Add(book);
        }

        public void AddBook(Book newBook)
        {
            if (newBook == null) throw new ArgumentNullException();
            Books.Add(newBook);
        }

        public void RemoveBook(Book book)
        {
            if (book == null) throw new ArgumentNullException();
            Books.Remove(book);
        }

        public Book FindBookByTag(Predicate<Book> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            return Books.FirstOrDefault(book => tag(book));
        }

        public void SortBooksByTag(Func<Book,Book> tag)
        {
            if (tag == null) throw new ArgumentNullException();
            Books.Collection.SortBy(tag);
        }
    }
}
