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
        Student selectedStudent;
        int StudentListselectedIndex= 0;
        int VisitsListselectedIndex= 0;
        bool changedOn = false;

        public MainWindow()
        {
            InitializeComponent();
            studentList = new List<Student>();
            visitsList = new List<Visit>();
            selectedStudent= new Student();   
            UpDateStudentsTable();
            UpDateVisitsTable();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
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
                    AddBTN.Content = "Изменить";
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
        public async void UpDateStudentsTable()
        {
            studentList = await _db.Students.ToListAsync();
            MainStudentListDG.ItemsSource = studentList;
        }
        public async void UpDateVisitsTable()
        {
            visitsList = await _db.Visits.Include(visit => visit.Student).ToListAsync();
            StudentVisitationsListDG.ItemsSource = visitsList;
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
            if(VistationDP.SelectedDate.ToString()!="")
            {
                var visit = new Visit()
                {
                    Id = Guid.NewGuid(),
                    Date = (DateTime)VistationDP.SelectedDate,
                    Student = selectedStudent
                };
                await _db.Visits.AddAsync(visit);
                await _db.SaveChangesAsync();
                UpDateVisitsTable();
                AddVisitationBTN.IsEnabled = false;
                MessageBox.Show("Дата посещения успешно добавлена!");
            }
            else MessageBox.Show("Заполните поле даты(В таблице посещения)!");
        }

        private void StudentVisitationsListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisitsListselectedIndex = StudentVisitationsListDG.SelectedIndex;
        }
    }
}
