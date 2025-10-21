using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    /// <summary>
    /// Класс задачи
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Возможные статусы
        /// </summary>
        public enum TaskStatus
        {
            InProgress,
            Done,
            NotStarted
        }
        public string IDTask { get; set; }
        public string NameTask { get; set; }
        public string? DescriptionTask { get; set; }
        public TaskStatus StatusTask { get; set; }
        public DateOnly DeadLineTask { get; set; }
        /// <summary>
        /// Конструктор Задачи
        /// </summary>
        /// <param name="idTask">Id задачи</param>
        /// <param name="nameTask">название задачи</param>
        /// <param name="description">Описание </param>
        /// <param name="statusTask">статус</param>
        /// <param name="deadLine">дедлайн</param>
        public Task(string idTask, string nameTask, string? description, string statusTask, string deadLine) 
        { 
            IDTask = idTask;
            NameTask = nameTask;
            DescriptionTask = description;
            if (Enum.TryParse(statusTask, out TaskStatus status)) //Проверка на корректность статусы
            {
                StatusTask = status;
            }
            else
            {
                StatusTask = TaskStatus.NotStarted; // По умолчанию
            }
            DeadLineTask = DateOnly.Parse(deadLine);
        }
        /// <summary>
        /// Проверка просрочена ли задача
        /// </summary>
        /// <returns>Да или нет</returns>
        public bool IsOverdue()
        {
            return DateOnly.FromDateTime(DateTime.Now) > DeadLineTask && StatusTask != TaskStatus.Done;
        }
        /// <summary>
        /// Проверка на приближающийся дедлайн
        /// </summary>
        /// <returns>Да или нет</returns>
        public bool IsDeadlineEnds()
        {
            return DeadLineTask == DateOnly.FromDateTime(DateTime.Now.AddDays(1)) && StatusTask != TaskStatus.Done;
        }
        /// <summary>
        /// Переопределение для вывода
        /// </summary>
        /// <returns>строка с даннами о задаче</returns>
        public override string ToString()
        {
            if (DescriptionTask == null)
            { return $"\n Название задачи: {NameTask}" + $"\n Id: {IDTask}" + $"\n Статус: {StatusTask.ToString()}" + $"\n Дедлайн: {DeadLineTask.ToString()}"; }
            else
            { return $"\n Название задачи: {NameTask}" + $"\n Id: {IDTask}" + $"\n Описание: {DescriptionTask}" + $"\n Статус: {StatusTask.ToString()}" + $"\n Дедлайн: {DeadLineTask.ToString()}"; }
        }
    }
}
