using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ProjectManager;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Task = ProjectManager.Task;


namespace Volodicheva_Project_4
{
    /// <summary>
    /// Меню и взаимодействие с ним 
    /// </summary>
    public class MenuesManager
    {
        /// <summary>
        /// Основное меню
        /// </summary>
        public static void Menu()
        {
            Console.WriteLine("\r\nМеню. Для выбора введите номер.");
            Console.WriteLine("1. Просмотр списка проектов.");
            Console.WriteLine("2. Просмотр списка задач для выбранного проекта.");
            Console.WriteLine("3. Добавление нового проекта.");
            Console.WriteLine("4. Добавление новой задачи к выбранному проекту.");
            Console.WriteLine("5. Редактирование проекта (название, описание).");
            Console.WriteLine("6. Редактирование задачи (название, описание, статус).");
            Console.WriteLine("7. Удаление проекта (со всеми его задачами).");
            Console.WriteLine("8. Удаление задачи, по выбранному проекту.");
            Console.WriteLine("9. Визуализация иными методами");
            Console.WriteLine("10. Kanban-доска");
            Console.WriteLine("11. Вывести сообщение о просроченных и ближайщих дедлайнах.");
            Console.WriteLine("12. Экспортировать данные в файлы другого формата");
            Console.WriteLine("13. Завершить программу и сохранить все в исходный файл.");

        }
        /// <summary>
        /// Дополнительное меню для 9 выбора
        /// </summary>
        public static void Menu9()
        {
            Console.WriteLine("\r\nВыберите способ визуализации. (Введите цифру)");
            Console.WriteLine("1. Таблица");
            Console.WriteLine("2. Дерево");
            Console.WriteLine("3. Progress Bar");

        }
        /// <summary>
        /// Дополнительное меню для 9 выбора
        /// </summary>
        public static void Menu9_2()
        {
            Console.WriteLine("\r\nВыберите для чего таблицу. (Введите цифру)");
            Console.WriteLine("1. Для проектов");
            Console.WriteLine("2. Для задач");

        }
        /// <summary>
        /// Дополнительное меню для 11 выбора
        /// </summary>
        public static void Menu11()
        {
            Console.WriteLine("\r\nВыберите место выводаю (Введите цифру)");
            Console.WriteLine("1. Консоль");
            Console.WriteLine("2. Telegram Bot");
        }
        /// <summary>
        /// Дополнительное меню для 12 выбора
        /// </summary>
        public static void Menu12()
        {
            Console.WriteLine("\r\nВыберите формат вывода");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.WriteLine("3. HTML");
        }

        /// <summary>
        /// Работа с 9 выбором с консолью и вызов методов 
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public static void ChoieFor9(List<Project> projects)
        {
            bool check = true;
            while (check)
            {
                Menu9();
                string? answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        bool check2 = true;
                        while (check2)
                        {
                            Menu9_2();
                            Console.WriteLine();
                            string? answer2 = Console.ReadLine();
                            if (answer2 == "1")
                            {
                                ShowToConsole.DisplayProjectTable(projects);
                                check2 = false;
                                check = false;
                            }
                            else if (answer2 == "2")
                            {
                                bool repeat = true;
                                while (repeat)
                                {
                                    Console.WriteLine("Введите ID проектa, к которому относится задача");
                                    string? idProject = Console.ReadLine();
                                    Project? projectToFInd = projects.Find(p => p.IDProject == idProject);
                                    if (projectToFInd == null)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Некорректный ID проекта.");
                                        Console.ResetColor();
                                        continue;
                                    }
                                    else
                                    {
                                        ShowToConsole.DisplayTaskTable(projectToFInd.Tasks);
                                        repeat = false;
                                    }
                                }
                                check2 = false;
                                check = false;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Некорректный выбор.");
                                Console.ResetColor();
                                continue;
                            }
                        }
                        break;
                    case "2":
                        ShowToConsole.DisplayProjectTree(projects);
                        check = false;
                        break;
                    case "3":
                        ShowToConsole.DisplayProjectProgress(projects);
                        check = false;
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Некорректный выбор, попрбуйте снова");
                        Console.ResetColor();
                        break;
                }
            }
        }
        /// <summary>
        ///  Работа с 11 выбором с консолью и вызов методов 
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <param name="_botClient">Экземпляр ТГ-бота</param>
        public static void ChoiceFor11(List<Project> projects, ITelegramBotClient _botClient)
        {
            Console.WriteLine("Запусти бота по ссылке и введи /start (https://t.me/ProjectManager_Notification_bot)");
            Console.WriteLine("Запущен бот " + _botClient.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            _botClient.StartReceiving(
                (botClient, update, token) => TelegramNotifier.HandleUpdateAsync(botClient, update, token, projects),
                TelegramNotifier.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.WriteLine("Бот запущен. Нажмите Enter для остановки.");
            Console.ReadLine();
            cts.Cancel();
        }
        /// <summary>
        ///  Работа с 12 выбором с консолью и вызов методов 
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public static void ChoiceFor12(List<Project> projects)
        {
            try
            {
                bool check = true;
                while (check)
                {
                    Menu12();
                    string? answer = Console.ReadLine();
                    Console.WriteLine("\nВведите путь к файлу: ");
                    string filePath = Console.ReadLine();
                    ProcessFile processFile = new ProcessFile();
                    switch (answer)
                    {
                        case "1":
                            processFile.WriteToCsvAll(projects, $@"../../../{filePath}.csv");
                            check = false;
                            break;
                        case "2":
                            processFile.WriteToJson(projects, $@"../../../{filePath}.json");
                            check = false;
                            break;
                        case "3":
                            processFile.WriteToHtml(projects, $@"../../../{filePath}.html");
                            check = false;
                            break;
                        default:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Некорректный выбор, попрбуйте снова");
                            Console.ResetColor();
                            break;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу при записи данных в файл. {ex.Message}");
                Console.ResetColor();
            }
            catch (PathTooLongException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка названия файла. {ex.Message}");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
                Console.ResetColor();
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу. {ex.Message}");
                Console.ResetColor();
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу. {ex.Message}");
                Console.ResetColor();
            }
        }
    }   
}
