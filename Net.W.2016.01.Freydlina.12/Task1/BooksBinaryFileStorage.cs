using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Task1.Logger;
using Task2;

namespace Task1
{
    public class BooksBinaryFileStorage: IBooksStorage
    {
        private readonly ILogger logger;
        private readonly string fileName;

        public BooksBinaryFileStorage(ILogger logger,string fileName)
        {
            if (logger == null || fileName == null) throw new ArgumentNullException();
            this.logger = logger;
            this.fileName = fileName;
        }

        public void UploadTo(IEnumerable<Book> books)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate)))
                {
                    foreach (var book in books)
                    {
                        writer.Write(book.Title);
                        foreach (var author in book.Authors)
                            writer.Write(author);
                        writer.Write("-");
                        writer.Write(book.Publisher);
                        writer.Write(book.PublishingYear);
                    }
                }
                logger.Debug("Uploading to file: " + fileName);
            }
            catch (Exception)
            {
                logger.Error("Exception during uploading to file");
            }


        }

        public IEnumerable<Book> DownloadFrom()
        {
            CustomSet<Book> books = new CustomSet<Book>();
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        string title = reader.ReadString();
                        string authors = "";
                        while (true)
                        {
                            string author = reader.ReadString();
                            if (author == "-") break;
                            authors += author + "|";
                        }
                        string publisher = reader.ReadString();
                        int publishingYear = reader.ReadInt32();
                        Book book = new Book(title, publisher, publishingYear, authors.Split('|'));
                        books.Add(book);
                    }
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
