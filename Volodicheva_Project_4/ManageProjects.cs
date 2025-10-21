using ProjectManager;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Task = ProjectManager.Task;

namespace Volodicheva_Project_4
{
    /// <summary>
    /// Работа с проектами и задачами
    /// </summary>
    public class ManageProjects
    {
        /// <summary>
        /// Добавление проектов
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <returns>Лист проектов измененный</returns>
        public List<Project> CreatProject(List<Project> projects)
        {
            bool repeat = true;
            while (repeat) 
            {
                Console.WriteLine("Введите ID проектa");
                string? id = Console.ReadLine();
                Console.WriteLine("Введите название проектa");
                string? name = Console.ReadLine();
                Console.WriteLine("Введите описание проектa по желанию");
                string? description = Console.ReadLine();
                if (id == String.Empty || name == String.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некоректные данные. Попробуйте снова.");
                    Console.ResetColor();
                }
                else if (projects.Any(p => p.IDProject == id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Проект с таким ID уже существует. Введите другой ID.");
                    Console.ResetColor();
                    continue; 
                }
                else
                {
                    projects.Add(new Project(id, name, description));
                    Console.WriteLine("Введите 'N', если хотите закончить ввод проектов.");
                    string? answer = Console.ReadLine();
                    if (answer == "N")
                    {
                        repeat = false;
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.WriteLine("Проект(ы) добавлены!");
                        Console.ResetColor();
                    }
                }

                

            }
            return projects;

        }
        /// <summary>
        /// Добавление задач
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <returns>Лист проектов измененный</returns>
        public List<Project> AddTask(List<Project> projects)
        {
            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("Введите ID проектa, к которому относится задача");
                string? idProject = Console.ReadLine();
                Console.WriteLine("Введите ID задачи");
                string? id = Console.ReadLine();
                Console.WriteLine("Введите название задачи");
                string? name = Console.ReadLine();
                Console.WriteLine("Введите описание задачи по желанию");
                string? description = Console.ReadLine();
                Console.WriteLine("Введите статус задачи (InProgress/Done/NotStarted)");
                string? status = Console.ReadLine();
                Console.WriteLine("Введите дедлайн задачи (В формате дд.мм.гггг)");
                string? deadline = Console.ReadLine();
                Project? projectToFind = projects.Find(p => p.IDProject == idProject);
                if (id == null || name == null || idProject == null || status == null || projectToFind == null || deadline == null || (!Enum.TryParse(status, true, out Task.TaskStatus status2)) || (!DateOnly.TryParse(deadline, out DateOnly deadline2)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некоректные данные. Попробуйте снова.");
                    Console.ResetColor();
                    continue;
                }
                else if (projectToFind.Tasks.Any(p => p.IDTask == id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Задача с таким ID уже существует. Введите другой ID.");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    projectToFind.AddTask(new Task(id, name, description, status, deadline));
                    Console.WriteLine("Введите 'N', если хотите закончить добавление задач.");
                    string? answer = Console.ReadLine();
                    if (answer == "N")
                    {
                        repeat = false;
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.WriteLine("Задача(и) успешно добавлена.");
                        Console.ResetColor();
                    }
                }
                

            }
            return projects;

        }
        /// <summary>
        /// Показ задач по выбранному проекту
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public void ShowTasks(List<Project> projects)
        {
            bool check = true;
            while (check) 
            {
                Console.WriteLine("Введите Id проекта, который вы хотите выбрать");
                string? projectID = Console.ReadLine();
                Project? projectToFInd = projects.Find(p => p.IDProject ==  projectID);
                if (projectToFInd == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный ID проекта.");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    Console.WriteLine(projectToFInd.ToString());
                    Console.WriteLine("\nСписок задач:");
                    foreach (Task task in projectToFInd.Tasks)
                    {
                        Console.WriteLine(task.ToString());
                    }
                    check = false;
                }
                
            }
        }
        /// <summary>
        /// Изменение проекта
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public void EditProject(List<Project> projects)
        {
            bool check = true;
            while (check)
            {
                Console.WriteLine("Введите Id проекта, который вы хотите отредактировать");
                string? projectID = Console.ReadLine();
                Project? projectToFind = projects.Find(p => p.IDProject == projectID);
                if (projectToFind == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный ID проекта.");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    Console.WriteLine("Введите новое название проекта (оставьте пустым, чтобы не изменять):");
                    string? newName = Console.ReadLine();
                    Console.WriteLine("Введите новое описание проекта (оставьте пустым, чтобы не изменять):");
                    string? newDescription = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        projectToFind.NameProject = newName;
                    }

                    if (!string.IsNullOrWhiteSpace(newDescription))
                    {
                        projectToFind.Description = newDescription;
                    }
                    Console.ForegroundColor= ConsoleColor.Green;
                    Console.WriteLine("Данные проекта успешно обновлены.");
                    Console.ResetColor();
                    check = false;
                }
            }
        }
        /// <summary>
        /// Изменение Задач по выбранному проекту
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public void EditTask(List<Project> projects)
        {
            bool check = true;
            while (check)
            {
                Console.WriteLine("Введите Id проекта, задачи которого вы хотите отредактировать");
                string? projectID = Console.ReadLine();
                Project? projectToFind = projects.Find(p => p.IDProject == projectID);
                if (projectToFind == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный ID проекта.");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    Console.WriteLine("Введите Id задачи, которую вы хотите отредактировать");
                    string? taskID = Console.ReadLine();
                    Task? taskToFind = projectToFind.Tasks.Find(t => t.IDTask == taskID);
                    if (taskToFind == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Некорректный ID проекта.");
                        Console.ResetColor();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Введите новое название задачи (оставьте пустым, чтобы не изменять):");
                        string? newName = Console.ReadLine();
                        Console.WriteLine("Введите новое описание задачи (оставьте пустым, чтобы не изменять):");
                        string? newDescription = Console.ReadLine();
                        Console.WriteLine("Введите новый статус задачи (InProgress/Done/NotStarted) (оставьте пустым, чтобы не изменять):");
                        string? newStatus = Console.ReadLine();
                        Console.WriteLine("Введите новый дедлайн задачи (В формате дд.мм.гггг) (оставьте пустым, чтобы не изменять):");
                        string? newDeadline = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                        {
                            taskToFind.NameTask = newName;
                        }

                        if (!string.IsNullOrWhiteSpace(newDescription))
                        {
                            taskToFind.DescriptionTask = newDescription;
                        }

                        if (!string.IsNullOrWhiteSpace(newStatus))
                        {
                            if (Enum.TryParse(newStatus, true, out Task.TaskStatus newStatus2))
                            {
                                taskToFind.StatusTask = newStatus2;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Неверный формат статуса. Используйте 'NotStarted', 'InProgress' или 'Done'.");
                                Console.ResetColor();
                                continue;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(newDeadline))
                        {
                            if (DateOnly.TryParse(newDeadline, out DateOnly newDeadline2))
                            {
                                taskToFind.DeadLineTask = newDeadline2;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Неверный формат дедлайна.");
                                Console.ResetColor();
                                continue;
                            }
                        }


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Данные задачи успешно обновлены.");
                        Console.ResetColor();
                        check = false;
                    }
                }
            }
        }
        /// <summary>
        /// Удаление проекта со всеми задачами
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public void RemoveProject(List<Project> projects)
        {
            bool check = true;
            while (check)
            {
                Console.WriteLine("Введите Id проекта, который вы хотите отредактировать");
                string? projectID = Console.ReadLine();
                Project? projectToFind = projects.Find(p => p.IDProject == projectID);
                if (projectToFind == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный ID проекта.");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    projects.Remove(projectToFind);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Проект успешно удален!");
                    Console.ResetColor();
                    check = false;
                }
            }
        }
        /// <summary>
        /// Удаление задачи в выбранномм проекте
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public void RemoveTask(List<Project> projects)
        {
            bool check = true;
            while (check)
            {
                Console.WriteLine("Введите Id проекта, задачи которого вы хотите отредактировать");
                string? projectID = Console.ReadLine();
                Project? projectToFind = projects.Find(p => p.IDProject == projectID);
                if (projectToFind == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный ID проекта.");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    Console.WriteLine("Введите Id задачи, которую вы хотите отредактировать");
                    string? taskID = Console.ReadLine();
                    Task? taskToFind = projectToFind.Tasks.Find(t => t.IDTask == taskID);
                    if (taskToFind == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Некорректный ID проекта.");
                        Console.ResetColor();
                        continue;
                    }
                    else
                    {
                        projectToFind.Tasks.Remove(taskToFind);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Задача успешно удалена!");
                        Console.ResetColor();
                        check = false;
                    }
                }
            }
        }

    }
}
