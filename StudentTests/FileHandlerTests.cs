using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading.Tasks;
using LAB5;

namespace StudentTests
{
    [TestClass]
    public class FileHandlerTests
    {
        private string _testFilePath;

        [TestInitialize]
        public void TestInitialize()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
        }
        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        //запис створює файл з даними
        [TestMethod]
        public async Task SaveAsync_Should_WriteDataToFile()
        {
            var students = new List<Student>
            {
                new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145"),
                new Student("Шевченко Юлія Михайлівна", 2003, "Русанівський ліцей")
            };

            var fileSaver = new JsonFileWriter();
            await fileSaver.SaveAsync(_testFilePath, students);
            Assert.IsTrue(File.Exists(_testFilePath), "Файл не було створено!");

            string fileContent = await File.ReadAllTextAsync(_testFilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(fileContent), "Файл порожній!");

            var fileReader = new JsonFileReader();
            var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);
            Assert.IsNotNull(loadedStudents, "Десеріалізація не вдалася!");
            Assert.AreEqual(2, loadedStudents.Count, "Кількість десеріалізованих об'єктів не збігається!");
        }

        //читання з готового JSON-файлу
        [TestMethod]
        public async Task LoadAsync_Should_ReadDataFromFile()
        {
            var jsonContent = "[{\"FullName\":\"Коваль Ігор Михайлович\",\"BirthYear\":2006,\"NameOfSchool\":\"Природничо-науковий ліцей № 145\"}]";
            await File.WriteAllTextAsync(_testFilePath, jsonContent);

            var fileReader = new JsonFileReader();
            var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);

            Assert.IsNotNull(loadedStudents, "Об'єкти не були завантажені!");
            Assert.AreEqual(1, loadedStudents.Count, "Некоректна кількість об'єктів!");
            Assert.AreEqual("Коваль Ігор Михайлович", loadedStudents[0].FullName);
            Assert.AreEqual(2006, loadedStudents[0].BirthYear);
            Assert.AreEqual("Природничо-науковий ліцей № 145", loadedStudents[0].NameOfSchool);
        }

        //немає файлу - 0
        [TestMethod]
        public async Task LoadAsync_WhenFileDoesNotExist_ReturnsNull()
        {
            var fileReader = new JsonFileReader();
            var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);

            Assert.IsNull(loadedStudents, "Повинен повертати null, якщо файл не існує!");

        }

        //при записі та читання однакові дані
        [TestMethod]
        public async Task SaveAndLoad_RoundTrip_PreservesData()
        {
            var students = new List<Student>
            {
                new Student("Коваль Ігор Михайлович", 2006, "Природничо-науковий ліцей № 145")
            };

            await new JsonFileWriter().SaveAsync(_testFilePath, students);
            var loadedStudents = await new JsonFileReader().LoadAsync<Student>(_testFilePath);

            Assert.IsNotNull(loadedStudents);
            Assert.AreEqual(students[0].FullName, loadedStudents[0].FullName);
            Assert.AreEqual(students[0].BirthYear, loadedStudents[0].BirthYear);
            Assert.AreEqual(students[0].NameOfSchool, loadedStudents[0].NameOfSchool);
        }
    }
}