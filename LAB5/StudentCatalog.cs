using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LAB5
{
    public class StudentCatalog
    {
        private readonly IFileSaver _fileSaver;
        private readonly IFileLoader _fileLoader;
        private readonly string _filePath;

        //публічна колекція для зберігання всіх даних
        public ObservableCollection<Student> Students { get;}

        private List<Student> _allStudents;
        public StudentCatalog(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            _fileSaver = fileSaver;
            _fileLoader = fileLoader;
            _filePath = filePath;
            Students = new ObservableCollection<Student>();
            _allStudents = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
            _allStudents.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            Students.Remove(student);
            _allStudents.Remove(student);
        }

        public void SortByBirthYear()
        {
            //var sortedStudents = new List<Student>(Students);
            //sortedStudents.Sort(new StudentBirthYearComparer());
            //UpdateVisibleCollection(sortedStudents);

            var sortedStudents = Students
            .OrderBy(s => s.BirthYear)
            .ToList();
            UpdateVisibleCollection(sortedStudents);
        }

        public void FilterBySchool(string school)
        {
            if(string.IsNullOrWhiteSpace(school))
            {
                ResetFilter(); return;
            }

            /*var filtered = new List<Student>();
            for(int i=0;i<_allStudents.Count;i++)
            {
                if (string.Equals(_allStudents[i].NameOfSchool,school.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    filtered.Add(_allStudents[i]);
                }
            }*/
            string trimmedSchool = school.Trim();
            var filtered = _allStudents
                .Where(s => string.Equals(s.NameOfSchool, trimmedSchool, StringComparison.OrdinalIgnoreCase))
                .ToList();
            UpdateVisibleCollection(filtered);
        }

        public void SearchStudent (string searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
            {
                ResetFilter(); return;
            }

            string lowerTerm = searchTerm.Trim().ToLowerInvariant();
            var searchResult = _allStudents
                .Where(s => s.FullName.ToLowerInvariant().Contains(lowerTerm) || s.NameOfSchool.ToLowerInvariant().Contains(lowerTerm))
                .ToList();
            UpdateVisibleCollection(searchResult);
        }

        //проєкція Select
        public void ShowUniqueSchools()
        {
            var uniqueSchools = _allStudents
                .Select(s => s.NameOfSchool) //проєкція (список рядків)
                .Distinct()
                .OrderBy(n => n) //лямбда n => n сортуй самі рядки
                .ToList();
            var schoolStudents = uniqueSchools
                .Select( n => new Student("Школа", 2000, n))
                .ToList();
            UpdateVisibleCollection(schoolStudents);
        }

        public void FindThreeOldestStudents()
        {
            var oldestStudents = _allStudents
                .OrderBy(s => s.BirthYear)
                .Take(3)
                .ToList();
            UpdateVisibleCollection(oldestStudents);
        }

        //агрегація Average
        public double CalculateAverageBirthYear()
        {
            if(_allStudents.Count == 0)
            {
                return 0;
            }
            return _allStudents.Average(s => s.BirthYear);
        }

        //групування GroupBy
        public void GroupStudentsBySchool()
        {
            var groupedStudents = _allStudents
                .GroupBy(s => s.NameOfSchool)
                .OrderBy(g => g.Key) //сортування груп за алфавітом
                .SelectMany(g => g) //розгортання груп в список студентів
                .ToList();
            UpdateVisibleCollection(groupedStudents);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allStudents);
        }

        private void UpdateVisibleCollection(List<Student> newList)
        {
            Students.Clear();
            foreach(var student in newList)
            {
                Students.Add(student);
            }
        }

        public async Task SaveAsync()
        {
            await _fileSaver.SaveAsync(_filePath, _allStudents);
        }

        public async Task LoadAsync()
        {
            var loadedStudents = await _fileLoader.LoadAsync<Student>(_filePath);
            if (loadedStudents != null)
            {
                _allStudents = loadedStudents;
                UpdateVisibleCollection(_allStudents);
            }
        }

    }
}
