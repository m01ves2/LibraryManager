using LibraryManager.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace LibraryManager.Domain.ValueObjects
{
    public sealed class Isbn
    {
        public string Value { get; }

        private Isbn(string value)
        {
            this.Value = value;
        }

        public override bool Equals(object? obj) //canonic code for VO
        {
            if(ReferenceEquals(this, obj))
                return true;

            if (obj is Isbn otherIsbn) { 
                return this.Value == otherIsbn.Value;
            }
            return false;
        }

        public static bool operator==(Isbn left, Isbn right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            return ReferenceEquals(left, right) || left.Equals(right);
        }

        public static bool operator!=(Isbn left, Isbn right)
        { 
            return !(left==right); 
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static Isbn Parse(string value)
        {
            if (IsValid(value)) {
                return new Isbn(value);
            }
            else {
                throw new ArgumentException("InvalidBookException: ISBN not valid");
            }
        }

        public static bool IsValid(string value)
        {
            if(string.IsNullOrEmpty(value))
                return false;

            string normalized = value.Replace("-", "");
            return (normalized.Length == 10 || normalized.Length == 13) && normalized.All(char.IsDigit);

            //TODO
            //10 цифр, разделенных на 4 части:
            //код страны/ группы стран,
            //код издательства,
            //уникальный номер издания и
            //контрольная цифра, служащая для проверки корректности номера.
            //Он однозначно идентифицирует книгу, при этом длина каждой части переменная, чтобы вместить разные объемы книгоиздания по странам.
            //80-902734-1-6

            //13 цифр, разделенных на 5 групп:
            //префикс (978/979),
            //код страны/языка (например, 5 для России),
            //код издательства,
            //номер издания (книги) и
            //контрольная цифра, которая проверяет правильность номера,
            //обеспечивая уникальность и возможность идентификации каждой книги, издательства и страны выпуска.
            //978-5-699-12014-7


            //string pattern13 = @"(978|979)-(\d)-(\d){3}-(\d){5}-(\d)";
            //return Regex.IsMatch(value, pattern13);
        }
    }
}
