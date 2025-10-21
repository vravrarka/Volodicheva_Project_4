using System.ComponentModel;

namespace ProjectManager
{
    /// <summary>
    /// Класс Проекта
    /// </summary>
    public class Project
    {
        public string? IDProject { get; set; } 
        public string? NameProject { get; set; }
        public string? Description { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
        /// <summary>
        /// Конструктор проекта
        /// </summary>
        /// <param name="id">ID проекта</param>
        /// <param name="name">название</param>
        /// <param name="description">описание</param>
        public Project(string? id, string? name, string? description)
        {
            IDProject = id;
            NameProject = name;
            Description = description;
        }
        /// <summary>
        /// Добавление задач в лист
        /// </summary>
        /// <param name="task">Задача</param>
        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }
        /// <summary>
        /// Переопределение для вывода
        /// </summary>
        /// <returns>Данные о проекте</returns>
        public override string ToString()
        {
            if (Description == null)
               { return $"\n Название проекта: {NameProject}" + $"\n Id: {IDProject}"; }
            else 
               { return $"\n Название проекта: {NameProject}" + $"\n Id: {IDProject}" + $"\n Описание: {Description}"; }
        }

    }
}
