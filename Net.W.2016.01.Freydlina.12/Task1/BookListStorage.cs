using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2;

namespace Task1
{
    class BookListStorage
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

        public static CustomSet<Book> DownloadFromFile(string file)
        {
            throw new NotImplementedException();
        }
    }
}
