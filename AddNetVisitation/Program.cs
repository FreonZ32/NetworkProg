using AddNetVisitation;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

var dbservice = new StudentsVisitationService();
string thisPath = Environment.CurrentDirectory;
string? name;
thisPath += "\\myappdb.db";
var next = "\nДля продолжения нажмите любую клавишу.";
bool fileExist = File.Exists(thisPath);
if (!fileExist)
{
    dbservice.CreateTable();
    Console.WriteLine("Папка с бд не найдена! Создали новую. Для продолжения, нажмите любую кнопку.");
    Console.ReadLine();
}
var key = ConsoleKey.M;
while (key != ConsoleKey.Escape)
{
    switch(key)
    {
        case ConsoleKey.D1:
            
            Console.Write("\rВведите имя ученика:");
            name = Console.ReadLine();
            if (name != null && name != "" && Regex.IsMatch(name, "[a-zA-Z]") && !Regex.IsMatch(name, "\\s"))
            {
                if (dbservice.FindSameName(name) != true)
                {
                    Console.WriteLine("Ввести текущую дату? Y/N Вы так же можете отменить добавление ученика нажатием клавиши 'Esc'");
                    key = Console.ReadKey().Key;
                    do
                    {
                        switch (key)
                        {
                            case ConsoleKey.Y: dbservice.CreatePersonalTable(name, DateOnly.FromDateTime(DateTime.Now)); Console.WriteLine("\rДата успешно добавлена!" + next);
                                Console.ReadKey();
                                key = ConsoleKey.Escape;
                                    break;
                            case ConsoleKey.N:
                                while(key != ConsoleKey.Escape)
                                {
                                    Console.WriteLine("\rВведите дату формата дд-мм-гггг");
                                    string? userDate = Console.ReadLine();
                                    DateOnly date;
                                    if (DateOnly.TryParseExact(userDate, "dd-mm-yyyy", out date) == true)
                                    {
                                        dbservice.CreatePersonalTable(name, date);
                                        Console.WriteLine($"\rУченик успешно добавлен с датой '{date}'."+next);
                                        Console.ReadKey();
                                        key = ConsoleKey.Escape;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Дата введена не правильно! Пример: \"23-12-2001\"." + next);
                                        Console.ReadLine();
                                    }
                                }
                                break;
                        }
                    } while (key != ConsoleKey.Y && key != ConsoleKey.N && key != ConsoleKey.Escape);
                    key = ConsoleKey.M;
                }
                else
                {
                    Console.WriteLine("Ученик с таким именем уже есть!" + next);
                    Console.ReadKey();
                    key = ConsoleKey.M;
                }
            }
            else 
            { 
                if(name == null || name == "")Console.WriteLine("Поле ввода пусто!" + next);
                else Console.WriteLine("Неверный формат имени! Уберите пробелы, цифры и прочие знаки!" + next);
                Console.ReadKey();
                key = ConsoleKey.M;
            }
                break;
        case ConsoleKey.D2:
            Console.Write("\rВведите имя ученика:");
            List<VisitationPTB> visits = new List<VisitationPTB>();
            name = Console.ReadLine();
            if (name != null && name != "" && Regex.IsMatch(name, "[a-zA-Z]") && !Regex.IsMatch(name, "\\s"))
            {

                if (dbservice.FindSameName(name) == true)
                {
                    Console.Clear();
                    Console.WriteLine("Учащийся:"+name);
                    Console.WriteLine("Выберите одно из действий:\n1 - Добавить дату посещения\n2 - Показать все даты посещения\n3 - Удалить ученика");
                    key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            Console.WriteLine("\rВвести текущую дату? Y/N Вы так же можете отменить добавление ученика нажатием клавиши 'Esc'");
                            key = Console.ReadKey().Key;
                            do
                            {
                                switch (key)
                                {
                                    case ConsoleKey.Y:
                                        dbservice.FillVisitationByName(name, DateOnly.FromDateTime(DateTime.Now)); Console.WriteLine("\rДата успешно добавлена!" + next);
                                        Console.ReadKey();
                                        key = ConsoleKey.Escape;
                                        break;
                                    case ConsoleKey.N:
                                        while (key != ConsoleKey.Escape)
                                        {
                                            Console.WriteLine("\rВведите дату формата дд-мм-гггг");
                                            string? userDate = Console.ReadLine();
                                            DateOnly date;
                                            if (DateOnly.TryParseExact(userDate, "dd-mm-yyyy", out date) == true)
                                            {
                                                dbservice.FillVisitationByName(name, date);
                                                Console.WriteLine($"\rДата успешно добавлена!" + next);
                                                Console.ReadKey();
                                                key = ConsoleKey.Escape;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Дата введена не правильно! Пример: \"23-12-2001\"." + next);
                                                Console.ReadLine();
                                            }
                                        }
                                        break;
                                }
                            } while (key != ConsoleKey.Y && key != ConsoleKey.N && key != ConsoleKey.Escape);
                            key = ConsoleKey.M;
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            Console.WriteLine("Учащийся:" + name);
                            visits = dbservice.ShowVisitationByName(name);
                            foreach (VisitationPTB visit in visits)
                            {
                                Console.Write(visit.Date.ToString() + " ");
                            }
                            Console.Write("\n" + next);
                            Console.ReadLine();
                            break;
                        case ConsoleKey.D3:
                            dbservice.DeletePersonalTable(name);
                            Console.Write("\nДанные успешно удалены!" + next);
                            Console.ReadLine();
                            break;
                    }
                }
                else
                    {
                        Console.Write("Такого имени не найдено!");
                        Console.Write("\n" + next);
                        Console.ReadLine();
                    }
                }
            else Console.WriteLine("Неверный формат имени! Уберите пробелы, цифры и прочие знаки!" + next);
                Console.ReadKey();
                key = ConsoleKey.M;
                break;
        case ConsoleKey.D3: 
            dbservice.ShowAllVisitations(); 
            Console.WriteLine(next);Console.ReadLine(); 
            key = ConsoleKey.M; 
            break;
    }
    if(key == ConsoleKey.M) 
    {
        Console.Clear();
        Console.WriteLine("Выберите действие в вашей бд:" +
            "\n1 - Добавить ученика;" +
            "\n2 - Выбрать ученика;" +
            "\n3 - Вывести всех учашихся с датами посещения;" +
            "\nEsc - Выход из программы.\n");
    }
    key = Console.ReadKey().Key;
}


