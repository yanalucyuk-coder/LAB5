using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LAB5
{
    public partial class MainWindow : Window
    {
        private readonly StudentCatalog _catalog;
        public MainWindow()
        {
            InitializeComponent();
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(documentsPath, "students.json");
            _catalog = new StudentCatalog(new JsonFileWriter(), new JsonFileReader(), filePath);
            StudentsDataGrid.ItemsSource = _catalog.Students;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(BirthYearTextBox.Text, out int birthYear))
                {
                    MessageBox.Show("Введіть рік народження числом!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newStudent = new Student(
                    FullNameTextBox.Text,
                    birthYear,
                    SchoolTextBox.Text
                );
                _catalog.AddStudent(newStudent);

                FullNameTextBox.Clear();
                BirthYearTextBox.Clear();
                SchoolTextBox.Clear();
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(Object sender, RoutedEventArgs e)
        {
            if(StudentsDataGrid.SelectedItem is Student selectedStudent)
            {
                _catalog.DeleteStudent(selectedStudent);
            }
        }

        private void SortByBirthYearButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.SortByBirthYear();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.FilterBySchool(FilterSchoolTextBox.Text);
        }

        private void SearchButton_Click( object sender, RoutedEventArgs e)
        {
            _catalog.SearchStudent(SearchTextBox.Text);
        }

        private void UniqueSchoolsButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.ShowUniqueSchools();
        }

        private void ThreeOldestButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.FindThreeOldestStudents();
        }

        private void AverageBirthYearButton_Click(object sender, RoutedEventArgs e)
        {
            double average = _catalog.CalculateAverageBirthYear();
            MessageBox.Show($"Середній рік народження = {average}","Розрахунки", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GroupBySchoolButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.GroupStudentsBySchool();
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterSchoolTextBox.Clear();
            _catalog.ResetFilter();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await _catalog.SaveAsync();
            MessageBox.Show("Список успішно збережено.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            await _catalog.LoadAsync();
            MessageBox.Show("Список успішно завантажено.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}