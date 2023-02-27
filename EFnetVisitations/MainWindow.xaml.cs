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
        Student selectedStudent;
        int selectedIndex= 0;
        bool changedOn = false;

        public MainWindow()
        {
            InitializeComponent();
            studentList = new List<Student>();
            selectedStudent= new Student();   
            UpDateMainTable();
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
                    UpDateMainTable();
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
                    UpDateMainTable();
                    MessageBox.Show("Успешно добавлен!");
                }
                else MessageBox.Show("Заполните все поля!");
            }
        }
        public async void UpDateMainTable()
        {
            studentList = await _db.Students.ToListAsync();
            MainStudentListDG.ItemsSource = studentList;
        }

        private void MainStudentListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainStudentListDG.SelectedIndex+1!=MainStudentListDG.Items.Count && MainStudentListDG.SelectedIndex!=-1)
            {
                selectedIndex = MainStudentListDG.SelectedIndex;
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
            UpDateMainTable();
            MessageBox.Show("Ученик успешно удален");
        }
    }
}
