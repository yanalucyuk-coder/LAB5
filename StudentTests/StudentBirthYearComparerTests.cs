using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB5;

namespace StudentTests
{
    [TestClass]
    public class StudentBirthYearComparerTests
    {
        [TestMethod]
        public void Compare_WithDifferentBirthYears()
        {
            var comparer = new StudentBirthYearComparer();
            var older = new Student("Коваль Ігор Михайлович", 2003, "Природничо-науковий ліцей № 145");
            var younger = new Student("Шевченко Юлія Михайлівна", 2006, "Русанівський ліцей");

            Assert.IsTrue(comparer.Compare(older, younger) < 0, "Старший (2003) має бути перед молодшим (2006)!");
            Assert.IsTrue(comparer.Compare(younger,older) > 0, "Молодший (2006) має бути після старшого (2003)!");
        }

        [TestMethod]
        public void Compare_WithSameBirthYears()
        {
            var comparer = new StudentBirthYearComparer();
            var student1= new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145");
            var student2 = new Student("Шевченко Юлія Михайлівна", 2006, "Русанівський ліцей");

            Assert.AreEqual(0,comparer.Compare(student1, student2), "Однакові роки мають давати 0.");
        }

        [TestMethod]
        public void Compare_WithNull()
        {
            var comparer = new StudentBirthYearComparer();
            var student = new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145");

            Assert.AreEqual(0, comparer.Compare(student, null));
            Assert.AreEqual(0, comparer.Compare(null, student));
            Assert.AreEqual(0, comparer.Compare(null, null));
        }

        [TestMethod]
        public void Sort_WithComparerByBirthYear()
        {
            var students = new List<Student>
            {
                new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145"),
                new Student("Шевченко Юлія Михайлівна", 2003, "Русанівський ліцей"),
                new Student("Самійленко Марія Іванівна", 2004, "Ліцей «Інтелект»")
            };

            students.Sort(new StudentBirthYearComparer());
            Assert.AreEqual(2003, students[0].BirthYear);
            Assert.AreEqual(2004, students[1].BirthYear);
            Assert.AreEqual(2006, students[2].BirthYear);
        }
    }
}
