using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB5
{
    public class StudentBirthYearComparer : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            if (x == null || y == null) { return 0; }
            return x.BirthYear.CompareTo(y.BirthYear);
        }
    }
}
