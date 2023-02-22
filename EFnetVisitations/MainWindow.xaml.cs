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
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await using var db = new DBContext();
            await db.Database.EnsureCreatedAsync();
            await db.Students.AddAsync(new Student() { Name = "Adam", Birthday = DateTime.Now });
            await db.Students.AddAsync(new Student() { Name = "Saver", Birthday = DateTime.Today.AddDays(-2) });
            await db.Students.AddAsync(new Student() { Name = "Gagga", Birthday =  DateTime.Today.AddDays(-1)});
            await db.SaveChangesAsync();
            var students = await db.Students.ToListAsync();
            studentsDG.ItemsSource = students;
        }
    }
}
