using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB5;

namespace StudentTests
{
    [TestClass]
    public class StudentCatalogTests
    {
        private StudentCatalog catalog;
        private Student student1;
        private Student student2;
        private Student student3;

        [TestInitialize]
        public void TestInitialize()
        {
            student1 = new Student("Коваль Ігор Михайлович", 2003, "Ліцей «Інтелект»");
            student2 = new Student("Шевченко Юлія Михайлівна", 2006, "Русанівський ліцей");
            student3 = new Student("Самійленко Марія Іванівна", 2004, "Ліцей «Інтелект»");

            catalog = new StudentCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");
            catalog.AddStudent(student1);
            catalog.AddStudent(student2);
            catalog.AddStudent(student3);
        }

        [TestMethod]
        public void SearchStudents_WithNull()
        {
            catalog.SearchStudent(null);
            Assert.AreEqual(3, catalog.Students.Count);
        }

        [TestMethod]
        public void SearchStudents_WithEmpty()
        {
            catalog.SearchStudent("   ");
            Assert.AreEqual(3, catalog.Students.Count);
        }

        [TestMethod]
        public void SearchStudents_WithAllName()
        {
            catalog.SearchStudent("Шевченко Юлія Михайлівна");
            Assert.AreEqual(1, catalog.Students.Count);
            Assert.AreEqual(student2, catalog.Students[0]);
        }

        [TestMethod]
        public void SearchStudents_WithNotFullName()
        {
            catalog.SearchStudent("Марія");
            Assert.AreEqual(1, catalog.Students.Count);
            Assert.AreEqual(student3, catalog.Students[0]);
        }

        [TestMethod]
        public void SearchStudents_WithNotFullName2()
        {
            catalog.SearchStudent("Михайл");
            Assert.AreEqual(2, catalog.Students.Count);
            Assert.IsTrue(catalog.Students.Contains(student1));
            Assert.IsTrue(catalog.Students.Contains(student2));
        }

        [TestMethod]
        public void SearchStudents_WithNotFullSchool()
        {
            catalog.SearchStudent("Інтелект");
            Assert.AreEqual(2, catalog.Students.Count);
            Assert.IsTrue(catalog.Students.Contains(student1));
            Assert.IsTrue(catalog.Students.Contains(student3));
        }

        [TestMethod]
        public void SearchStudents_WithNotFullSchool3()
        {
            catalog.SearchStudent("ліцей");
            Assert.AreEqual(3, catalog.Students.Count);
            Assert.IsTrue(catalog.Students.Contains(student1));
            Assert.IsTrue(catalog.Students.Contains(student2));
            Assert.IsTrue(catalog.Students.Contains(student3));
        }

        [TestMethod]
        public void SearchStudents_IgnoresCase()
        {
            catalog.SearchStudent("русанівський");
            Assert.AreEqual(1, catalog.Students.Count);
            Assert.AreEqual(student2, catalog.Students[0]);
        }

        [TestMethod]
        public void SearchStudents_NoMatches()
        {
            catalog.SearchStudent("Олег");
            Assert.AreEqual(0, catalog.Students.Count);
        }

        //тести до ShowUniqueSchools
        [TestMethod]
        public void ShowUniqueSchools_RemoveDuplicates()
        {
            catalog.ShowUniqueSchools();
            Assert.AreEqual(2, catalog.Students.Count);
        }

        [TestMethod]
        public void ShowUniqueSchools_SortedByName()
        {
            catalog.ShowUniqueSchools();
            Assert.AreEqual("Ліцей «Інтелект»", catalog.Students[0].NameOfSchool);
            Assert.AreEqual("Русанівський ліцей", catalog.Students[1].NameOfSchool);
        }

        [TestMethod]
        public void ShowUniqueSchools_DoNotChangeAllStudents()
        {
            catalog.ShowUniqueSchools();
            catalog.ResetFilter();
            Assert.AreEqual(3, catalog.Students.Count);
        }

        [TestMethod]
        public void ShowUniqueSchools_EmptyCatalog()
        {
            var emptyCatalog = new StudentCatalog(new JsonFileWriter(), new JsonFileReader(), "empty.json");
            emptyCatalog.ShowUniqueSchools();
            Assert.AreEqual(0, emptyCatalog.Students.Count);
        }

        //тести до FindThreeOldestStudents
        [TestMethod]
        public void FindThreeOldestStudents_Return3_Oldest()
        {
            var student4 = new Student("Джура Гліб Леонідович", 2007, "Ліцей №9");
            var student5 = new Student("Білінська Зоя Олегівна", 2001, "Гіназія №48");
            catalog.AddStudent(student4);
            catalog.AddStudent(student5);

            catalog.FindThreeOldestStudents();
            Assert.AreEqual(3, catalog.Students.Count);
            Assert.AreEqual(student5, catalog.Students[0]);
            Assert.AreEqual(student1, catalog.Students[1]);
            Assert.AreEqual(student3, catalog.Students[2]);
        }

        [TestMethod]
        public void FindThreeOldestStudents_ExcludesYoungest()
        {
            var student4 = new Student("Джура Гліб Леонідович", 2007, "Ліцей №9");
            var student5 = new Student("Білінська Зоя Олегівна", 2001, "Гіназія №48");
            catalog.AddStudent(student4);
            catalog.AddStudent(student5);

            catalog.FindThreeOldestStudents();
            Assert.IsFalse(catalog.Students.Contains(student2));
            Assert.IsFalse(catalog.Students.Contains(student4));
        }

        [TestMethod]
        public void FindThreeOldestStudents_LessThanThree()
        {
            var smallCatalog = new StudentCatalog(new JsonFileWriter(), new JsonFileReader(), "small.json");
            smallCatalog.AddStudent(student1);
            smallCatalog.AddStudent(student2);

            smallCatalog.FindThreeOldestStudents();
            Assert.AreEqual(2, smallCatalog.Students.Count);
        }

        [TestMethod]
        public void FindThreeOldestStudents_EmptyCatalog()
        {
            var emptyCatalog = new StudentCatalog(new JsonFileWriter(), new JsonFileReader(), "empty.json");
            emptyCatalog.FindThreeOldestStudents();
            Assert.AreEqual(0, emptyCatalog.Students.Count);
        }

        [TestMethod]
        public void FindThreeOldestStudents_DoNotChangeAllStudents()
        {
            catalog.FindThreeOldestStudents();
            catalog.ResetFilter();
            Assert.AreEqual(3, catalog.Students.Count);
        }

        //тести CalculateAverageBirthYear
        [TestMethod]
        public void CalculateAverageBirthYear_IntResult()
        {
            catalog.AddStudent(new Student("Джура Гліб Леонідович", 2003, "Ліцей №9"));
            double result = catalog.CalculateAverageBirthYear();
            Assert.AreEqual(2004, result);
        }

        [TestMethod]
        public void CalculateAverageBirthYear_FractionalResult()
        {
            double result = catalog.CalculateAverageBirthYear();
            Assert.AreEqual(2004.33, result, 0.01);
        }

        [TestMethod]
        public void CalculateAverageBirthYear_EmptyCatalog()
        {
            var emptyCatalog = new StudentCatalog(new JsonFileWriter(), new JsonFileReader(), "empty.json");
            double result = emptyCatalog.CalculateAverageBirthYear();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateAverageBirthYear_IgnoresFilters()
        {
            catalog.FilterBySchool("Ліцей «Інтелект»");
            double result = catalog.CalculateAverageBirthYear();
            Assert.AreEqual(2004.33, result, 0.01);
        }

        //тести GroupStudentsBySchool
        [TestMethod]
        public void GroupStudentsBySchool_KeepAllStudents()
        {
            catalog.GroupStudentsBySchool();
            Assert.AreEqual(3,catalog.Students.Count);
        }

        [TestMethod]
        public void GroupStudentsBySchool_GroupSchoolsTogether()
        {
            catalog.GroupStudentsBySchool();
            Assert.AreEqual(student1, catalog.Students[0]);
            Assert.AreEqual(student3, catalog.Students[1]);
            Assert.AreEqual(student2, catalog.Students[2]);
        }

        [TestMethod]
        public void GroupStudentsBySchool_EmptyCatalog()
        {
            var emptyCatalog = new StudentCatalog(new JsonFileWriter(), new JsonFileReader(), "empty.json");
            catalog.GroupStudentsBySchool();
            Assert.AreEqual(0, emptyCatalog.Students.Count);
        }

        [TestMethod]
        public void GroupStudentsBySchool_ResetOriginalOrder()
        {
            catalog.GroupStudentsBySchool();
            catalog.ResetFilter();
            Assert.AreEqual(student1, catalog.Students[0]);
            Assert.AreEqual(student2, catalog.Students[1]);
            Assert.AreEqual(student3, catalog.Students[2]);
        }
    }
}
