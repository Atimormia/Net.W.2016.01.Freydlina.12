using System;

namespace Task1
{
    //public enum BookAttributes
    //{
    //    Title,
    //    Author,
    //    Publisher,
    //    PublishingYear
    //}

    public class Book: IEquatable<Book>, IComparable<Book>
    {
        public string Title { get; }
        public string[] Authors { get; }
        public string Publisher { get; }
        public int PublishingYear { get; }

        public Book() { }

        public Book(string title, string publisher = "", int publishingYear = 1000, params string[] authors)
        {
            Title = title;
            Publisher = publisher;
            if (publishingYear <= 1000 || publishingYear >= 3000) throw new ArgumentException();
            PublishingYear = publishingYear;
            Authors = authors;
        }

        bool IEquatable<Book>.Equals(Book other)
        {
            return Equals(other);
        }

        public int CompareTo(Book other, Func<Book,int> comparer)
        {
            return comparer(other);
        }

        public int CompareTo(Book other)
        {
            if (other == null) return 1;
            return CompareTo(other, o => string.Compare(Authors[0], o.Authors[0], StringComparison.Ordinal));
        }

        public override string ToString()
        {
            string authorsString = "";
            int i = 1;
            foreach (var author in Authors)
            {
                authorsString += $"{author}";
                if (i < 4)
                {
                    authorsString += ", ";
                    i++;
                }
                else
                {
                    authorsString += "and other";
                    break;
                }
            }
            return $"{authorsString}. {Title} - {Publisher}, {PublishingYear}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Title.Equals(((Book) obj).Title) && 
                Authors.Equals(((Book)obj).Authors) && 
                Publisher.Equals(((Book)obj).Publisher) && 
                PublishingYear.Equals(((Book)obj).PublishingYear);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Title?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (Authors?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (Publisher?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ PublishingYear;
                return hashCode;
            }
        }
    }
}
