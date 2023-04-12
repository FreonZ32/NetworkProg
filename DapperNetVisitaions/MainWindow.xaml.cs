using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DapperNetVisitaions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StudentServise _dbStudSer;
        IEnumerable<Student> students;
        Student selectedStudent;
        bool changeOn = false;
        public MainWindow()
        {
            InitializeComponent();
            _dbStudSer = new StudentServise();
            selectedStudent= new Student();
            students = new List<Student>();
            UpDateAllSudentsList();
        }
        public async void UpDateAllSudentsList()
        {
            students = await _dbStudSer.GetStudents();
            StudentsDG.ItemsSource = students;
        }

        private void AddNewStudentBTN_Click(object sender, RoutedEventArgs e)
        {
            if (FirstNameTB.Text != "" && LastNameTB.Text != "" && BirthdayDP.SelectedDate != null)
            {
                if (changeOn)
                {
                    selectedStudent= new Student(selectedStudent.Id.ToString(), FirstNameTB.Text, LastNameTB.Text, BirthdayDP.SelectedDate.Value);
                    _dbStudSer.Update(selectedStudent);
                    MessageBox.Show("Данные изменены");
                    ClearStudentInfoField();
                    UpDateAllSudentsList();
                    AddNewStudentBTN.Content = "Принять изменения";
                    changeOn = false;

                }
                else
                {
                    Student student = new Student(Guid.NewGuid().ToString(), FirstNameTB.Text, LastNameTB.Text, BirthdayDP.SelectedDate.Value);
                    _dbStudSer.AddStudent(student);
                    MessageBox.Show("Ученик успешно добавлен!");
                    ClearStudentInfoField();
                    UpDateAllSudentsList();
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }
        private void ClearStudentInfoField()
        {
            FirstNameTB.Text = "";
            LastNameTB.Text = "";
            BirthdayDP.SelectedDate = null;
        }

        private void StudentsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StudentsDG.SelectedIndex != -1) 
            {
                selectedStudent = (Student)StudentsDG.SelectedItem;
                EditStudentNameBTN.IsEnabled= true;
                DeleteStudentNameBTN.IsEnabled= true;
                AddNewStudentBTN.IsEnabled= false;
            }
        }

        private void EditStudentNameBTN_Click(object sender, RoutedEventArgs e)
        {
            if(selectedStudent!= null) 
            {
                FirstNameTB.Text = selectedStudent.FirstName;
                LastNameTB.Text = selectedStudent.LastName;
                BirthdayDP.SelectedDate = selectedStudent.Birthday;
                AddNewStudentBTN.Content = "Принять изменения";
                EditStudentNameBTN.IsEnabled = false;
                DeleteStudentNameBTN.IsEnabled = false;
                AddNewStudentBTN.IsEnabled = true;
                changeOn = true;
            }
        }

        private async void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchTB.Text!= "" && SearchTB.Text!="Поиск...")
            {
                students = await _dbStudSer.GetStudentByFirstLastName(SearchTB.Text);
                StudentsDG.ItemsSource = students;
            }
        }

        private void SearchTB_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = "";
        }

        private void AddFakeBTN_Click(object sender, RoutedEventArgs e)
        {
            Student student = new Student(Guid.NewGuid().ToString(), Faker.Name.First(), Faker.Name.Last(), DateTime.Now);
            _dbStudSer.AddStudent(student);
            UpDateAllSudentsList();
        }

        private void SearchCleanBTN_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = "Поиск...";
            UpDateAllSudentsList();
        }
    }
}
