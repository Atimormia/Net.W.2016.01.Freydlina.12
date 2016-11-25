using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2;

namespace Task1
{
    public class BookListStorage
    {
        public static void UploadToFile(CustomSet<Book> books, string fileName)
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
        }

        public static CustomSet<Book> DownloadFromFile(string fileName)
        {
            CustomSet<Book> books = new CustomSet<Book>();
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
            return books;
        }
    }
}
