using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB5
{
    public class Student : IComparable<Student>
    {
        private string _fullName;
        private int _birthYear;
        private string _nameOfSchool;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Повне ім'я не може бути пустим!", nameof(FullName));
                }
                _fullName = value;
            }
        }

        public int BirthYear
        {
            get { return _birthYear; }
            set
            {
                if (value < 1909)
                {
                    throw new ArgumentException("Рік народження не може бути менше 1909 року!", nameof(BirthYear));
                }
                if (value > DateTime.Now.Year)
                {
                    throw new ArgumentException("Рік народження не може бути більше 2026 року!", nameof(BirthYear));
                }
                _birthYear = value;
            }
        }

        public string NameOfSchool
        {
            get { return _nameOfSchool; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ім'я школи не може бути пустим!", nameof(NameOfSchool));
                }
                _nameOfSchool = value;
            }
        }

        public Student(string fullName, int birthYear, string nameOfSchool)
        {
            FullName = fullName;
            BirthYear = birthYear;
            NameOfSchool = nameOfSchool;
        }

        public int CompareTo(Student other)
        {
            if(other == null)
            {
                return 1;
            }
            return string.Compare(this.FullName, other.FullName, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"ПІБ: {FullName}, Рік народження: {BirthYear}, Школа: {NameOfSchool}";
        }
    }
}
