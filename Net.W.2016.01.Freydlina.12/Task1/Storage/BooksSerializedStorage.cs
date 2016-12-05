using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Task1.Logger;

namespace Task1.Storage
{
    public class BooksSerializedStorage: IBooksStorage
    {
        private readonly ILogger logger;
        private readonly string fileName;

        public BooksSerializedStorage(ILogger logger, string fileName)
        {
            if (logger == null) logger = new Logger.Logger();
            if (fileName == null) throw new ArgumentNullException();
            this.logger = logger;
            this.fileName = fileName;
        }

        public void Save(IEnumerable<Book> books)
        {
            List<Book> booksList = new List<Book>(books);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, booksList);
                logger.Debug("Uploading to file: " + fileName);
            }
            catch (SerializationException e)
            {
                logger.Debug("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        public IEnumerable<Book> Open()
        {
            List<Book> books = null;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                books = (List<Book>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                logger.Debug("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return books;
        }
    }
}
