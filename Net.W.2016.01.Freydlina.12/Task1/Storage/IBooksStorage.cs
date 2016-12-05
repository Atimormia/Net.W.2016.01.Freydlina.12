using System.Collections.Generic;

namespace Task1.Storage
{
    public interface IBooksStorage
    {
        void Save(IEnumerable<Book> books);
        IEnumerable<Book> Open();
    }
}
