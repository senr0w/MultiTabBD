using Microsoft.Data.SqlClient;
using System;
using System.Reflection.PortableExecutable;

class BD
{
    static async Task Main(string[] args)
    {
        while (true) { 

            string conectBD= "Server=DESKTOP-5BD88QO\\SQLEXPRESS;Database=MultiTablesBD;Trusted_Connection=True;TrustServerCertificate=true";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите таблицу для дальнейших действий");
            Console.ResetColor();
            Console.WriteLine("1. Пользователи");
            Console.WriteLine("2. Заказы");
            Console.WriteLine("3. Объединение\n");
       
            int N = Convert.ToInt16(Console.ReadLine());
            switch (N)
            {
                //Действия с таблицей пользователей
                case 1:
                    using (SqlConnection conect = new SqlConnection(conectBD))
                    {
                        conect.Open();
                        string sql = $"SELECT * FROM Users;";
                        using(SqlCommand command=new SqlCommand(sql, conect))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("\nПользователи\n");
                            Console.ResetColor();
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                
                              if (reader.HasRows) // если есть данные
                              {
                                        string columnName1 = reader.GetName(0);
                                        string columnName2 = reader.GetName(1);
                                        string columnName3 = reader.GetName(2);
                                        string columnName4 = reader.GetName(3);
                                    //Шапка таблицы
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"{columnName1}  {columnName2}    {columnName3}   {columnName4}");
                                    Console.ResetColor();
                                    
                                    //Чтение данных из таблицы
                                    while (await reader.ReadAsync())
                                    {   
                                        object UserID=reader.GetValue(0);
                                        object Имя=reader.GetValue(1);
                                        object Отчество=reader.GetValue(2);
                                        object Фамилия=reader.GetValue(3);
                                        Console.WriteLine($"{UserID}       {Имя}   {Отчество}      {Фамилия} ");
                                    }
                                 
                              }
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nВыберите действие");
                        Console.ResetColor();
                        Console.WriteLine("1. Создать пользователя");
                        Console.WriteLine("2. Обновить данные пользователя");
                        Console.WriteLine("3. Удалить");
                        Console.WriteLine("4. Вернуться назад\n");


                        int L = Convert.ToInt16(Console.ReadLine());
                        switch (L)
                        {
                            //Добавление нового пользователя
                            case 1:
                                try
                                {
                                    using (SqlCommand createUser = new SqlCommand(sql, conect))
                                    {
                                        int num= await createUser.ExecuteNonQueryAsync();
                                        Console.WriteLine("\nВведите Имя:");
                                        string firstname=Console.ReadLine();
                                        Console.WriteLine("\nВведите Отчество");
                                        string secondname=Console.ReadLine();
                                        Console.WriteLine("\nВведите Фамилию");
                                        string lastname = Console.ReadLine();

                                        sql =$"INSERT INTO Users (Имя,Отчество,Фамилия) VALUES ('{firstname}','{secondname}','{lastname}')";
                                        createUser.CommandText= sql;
                                        num= await createUser.ExecuteNonQueryAsync();   
                                        Console.WriteLine($"\nДобавлено объектов: {num}");
                                     }
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nОШИБКА!!! Некорректный ввод данных\n");
                                    Console.ResetColor();
                                }
                                break;
                                //Обновление данных с возможностью выбрать,что обновить
                                case 2:
                                using (SqlCommand updateUser = new SqlCommand( sql, conect))
                                {
                                    int up=await updateUser.ExecuteNonQueryAsync();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\nВыберите действие");
                                    Console.ResetColor();
                                    Console.WriteLine("1. Изменить Имя");
                                    Console.WriteLine("2. Изменить Отчество");
                                    Console.WriteLine("3. Изменить Фамилию");
                                    Console.WriteLine("4. Вернуться назад\n");

                                    //Выбор что обновить
                                    int obnova =Convert.ToInt16(Console.ReadLine());
                                    switch (obnova)
                                    {
                                        case 1:
                                            Console.WriteLine("Введите ID пользователя для изменения");
                                            string userID=Console.ReadLine();
                                            Console.WriteLine("\nВведите новое Имя:");
                                            string firstname = Console.ReadLine();
                                            sql = $"UPDATE Users set Имя='{firstname}' WHERE UserID={userID}";

                                           break;
                                        case 2:
                                            Console.WriteLine("Введите ID пользователя для изменения");
                                            string useID = Console.ReadLine();
                                            Console.WriteLine("\nВведите новую Отчество:");
                                            string second = Console.ReadLine();
                                            sql = $"UPDATE Users set Отчество='{second}' WHERE UserID={useID}";

                                            break;
                                        case 3:
                                            Console.WriteLine("Введите ID пользователя для изменения");
                                            string usID = Console.ReadLine();
                                            Console.WriteLine("\nВведите новую Фамилию:");
                                            string last = Console.ReadLine();
                                            sql = $"UPDATE Users set Фамилия='{last}' WHERE UserID={usID}";

                                            break;
                                    }
                                  
                                    updateUser.CommandText= sql;
                                    up= await updateUser.ExecuteNonQueryAsync(); 
                                }
                                break;  
                                //Удаление пользователя
                                case 3:
                                try
                                {
                                    using(SqlCommand deleteUser=new SqlCommand(sql, conect))
                                    {
                                        int del=await deleteUser.ExecuteNonQueryAsync();            
                                        Console.WriteLine("Введите ID пользователя для удаления");
                                        string delID=Console.ReadLine();
                                        sql = $"use MultiTablesBD delete from Users where UserID={delID}";
                                        deleteUser.CommandText= sql;
                                        del= await deleteUser.ExecuteNonQueryAsync();
                                        Console.WriteLine($"\nУдалено объектов: {del}");
                                    }
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nОШИБКА!!! Некорректный ввод данных\n");
                                    Console.ResetColor();
                                }
                                break;
                        }
                    }

                //Действия с таблицей заказов
                    break;
                case 2:
                    using (SqlConnection conect = new SqlConnection(conectBD))
                    {
                        conect.Open();
                        string sql = $"SELECT * FROM Orders;";
                        using (SqlCommand command = new SqlCommand(sql, conect))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\nЗаказы\n");
                            Console.ResetColor();
                            using (SqlDataReader read = command.ExecuteReader())
                            {
                                if (read.HasRows) // если есть данные
                                {
                                    string columnName1 = read.GetName(0);
                                    string columnName2 = read.GetName(1);
                                    string columnName3 = read.GetName(2);
                
                                    //Шапка таблицы
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"{columnName1}  {columnName2}    {columnName3}   ");
                                    Console.ResetColor();
                                    //Чтение данных из таблицы
                                    while (await read.ReadAsync())
                                    {
                                        object OrderID = read.GetValue(0);
                                        object UserID = read.GetValue(1);
                                        object OrderDate = read.GetValue(2);
                                        Console.WriteLine($"{OrderID}        {UserID}         {OrderDate}");
                                    }
                                }
                      
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nВыберите действие");
                        Console.ResetColor();
                        Console.WriteLine("1. Создать заказ");
                        Console.WriteLine("2. Обновить данные заказа");
                        Console.WriteLine("3. Удалить");
                        Console.WriteLine("4. Вернуться назад\n");
                        int M=Convert.ToInt32(Console.ReadLine());
                        switch(M)
                        {
                            //Create Заказа
                            case 1:
                                try
                                {
                                    Console.WriteLine("Создание заказа...");
                                    using(SqlCommand createOrder=new SqlCommand(sql, conect))
                                    {
                                        int crt=await createOrder.ExecuteNonQueryAsync();
                                        Console.WriteLine("Введите ID пользователя:");
                                        string crtID=Console.ReadLine();
                                        Console.WriteLine("Введите дату заказа");
                                        string dateOrder=Console.ReadLine();
                                        sql = $"insert into Orders (UserID,OrderDate) values ('{crtID}','{dateOrder}')";
                                        createOrder.CommandText = sql;
                                        crt= await createOrder.ExecuteNonQueryAsync();
                                        Console.WriteLine($"\nУдалено объектов: {crt}");
                                    }
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nОШИБКА!!! Некорректный ввод данных\n");
                                    Console.ResetColor();
                                }
                         
                            break;

                                //Update Заказа
                            case 2:
                                try
                                {
                                    Console.WriteLine("Редактирование заказа...");
                                    using (SqlCommand updateOrder = new SqlCommand(sql, conect))
                                    {
                                        int upOrd = await updateOrder.ExecuteNonQueryAsync();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\nВыберите действие");
                                        Console.ResetColor();
                                        Console.WriteLine("1. Изменить ID пользователя");
                                        Console.WriteLine("2. Изменить дату заказа");
                                        Console.WriteLine("3. Вернуться назад\n");

                                        int O = Convert.ToInt16(Console.ReadLine());
                                        switch (O)
                                        {
                                            
                                            case 1:
                                                try
                                                {
                                                    Console.WriteLine("Введите ID заказа для редактирования");
                                                    string OID = Console.ReadLine();
                                                    Console.WriteLine("Введите ID пользователя для изменения");
                                                    string userID = Console.ReadLine();
                                                    sql = $"update Orders set UserID='{userID}' where OrderID='{OID}'";
                                                    updateOrder.CommandText = sql;
                                                    upOrd = await updateOrder.ExecuteNonQueryAsync();
                                                }
                                                catch
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nОШИБКА!!! Некорректный ввод данных\n");
                                                    Console.ResetColor();
                                                }
                                                break;
                                            case 2:
                                                Console.WriteLine("Введите ID заказа для редактирования");
                                                string OrID = Console.ReadLine();
                                                Console.WriteLine("Введите новую дату заказа");
                                                string dateOr = Console.ReadLine();
                                                sql = $"update Orders set OrderDate='{dateOr}' where OrderID='{OrID}'";
                                                updateOrder.CommandText = sql;
                                                upOrd = await updateOrder.ExecuteNonQueryAsync();
                                                break;
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nОШИБКА!!! Некорректный ввод данных\n"); 
                                    Console.ResetColor();
                                }
                                
                                break;  

                                //Delete Заказа
                                case 3:
                                try
                                {
                                    Console.WriteLine("Удаление заказа");
                                    using (SqlCommand deleteOrder = new SqlCommand(sql, conect)) 
                                    {
                                        int del = await deleteOrder.ExecuteNonQueryAsync();
                                        Console.WriteLine("Введите ID заказа для удаления");
                                        string orID=Console.ReadLine();
                                        sql=$"delete from Orders where OrderID='{orID}'"; 
                                        deleteOrder.CommandText = sql;
                                        del = await deleteOrder.ExecuteNonQueryAsync();
                                        Console.WriteLine($"\nУдалено объектов: {del}");
                                    } 
                                }
                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nОШИБКА!!! Некорректный ввод данных\n");
                                    Console.ResetColor();
                                }
                                break;
                        }
                    }
                    break;
                    //Сложный запросик
                    case 3:
                        using (SqlConnection conect = new SqlConnection(conectBD))
                        {
                            conect.Open();
                            string sql = $"  SELECT Users.Имя, Orders.OrderDate FROM Users  INNER JOIN Orders ON Users.UserID = Orders.UserID;";
                            using (SqlCommand command = new SqlCommand(sql, conect))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("\nОбъединение\n");
                                Console.ResetColor();
                                using (SqlDataReader read = command.ExecuteReader())
                                {
                                    if (read.HasRows) // если есть данные
                                    {
                                        string columnName1 = read.GetName(0);
                                        string columnName2 = read.GetName(1);

                                        //Шапка таблицы
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine($"{columnName1}\t\t     {columnName2}");
                                        Console.ResetColor();
                                        //Чтение данных из таблицы
                                        while (await read.ReadAsync())
                                        {
                                            object UserID = read.GetValue(0);
                                            object OrderDate = read.GetValue(1);
                                            Console.WriteLine($"{UserID}\t         {OrderDate}\n");
                                        }
                                    }
                                }
                            }
                        }
                        break;
            }
        }
    }
}