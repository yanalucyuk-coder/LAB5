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
            var sortedStudents = new List<Student>(Students);
            sortedStudents.Sort(new StudentBirthYearComparer());
            UpdateVisibleCollection(sortedStudents);
        }

        public void FilterBySchool(string school)
        {
            if(string.IsNullOrWhiteSpace(school))
            {
                ResetFilter(); return;
            }

            var filtered = new List<Student>();
            for(int i=0;i<_allStudents.Count;i++)
            {
                if (string.Equals(_allStudents[i].NameOfSchool,school.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    filtered.Add(_allStudents[i]);
                }
            }
            UpdateVisibleCollection(filtered);
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
