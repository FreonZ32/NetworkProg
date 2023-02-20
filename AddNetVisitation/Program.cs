using AddNetVisitation;

var dbservice = new StudentsVisitationService();
//dbservice.CreateTable();
var key = ConsoleKey.M;
while (key != ConsoleKey.Escape)
{
    switch(key)
    {
        case ConsoleKey.D1:
            Console.Write("\rВведите имя ученика: ");
            string? name = Console.ReadLine();
            if (name != null && name != "")
            {
                if (dbservice.FindSameName(name) != true)
                {
                    Console.WriteLine("Ввести текущую дату? Y/N Вы так же можете отменить добавление ученика нажатием клавиши 'Esc'");
                    key = Console.ReadKey().Key;
                    do
                    {
                        switch (key)
                        {
                            case ConsoleKey.Y: dbservice.FillVisitations(name, DateOnly.FromDateTime(DateTime.Now)); Console.WriteLine("Ученик добавлен!"); break;
                            case ConsoleKey.N:
                                Console.WriteLine("\rВведите дату формата дд-мм-гггг");
                                string? userDate = Console.ReadLine();
                                DateOnly date;
                                if (DateOnly.TryParseExact(userDate,"dd-mm-yyyy", out date) == true)
                                {
                                    dbservice.FillVisitations(name, date);
                                    Console.WriteLine($"\rУченик успешно добавлен с датой '{date}'. \nДля продолжения нажмите любую клавишу.");
                                    Console.ReadKey();
                                    key = ConsoleKey.Escape;
                                }
                                else Console.WriteLine("Дата введена не правильно!");
                                break;
                            default: break;
                        }
                    } while (key != ConsoleKey.Y && key != ConsoleKey.N && key != ConsoleKey.Escape);
                    if (key == ConsoleKey.Escape) key = ConsoleKey.M;
                }
                else
                {
                    Console.WriteLine("Ученик с таким именем уже есть! \nДля продолжения нажмите любую клавишу.");
                    Console.ReadKey();
                    key = ConsoleKey.M;
                }
            }
            else 
            { 
                Console.WriteLine("Поле ввода пусто! \nДля продолжения нажмите любую клавишу.");
                Console.ReadKey();
                key = ConsoleKey.M;
            }
            ;break;
        case ConsoleKey.D2:Console.WriteLine("\rНажата клавиша 2;\n");break;
        case ConsoleKey.D3:Console.WriteLine
            ("\r" +string.Join
                (
                    Environment.NewLine, dbservice.GetVisitations().Select
                    (it => it.ToString())
                )
            );
            Console.Write("Для продолжения нажмите любую кнопку.");
            Console.ReadKey();
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


