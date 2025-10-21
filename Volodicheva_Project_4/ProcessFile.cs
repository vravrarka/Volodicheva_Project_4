using ProjectManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Task = ProjectManager.Task;

namespace Volodicheva_Project_4
{
    /// <summary>
    /// Взаимодействие с файлами
    /// </summary>
    public class ProcessFile
    {
        internal string? pathFile; //Файл с задачами
        /// <summary>
        /// Чтение начального файла
        /// </summary>
        /// <param name="projects">Лист с проектами</param>
        public void ReadFile(List<Project> projects)
        {
            bool fileReadSuccessfully = false;
            while (!fileReadSuccessfully) 
            { 
                try
                {
                    Console.WriteLine("Введите путь к файлу");
                    pathFile = Console.ReadLine();
                    StreamReader streamReader = new StreamReader(pathFile);
                    streamReader.ReadLine();
                    int count = 0;
                    while (!streamReader.EndOfStream)
                    {
                        string? line = streamReader.ReadLine();
                        count++;
                        string[] lines = line.Split(";"); 

                        if (lines.Length == 6 || !string.IsNullOrWhiteSpace(lines[0]) || !string.IsNullOrWhiteSpace(lines[1]) || !string.IsNullOrWhiteSpace(lines[2]) || !string.IsNullOrWhiteSpace(lines[4]) || !string.IsNullOrWhiteSpace(lines[5]))
                        {
                            Console.WriteLine(count);
                            if (!Enum.TryParse(lines[4].Trim(), ignoreCase: true, out Task.TaskStatus status)) //Проверка на корректность статуса
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Неверный статус задачи в строке. Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }

                            if (!DateOnly.TryParse(lines[5], out DateOnly deadLine))//Проверка на корректность даты
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Неверный дедлайн задачи в строке. Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                            
                            try
                            {
                                foreach (Project project in projects)
                                {
                                    if (project.Tasks.Any(p => p.IDTask == lines[0]))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("Задача с таким ID уже существует. Строка не учитывается.");
                                        Console.ResetColor();
                                        continue;
                                    }
                                    if (project.IDProject == lines[1] && !project.Tasks.Any(t => t.IDTask == lines[0]))
                                    {
                                        project.AddTask(new Task(lines[0], lines[2], lines[3], lines[4], lines[5])); //заполняем лист задач
                                    }
                                }
                            }
                            catch (ArgumentNullException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Ошибка в формате строки. {ex.Message} Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                            catch (FormatException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Ошибка в формате строки. {ex.Message} Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Ошибка в формате строки или данных. {ex.Message} Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Ошибка в формате строки или данных. {ex.Message} Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                            catch (OverflowException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Ошибка в переполнении. {ex.Message} Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Ошибка в формате строки. {ex.Message} Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Ошибка в формате строки. {ex.Message} Строка не учитывается.");
                                Console.ResetColor();
                                continue;
                            }
                        }
                        else
                        {
                            streamReader.ReadLine(); 
                        }
                    }
                    streamReader.Close();
                    fileReadSuccessfully = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Файл успешно загружен!");
                    Console.ResetColor();
                }
                catch (FileNotFoundException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Файл не найден. Попробуйте снова.");
                    Console.ResetColor();
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Указанный путь не существует. Попробуйте снова.");
                    Console.ResetColor();
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Нет доступа к файлу. Попробуйте снова.");
                    Console.ResetColor();
                    continue;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Возникла ошибка: {ex.Message}");
                    Console.ResetColor();
                    continue;
                }

                if (!fileReadSuccessfully)
                {
                    Console.WriteLine("Попробуйте еще раз");
                    Console.Clear();
                }

            }
             
        }


        /// <summary>
        /// Запись задач в конце программы в исходный файл
        /// </summary>
        /// <param name="projects">Лист с проектами</param>
        public void WtiteToCsv(List<Project> projects)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(pathFile);
                streamWriter.WriteLine("ID Задачи;ID Проекта;Название задачи;Описание задачи;Статус задачи;Дедлайн"); // Записывает заголовок
                foreach (Project project in projects)
                {
                    foreach (Task task in project.Tasks)
                    {
                        streamWriter.WriteLine($"{task.IDTask};{project.IDProject};{task.NameTask};{task.DescriptionTask};{task.StatusTask};{task.DeadLineTask}"); //Записываем данные по каждой задаче в строку 
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Данные успешно загружены в файл {Path.GetFileName(pathFile)}!");
                Console.ResetColor();
                streamWriter.Close(); //Закрываем поток записи 
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу при записи данных в файл. {ex.Message}");
                Console.ResetColor();
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
                Console.ResetColor();
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу. {ex.Message}");
                Console.ResetColor();
                throw;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу. {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
        /// <summary>
        /// Запись в файл CSV проектов и задач к ним 
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <param name="filePath">путь к файлу</param>
        public void WriteToCsvAll(List<Project> projects, string filePath)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);
                
                foreach (Project project in projects)
                {
                    streamWriter.WriteLine("ID Проекта;Название проекта;Описание проекта"); // Записывает заголовок
                    streamWriter.WriteLine($"{project.IDProject};{project.NameProject};{project.Description}");
                    streamWriter.WriteLine("Задачи проекта");
                    streamWriter.WriteLine("ID Задачи; ID Проекта; Название задачи; Описание задачи; Статус задачи; Дедлайн"); // Записываем заголовок дял задач
                    foreach (Task task in project.Tasks)
                    {
                        streamWriter.WriteLine($"{task.IDTask};{project.IDProject};{task.NameTask};{task.DescriptionTask};{task.StatusTask};{task.DeadLineTask}"); //Записываем данные по каждой задаче в строку 
                    }
                    streamWriter.WriteLine("");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Данные успешно загружены в файл!");
                Console.ResetColor();
                streamWriter.Close(); 
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу при записи данных в файл. {ex.Message}");
                Console.ResetColor();
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
                Console.ResetColor();
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу. {ex.Message}");
                Console.ResetColor();
                throw;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Возникла ошибка доступа к файлу. {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
        /// <summary>
        /// Запись проектов и задач в JSON файл
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <param name="filePath">путь к файлу</param>
        public void WriteToJson(List<Project> projects, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(projects, options);
            File.WriteAllText(filePath, json);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Данные успешно загружены в файл!");
            Console.ResetColor();
        }
        /// <summary>
        /// Запись проектов и задач в html-файл
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <param name="filePath">Путь к файлу</param>
        public void WriteToHtml(List<Project> projects, string filePath)
        {
            var template = File.ReadAllText($@"../../../template.html");
            var content = new StringBuilder();

            foreach (var project in projects)
            {

                content.AppendLine($"<div class='project'>");
                content.AppendLine($"<h2>Проект: {project.NameProject} (ID: {project.IDProject})</h2>");
                content.AppendLine($"<p>Описание: {project.Description}</p>");

                content.AppendLine("<h3>Задачи:</h3>");
                content.AppendLine("<ul>");

                foreach (var task in project.Tasks)
                {
                    content.AppendLine($"<li class='task'>");
                    content.AppendLine($"<strong>Задача:</strong> {task.NameTask} (ID: {task.IDTask})<br>");
                    content.AppendLine($"<strong>Дедлайн:</strong> {task.DeadLineTask:dd-MM-yyyy}<br>");
                    content.AppendLine($"<strong>Статус:</strong> {task.StatusTask.ToString()}<br>");
                    content.AppendLine($"<strong>Описание:</strong> {task.DescriptionTask}<br>");
                    content.AppendLine("</li>");
                }

                content.AppendLine("</ul>");
                content.AppendLine("</div>");
            }

            // Заменяем {{CONTENT}}
            var htmlContent = template.Replace("{{CONTENT}}", content.ToString());
            File.WriteAllText(filePath, htmlContent);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Данные успешно загружены в файл!");
            Console.ResetColor();
        }
    }
}

