using Microsoft.VisualStudio.TestTools.UnitTesting; 
using System;
using LAB5;

namespace StudentTests
{
    [TestClass]
    public class StudentTest
    {
        [TestMethod]
        public void Constructor_WithValidData()
        {
            string fullName = "Коваль Ігор Михайлович";
            int birthYear = 2006;
            string nameOfSchool = "Природничо-науковий ліцей № 145";

            var student = new Student(fullName, birthYear, nameOfSchool);
            Assert.AreEqual(fullName, student.FullName);
            Assert.AreEqual(birthYear, student.BirthYear);
            Assert.AreEqual(nameOfSchool, student.NameOfSchool);
        }

        [TestMethod]
        [DataRow(null)] // DataRow запускає один і той самий тест з різними значеннями
        [DataRow("")]
        [DataRow(" ")]
        public void Constructor_WithInvalisFullName(string invalidFullName)
        {
            Assert.ThrowsException<ArgumentException>(() => new Student(invalidFullName, 2006, "Природничо-науковий ліцей № 145"));
        }

        [TestMethod]
        [DataRow(null)] // DataRow запускає один і той самий тест з різними значеннями
        [DataRow("")]
        [DataRow(" ")]
        public void Constructor_WithInvalidNameOfSchool(string invalidNameOfSchool)
        {
            Assert.ThrowsException<ArgumentException>(() => new Student("Коваль Ігор Михайлович", 2006, invalidNameOfSchool));
        }

        [TestMethod]
        public void Constructor_WithFutureYear()
        {
            Assert.ThrowsException<ArgumentException>(() => new Student("Коваль Ігор Михайлович", 2027, "Природничо-науковий ліцей № 145"));
        }

        [TestMethod]
        public void Constructor_WithEarlyYear()
        {
            Assert.ThrowsException<ArgumentException>(() => new Student("Коваль Ігор Михайлович", 1900, "Природничо-науковий ліцей № 145"));
        }

        [TestMethod]
        public void CompareTo_WithOtherStudent()
        {
            var student1 = new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145");
            var student2 = new Student("Шевченко Юлія Михайлівна", 2003, "Русанівський ліцей");
            var student3 = new Student("Коваль Ігор Михайлович", 2005, "Ліцей «Інтелект»");

            Assert.IsTrue(student1.CompareTo(student2)<0, "Коваль має бути перед Шевченко!");
            Assert.IsTrue(student2.CompareTo(student1) >0, "Шевченко має бути після Коваль!");
            Assert.AreEqual(0, student1.CompareTo(student3), "Однакові ПІБ мають давати 0!");
        }

        [TestMethod]
        public void CompareTo_WithNull()
        {
            var student = new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145");
            Assert.IsTrue(student.CompareTo(null)>0, "Порівння з null має повертаити додатне значення!");
        }

        [TestMethod]
        public void ToString_ReturnsCorrectString()
        {
            var student = new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145");
            string expectedString = "ПІБ: Коваль Ігор Михайлович, Рік народження: 2006, Школа: Природничо-науковий ліцей № 145";
            Assert.AreEqual (expectedString, student.ToString());
        }
    }
}