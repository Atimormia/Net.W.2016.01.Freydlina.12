﻿using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Task1
{
    [Serializable]
    public class Book: IEquatable<Book>, IComparable<Book>, ISerializable
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
            if (publishingYear < 1000 || publishingYear >= 3000) throw new ArgumentException();
            PublishingYear = publishingYear;
            Authors = authors;
        }

        public Book(SerializationInfo info, StreamingContext context)
            :this((string)info.GetValue("title",typeof(string)),
                 (string)info.GetValue("publisher",typeof(string)),
                 (int)info.GetValue("year",typeof(int)),
                 (string[])info.GetValue("authors",typeof(string[]))) { }

        bool IEquatable<Book>.Equals(Book other)
        {
            return Equals(other);
        }

        bool Equals(Book other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if(Authors.Length==other.Authors.Length)
                if (Authors.Where((t, i) => !t.Equals(other.Authors[i])).Any())
                    return false;
                //for (int i = 0; i < Authors.Length; i++)
                //    if (!Authors[i].Equals(other.Authors[i]))
                //        return false;
            return Title.Equals(other.Title) &&
                Publisher.Equals(other.Publisher) &&
                PublishingYear.Equals(other.PublishingYear);
        }

        public int CompareTo(Book other, Func<Book,Book,int> comparer)
        {
            return comparer(this,other);
        }

        public int CompareTo(Book other)
        {
            if (other == null) return 1;
            return CompareTo(other, (b1, b2) =>
            {
                if (b1.Authors.Length == 0)
                    if (b2.Authors.Length == 0) return 0;
                    else return -1;
                if (b2.Authors.Length == 0) return 1;
                return string.Compare(b1.Authors[0], b2.Authors[0], StringComparison.Ordinal);
            });
        }

        public override string ToString()
        {
            string result = "";
            if (Authors.Length != 0)
            {
                int i = 1;
                foreach (var author in Authors)
                {
                    if (author == "") continue;
                    result += $"{author}";
                    if (i == Authors.Length - 1)
                    {
                        result += ".";
                    }
                    else if (i < 4)
                    {
                        result += ", ";
                        i++;
                    }
                    else
                    {
                        result += " and other.";
                        break;
                    }
                }
            }
            result += Title;
            if (Publisher != null) result += " - " + Publisher;
            if (PublishingYear > 1000) result += ", " + PublishingYear;
            return result;
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
            return Equals((Book) obj);
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
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("title", Title, typeof(string));
            info.AddValue("authors", Authors, typeof(string[]));
            info.AddValue("publisher", Publisher, typeof(string));
            info.AddValue("year", PublishingYear, typeof(int));
        }

    }
}
