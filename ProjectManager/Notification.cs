using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectManager
{
    /// <summary>
    /// Уведомления о просроченных и ближайщих дедлайнах
    /// </summary>
    public static class Notification
    {
        /// <summary>
        /// Уведомления в консоль
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public static void NoticeToConsole(List<Project> projects)
        {
            foreach (Project project in projects)
            {
                foreach (Task task in project.Tasks)
                {
                    if (task.IsOverdue())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ПРОСРОЧЕНО \n Проект: {project.NameProject} (ID:{project.IDProject}) \n Задача: {task.NameTask} (ID:{task.NameTask}) \n Срок истек {task.DeadLineTask}");
                        Console.ResetColor();
                    }

                    if (task.IsDeadlineEnds())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"СРОК ИСТЕКАЕТ ЗАВТРА \n Проект: {project.NameProject} (ID:{project.IDProject}) \n Задача: {task.NameTask} (ID:{task.NameTask}) \n Срок истекает {task.DeadLineTask}");
                        Console.ResetColor();
                    }
                }
            }
        }
        /// <summary>
        /// Поиск просроченныз задач и составление уведомления для ТГ-бота о просроченных дедлайнах
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <returns>строка с сообщением</returns>
        public static string NoticeToTelegramOverdue(List<Project> projects)
        {
            foreach (Project project in projects)
            {
                foreach (Task task in project.Tasks)
                {
                    if (task.IsOverdue())
                    {
                        return $"ПРОСРОЧЕНО \n Проект: {project.NameProject} (ID:{project.IDProject}) \n Задача: {task.NameTask} (ID:{task.NameTask}) \n Срок истек {task.DeadLineTask}";
                    }
                }
            }
            return "Просроченных дедлайнов нет.";
        }
        /// <summary>
        ///  Поиск задач с ближайщим дедлайном и составление уведомления для ТГ-бота о ближайщих дедлайнах
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        /// <returns>Строка уведомления</returns>
        public static string NoticeToTelegramdeadlineEnds(List<Project> projects)
        {
            foreach (Project project in projects)
            {
                foreach (Task task in project.Tasks)
                {
                    if (task.IsDeadlineEnds())
                    {
                        return $"СРОК ИСТЕКАЕТ ЗАВТРА \n Проект: {project.NameProject} (ID:{project.IDProject}) \n Задача: {task.NameTask} (ID:{task.NameTask}) \n Срок истекает {task.DeadLineTask}";
                    }
                    
                    
                }
            }
            return "Ближайщих дедлайнов нет.";
        }
    }
}
