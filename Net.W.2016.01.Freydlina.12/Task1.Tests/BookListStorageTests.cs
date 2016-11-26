using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Task2;

namespace Task1.Tests
{
    [TestFixture]
    public class BookListStorageTests
    {
        public static IEnumerable<TestCaseData> TestCasesForUploadDownload
        {
            get
            {
                CustomSet<Book> books = new CustomSet<Book>(3)
                {
                    new Book("Announcing.NET Framework 4.6.2", "Microsoft", 2016, "Haffner, Stacey"),
                    new Book(".NET Core - .NET Goes Cross - Platform with.NET Core", "Microsoft", 2016,
                        "Carter, Phillip", "Knezevic, Zlatko"),
                    new Book("Understanding .NET Just-In-Time Compilation", "Telerik")
                };
                yield return new TestCaseData(books).Returns(true);
            }
        }

        [Test, TestCaseSource(nameof(TestCasesForUploadDownload))]
        public bool TestUploadToFileDownload(CustomSet<Book> booksUp)
        {
            BookListStorage storage = new BookListStorage(new Task1.Logger.Logger(), @"C:\Users\Polina\Documents\git\Net.W.2016.01.Freydlina.12\Net.W.2016.01.Freydlina.12\Net.W.2016.01.Freydlina.12\test");
            storage.UploadToFile(booksUp);
            CustomSet<Book> booksDown = storage.DownloadFromFile();
            return booksDown.Equals(booksUp);
        }

        
    }
}
