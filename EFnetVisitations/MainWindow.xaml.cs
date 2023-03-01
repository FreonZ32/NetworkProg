using EFnetVisitations.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace EFnetVisitations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBContext _db = new DBContext();
        List<Student> studentList;
        List<Visit> visitsList;
        List<Subject> subjectsList;
        Student selectedStudent;
        Subject selectedSubject;
        int StudentListselectedIndex= 0;
        int VisitsListselectedIndex= 0;
        int SubjectListselectedIndex= 0;
        bool changedOn = false;

        public MainWindow()
        {
            InitializeComponent();
            studentList = new List<Student>();
            visitsList = new List<Visit>();
            subjectsList = new List<Subject>();
            selectedStudent= new Student();   
            UpDateStudentsTable();
            UpDateVisitsTable();
            UpDateSubjectsTable();
        }
        public async void UpDateStudentsTable()
        {
            studentList = await _db.Students.ToListAsync();
            MainStudentListDG.ItemsSource = studentList;
        }
        public async void UpDateVisitsTable()
        {
            visitsList = await _db.Visits.Include(visit => visit.Student).Include(visit=>visit.Subject).ToListAsync();
            StudentVisitationsListDG.ItemsSource = visitsList;
        }
        public async void UpDateSubjectsTable()
        {
            subjectsList = await _db.Subjects.ToListAsync();
            StudentSubjectListDG.ItemsSource = subjectsList;
        }

        private async void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            if (changedOn == true)
            {
                if (FirstNameTB.Text != "" && LastNameTB.Text != "" && BirthDayDP.SelectedDate.ToString() != "")
                {
                    var student = _db.Students.Where(c => c.Id == selectedStudent.Id).FirstOrDefault();
                    student.FirstName = FirstNameTB.Text;
                    FirstNameTB.Text = "";
                    student.LastName = LastNameTB.Text;
                    LastNameTB.Text = "";
                    student.Birthday = (DateTime)BirthDayDP.SelectedDate;
                    await _db.SaveChangesAsync();
                    AddBTN.Content = "Добавить";
                    changedOn = false;
                    UpDateStudentsTable();
                    MessageBox.Show("Данные ученика успешно изменены!");
                }
                else MessageBox.Show("Заполните все поля!");
            }
            else
            {
                if (FirstNameTB.Text != "" && LastNameTB.Text != "" && BirthDayDP.SelectedDate.ToString() != "")
                {
                    await _db.Students.AddAsync(new Student() { FirstName = FirstNameTB.Text, LastName = LastNameTB.Text, Birthday = (DateTime)BirthDayDP.SelectedDate });
                    await _db.SaveChangesAsync();
                    UpDateStudentsTable();
                    MessageBox.Show("Успешно добавлен!");
                }
                else MessageBox.Show("Заполните все поля!");
            }
        }
        private void MainStudentListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainStudentListDG.SelectedIndex+1!=MainStudentListDG.Items.Count && MainStudentListDG.SelectedIndex!=-1)
            {
                StudentListselectedIndex = MainStudentListDG.SelectedIndex;
                AddVisitationBTN.IsEnabled= true;
                AddBTN.IsEnabled = false;
                ChangeBTN.IsEnabled = true;
                DeleteBTN.IsEnabled = true;
                selectedStudent = (Student)MainStudentListDG.Items[MainStudentListDG.SelectedIndex];
            }
        }

        private void ChangeBTN_Click(object sender, RoutedEventArgs e)
        {
            if(changedOn==false)
            {
                AddBTN.Content = "Принять \nизменения";
                FirstNameTB.Text = selectedStudent.FirstName;
                LastNameTB.Text = selectedStudent.LastName;
                BirthDayDP.SelectedDate = selectedStudent.Birthday;
                ChangeBTN.IsEnabled = false;
                DeleteBTN.IsEnabled = false;
                AddBTN.IsEnabled = true;
                changedOn = true;
            }
        }

        private async void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {
            var student = _db.Students.Where(c => c.Id == selectedStudent.Id).FirstOrDefault();
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            ChangeBTN.IsEnabled = false;
            DeleteBTN.IsEnabled = false;
            AddBTN.IsEnabled = true;
            UpDateStudentsTable();
            MessageBox.Show("Ученик успешно удален");
        }

        private async void AddVisitationBTN_Click(object sender, RoutedEventArgs e)
        {
            if(VistationDP.SelectedDate.ToString()!="" && selectedStudent!=null && selectedSubject!=null)
            {
                var visit = new Visit()
                {
                    Id = Guid.NewGuid(),
                    Date = (DateTime)VistationDP.SelectedDate,
                    Student = selectedStudent,
                    Subject = selectedSubject
                };
                await _db.Visits.AddAsync(visit);
                await _db.SaveChangesAsync();
                UpDateVisitsTable();
                AddVisitationBTN.IsEnabled = false;
                MessageBox.Show("Дата посещения успешно добавлена!");
            }
            else MessageBox.Show("Заполните поле даты(В таблице посещения)!");
        }
        private async void AddSubjectBTN_Click(object sender, RoutedEventArgs e)
        {
            if(SubjectTB.Text!=""&& SubjectTB.Text!=null)
            {
                var subject = new Subject()
                {
                    Id = Guid.NewGuid(),
                    Name = SubjectTB.Text,
                };
                await _db.Subjects.AddAsync(subject);
                await _db.SaveChangesAsync();
                UpDateSubjectsTable();
                AddSubjectBTN.IsEnabled = false;
                MessageBox.Show("Предмет успешно добавлен!");
            }
            else MessageBox.Show("Заполните поле названия предмета(В таблице предметов)!");
        }
        private void StudentVisitationsListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisitsListselectedIndex = StudentVisitationsListDG.SelectedIndex;
        }

        private void StudentSubjectListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SubjectListselectedIndex= StudentSubjectListDG.SelectedIndex;
            selectedSubject = (Subject)StudentSubjectListDG.Items[StudentSubjectListDG.SelectedIndex];
        }
    }
}
