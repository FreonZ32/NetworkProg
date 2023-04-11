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
using System.Xml.Linq;

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
        List<Group> groupsList;
        Student selectedStudent;
        Subject selectedSubject;
        Group selectedGroup;
        bool changedOn = false;

        public MainWindow()
        {
            InitializeComponent();
            studentList = new List<Student>();
            visitsList = new List<Visit>();
            subjectsList = new List<Subject>();
            groupsList = new List<Group>();
            selectedStudent = new Student();
            UpDateSubjectsTable();
            UpDateGroupsTable();
        }
        //Other functions
        public async void UpDateStudentsTable()
        {
            studentList = await _db.Students.Where(s => s.Group.Id == selectedGroup.Id).ToListAsync();
            MainStudentListDG.ItemsSource = studentList;
            MainStudentListDG.Columns[5].Visibility = Visibility.Hidden;
            MainStudentListDG.Columns[6].Visibility = Visibility.Hidden;
        }
        public async void UpDateVisitsTable()
        {
            if (selectedStudent != null && selectedSubject != null)
            {
                visitsList = await _db.Visits.Where(v => v.Student.Id == selectedStudent.Id).Where(visit => visit.Subject.Id == selectedSubject.Id)
                .Include(visit => visit.Student).ToListAsync();
            }
            else visitsList = new List<Visit> { };
            StudentVisitationsListDG.ItemsSource = visitsList;
        }
        public async void UpDateSubjectsTable()
        {
            subjectsList = await _db.Subjects.ToListAsync();
            StudentSubjectListDG.ItemsSource = subjectsList;
        }
        public async void UpDateGroupsTable()
        {
            groupsList = await _db.Groups.ToListAsync();
            GroupsListDG.ItemsSource = groupsList;
            //GroupsListDG.Columns[2].Visibility = Visibility.Hidden;
        }
        //DataSelectionsChanged
        private void MainStudentListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainStudentListDG.SelectedIndex != -1)
            {
                AddVisitationBTN.IsEnabled = true;
                AddBTN.IsEnabled = false;
                ChangeBTN.IsEnabled = true;
                DeleteBTN.IsEnabled = true;
                selectedStudent = (Student)MainStudentListDG.Items[MainStudentListDG.SelectedIndex];
                UpDateVisitsTable();
            }
        }
        private async void GroupsListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupsListDG.SelectedIndex != -1)
            {
                selectedGroup = (Group)GroupsListDG.Items[GroupsListDG.SelectedIndex];
                UpDateStudentsTable();
                AddBTN.IsEnabled = true;
                DeleteGroupBTN.IsEnabled = true;
            }
        }
        private void StudentSubjectListDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSubject = (Subject)StudentSubjectListDG.Items[StudentSubjectListDG.SelectedIndex];
            UpDateVisitsTable();
            DeleteSubjectBTN.IsEnabled = true;
        }
        //Buttons
        private async void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (changedOn == true)
                {
                    if (FirstNameTB.Text != "" && LastNameTB.Text != "" && BirthDayDP.SelectedDate.ToString() != "")
                    {
                        var student = _db.Students.Where(c => c.Id == selectedStudent.Id).FirstOrDefault();
                        student.FirstName = FirstNameTB.Text;
                        student.LastName = LastNameTB.Text;
                        student.Birthday = (DateTime)BirthDayDP.SelectedDate;
                        student.Passport = new Passport(PassSerTB.Text, PassNumTB.Text);
                        await _db.SaveChangesAsync();
                        AddBTN.Content = "Добавить";
                        changedOn = false;
                        InputFieldCleaner();
                        UpDateStudentsTable();
                        MessageBox.Show("Данные ученика успешно изменены!");
                    }
                    else MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    if (FirstNameTB.Text != "" && LastNameTB.Text != "" && BirthDayDP.SelectedDate.ToString() != "")
                    {
                        await _db.Students.AddAsync(new Student()
                        {
                            FirstName = FirstNameTB.Text,
                            LastName = LastNameTB.Text,
                            Birthday = (DateTime)BirthDayDP.SelectedDate,
                            Group = selectedGroup,
                            Passport = new Passport(PassSerTB.Text, PassNumTB.Text)

                        });
                        InputFieldCleaner();
                        await _db.SaveChangesAsync();
                        UpDateStudentsTable();
                        MessageBox.Show("Успешно добавлен!");
                    }
                    else MessageBox.Show("Заполните все поля!");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void ChangeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (changedOn == false)
            {
                AddBTN.Content = "Принять \nизменения";
                FirstNameTB.Text = selectedStudent.FirstName;
                LastNameTB.Text = selectedStudent.LastName;
                BirthDayDP.SelectedDate = selectedStudent.Birthday;
                PassSerTB.Text = selectedStudent.Passport.series;
                PassNumTB.Text = selectedStudent.Passport.number;
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
            if (VistationDP.SelectedDate.ToString() != "" && selectedStudent != null && selectedSubject != null)
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
                MessageBox.Show("Дата посещения успешно добавлена!");
            }
            else MessageBox.Show("Выберите ученика, предмет и заполните поле даты(В таблице посещения)!");
        }
        private async void AddSubjectBTN_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectTB.Text != "" && SubjectTB.Text != null)
            {
                var subject = new Subject()
                {
                    Id = Guid.NewGuid(),
                    Name = SubjectTB.Text,
                };
                await _db.Subjects.AddAsync(subject);
                await _db.SaveChangesAsync();
                UpDateSubjectsTable();
                MessageBox.Show("Предмет успешно добавлен!");
            }
            else MessageBox.Show("Заполните поле названия предмета(В таблице предметов)!");
        }
        private async void DeleteSubjectBTN_Click(object sender, RoutedEventArgs e)
        {
            var Subject = _db.Subjects.Where(c => c.Id == selectedSubject.Id).FirstOrDefault();
            _db.Subjects.Remove(Subject);
            await _db.SaveChangesAsync();
            UpDateSubjectsTable();
            DeleteSubjectBTN.IsEnabled = false;
            MessageBox.Show("Предмет удален!");
        }
        private async void AddGroupBTN_Click(object sender, RoutedEventArgs e)
        {
            if (GroupTB.Text != "" && GroupTB.Text != null)
            {
                var group = new Group()
                {
                    Id = Guid.NewGuid(),
                    Name = GroupTB.Text,
                    CreatedDate = DateTime.Now
                };
                await _db.Groups.AddAsync(group);
                await _db.SaveChangesAsync();
                UpDateGroupsTable();
                MessageBox.Show("Группа успешно добавлен!");
            }
            else MessageBox.Show("Заполните поле названия группы(В таблице групп)!");
        }
        private async void DeleteGroupBTN_Click(object sender, RoutedEventArgs e)
        {
            var Group = _db.Groups.Where(c => c.Id == selectedGroup.Id).FirstOrDefault();
            _db.Groups.Remove(Group);
            await _db.SaveChangesAsync();
            AddBTN.IsEnabled = false;
            DeleteGroupBTN.IsEnabled = false;
            UpDateGroupsTable();
            MessageBox.Show("Группа успешно удалена");
        }
        private async void SearchStudentTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchStudentTB.Text != "Поиск...")
            {
                var oldtext = SearchStudentTB.Text;
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                if (oldtext == SearchStudentTB.Text) { return; }
                var studentsMatches = await _db.Students.Where(s => s.FirstName.Contains(SearchStudentTB.Text) || s.LastName.Contains(SearchStudentTB.Text)).ToListAsync();
                MainStudentListDG.ItemsSource = studentsMatches;
                var groupMatches = await _db.Groups.Where(g => g.Name.Contains(SearchStudentTB.Text)
                || g.Students!.Any(it => it.FirstName.Contains(SearchStudentTB.Text) || it.LastName.Contains(SearchStudentTB.Text))).ToListAsync();
                GroupsListDG.ItemsSource = groupMatches;
            }
        }

        private void SearchStudentTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchStudentTB.Text == "Поиск...")
                SearchStudentTB.Text = "";
        }

        private void SearchStudentTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchStudentTB.Text != "Поиск..." && MainStudentListDG.Items.Count == 0 || GroupsListDG.Items.Count == 0)
            {
                UpDateGroupsTable();
                SearchStudentTB.Text = "Поиск...";
            }
        }

        private void FirstNameTB_GotFocus(object sender, RoutedEventArgs e)
        {
            AddVisitationBTN.IsEnabled = false;
            AddBTN.IsEnabled = true;
            ChangeBTN.IsEnabled = false;
            DeleteBTN.IsEnabled = false;
            selectedStudent = null;
        }
        public void InputFieldCleaner()
        {
            FirstNameTB.Text = "";
            LastNameTB.Text = "";
            BirthDayDP.SelectedDate = null;
            PassSerTB.Text = "";
            PassNumTB.Text = "";
        }
    }
}
