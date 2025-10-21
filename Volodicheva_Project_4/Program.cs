using System;
using ProjectManager;
using Telegram.Bot;
using Volodicheva_Project_4;
using Telegram.Bot.Types;
/// <summary>
/// Володичева Варвара
/// БПИ-247
/// 4 вариант
/// B-side
/// </summary>
class Program
{
    public static ITelegramBotClient _botClient = new TelegramBotClient("7473330660:AAHs4f1Wj0wp5mFfEV7q7HRNpu8HTT4fRZ8"); //Создаем экземпляр для взаимодействия с Тг-ботом
    /// <summary>
    /// Взаимодействие с основным меню и всеми классами
    /// </summary>
    public static void Main()
    {
        List<Project> projects = new List<Project>();
        ConsoleKeyInfo keyToExit; //Позволяет выйти из программы 
        do
        {
            try
            {
                ManageProjects manageProjects = new ManageProjects();
                projects = manageProjects.CreatProject(projects);//Создаем проекты
                Console.Clear();
                ProcessFile processFile = new ProcessFile();
                processFile.ReadFile(projects); //Загружаем файл с задачами
                Notification.NoticeToConsole(projects); //Сообщение о просроченных и ближайщих дедланах 
                while (true)
                {
                    MenuesManager.Menu();
                    var key = Console.ReadLine();
                    switch (key)
                    {
                        case "1":
                            Console.Clear();
                            foreach (Project project in projects)
                            {
                                Console.WriteLine(project.ToString());
                            }
                            break;
                        case "2":
                            Console.Clear();
                            manageProjects.ShowTasks(projects);
                            break;
                        case "3":
                            Console.Clear();
                            projects = manageProjects.CreatProject(projects);
                            break;
                        case "4":
                            Console.Clear();
                            projects = manageProjects.AddTask(projects);
                            break;
                        case "5":
                            Console.Clear();
                            manageProjects.EditProject(projects);
                            break;
                        case "6":
                            Console.Clear();
                            manageProjects.EditTask(projects);
                            break;
                        case "7":
                            Console.Clear();
                            manageProjects.RemoveProject(projects);
                            break;
                        case "8":
                            Console.Clear();
                            manageProjects.RemoveTask(projects);    
                            break;
                        case "9":
                            Console.Clear();
                            MenuesManager.ChoieFor9(projects);
                            break;
                        case "10":
                            Console.Clear();
                            ShowToConsole.Kanban(projects);
                            break;
                        case "11":
                            Console.Clear();
                            MenuesManager.ChoiceFor11(projects, _botClient);
                            break;
                        case "12":
                            Console.Clear();
                            MenuesManager.ChoiceFor12(projects);
                            break;
                        case "13":
                            processFile.WtiteToCsv(projects);
                            return;
                        default: //Выводится при некорректном использовании меню.
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Некорректный выбор, попрбуйте снова");
                                Console.ResetColor();
                                break;
                            }
                    }

                }
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Данные некорректны");
                Console.WriteLine("Попробуйте снова, нажав любую клавишу");
                Console.ResetColor();

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
                Console.ResetColor();
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
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
                Console.WriteLine("Данные некорректны");
                Console.WriteLine("Попробуйте снова, нажав любую клавишу");
                Console.ResetColor();
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
                Console.ResetColor();
                break;
            }
            keyToExit = Console.ReadKey();
        }
        while (keyToExit.Key != ConsoleKey.Escape);

    }
}