using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Task1.Logger;

namespace Task1.Storage
{
    public class BooksXmlStorage: IBooksStorage
    {
        private readonly ILogger logger;
        private readonly string fileName;

        public BooksXmlStorage(ILogger logger, string fileName)
        {
            if (logger == null) logger = new Logger.Logger();
            if (fileName == null) throw new ArgumentNullException();
            this.logger = logger;
            this.fileName = fileName;
        }

        public void Save(IEnumerable<Book> books)
        {
            try
            {
                XDocument xDoc = new XDocument();
                XElement xRoot = new XElement("books");
                foreach (var book in books)
                {
                    XElement bookElement = new XElement("book");

                    XElement titleElement = new XElement("title", book.Title);
                    bookElement.Add(titleElement);

                    XElement authorsElement = new XElement("authors");
                    foreach (var author in book.Authors)
                    {
                        XElement authorElement = new XElement("author", author);
                        authorsElement.Add(authorElement);
                    }
                    bookElement.Add(authorsElement);

                    XElement publisherElement = new XElement("publisher", book.Publisher);
                    bookElement.Add(publisherElement);

                    XElement yearElement = new XElement("year", book.PublishingYear.ToString());
                    bookElement.Add(yearElement);

                    xRoot.Add(bookElement);
                }
                xDoc.Add(xRoot);
                xDoc.Save(fileName);
                logger.Debug("Uploading to file: " + fileName);
            }
            catch (Exception)
            {
                logger.Error("Exception during uploading to file");
            }
        }

        public IEnumerable<Book> Open()
        {
            List<Book> books = new List<Book>();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fileName);
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlElement xnode in xRoot)
                {
                    string title = "";
                    string authors = "";
                    string publisher = "";
                    int year = 0;
                    foreach (XmlNode childnodeBook in xnode.ChildNodes)
                    {
                        if (childnodeBook.Name == "title")
                            title = childnodeBook.InnerText;
                        foreach (XmlNode childnodeAuthor in childnodeBook.ChildNodes)
                            if (childnodeAuthor.Name == "author")
                                authors += childnodeAuthor.InnerText + "|";
                        if (childnodeBook.Name == "publisher")
                            publisher = childnodeBook.InnerText;
                        if (childnodeBook.Name == "year")
                            year = Int32.Parse(childnodeBook.InnerText);
                    }
                    Book book = new Book(title, publisher, year, authors.Split('|'));
                    books.Add(book);
                }
            }
            catch (Exception)
            {
                logger.Error("Exception during downloading from file");
            }
            return books;
        }
    }
}
