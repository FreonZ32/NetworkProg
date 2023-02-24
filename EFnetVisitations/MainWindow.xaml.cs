using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            studentList = new List<Student>();
            UpDateMainTable();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(FirstNameTB.Text!=""&&LastNameTB.Text!="")
            {
                await _db.Students.AddAsync(new Student() { Name = FirstNameTB.Text + " " + LastNameTB.Text, Birthday = (DateTime)BirthDayDP.SelectedDate});
                await _db.SaveChangesAsync();
                UpDateMainTable();
                MessageBox.Show("Успешно добавлен!");
            }
        }
        public async void UpDateMainTable() 
        {
            studentList = await _db.Students.ToListAsync();
            MainStudentListDG.ItemsSource = studentList;
        }
    }
}
