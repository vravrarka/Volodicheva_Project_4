using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console;

namespace ProjectManager
{
    /// <summary>
    /// Класс для визуализации в консоль с помощью Spectre.Consol
    /// </summary>
    public static class ShowToConsole
    {
        /// <summary>
        /// Визуализация таблицы с проектами
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public static void DisplayProjectTable(List<Project> projects)
        {
            var table = new Table();
            table.Border = TableBorder.Square;
            table.AddColumn(new TableColumn($"[bold]ID Проекта[/]").Centered());
            table.AddColumn(new TableColumn($"[bold]Название[/]").Centered());
            table.AddColumn(new TableColumn($"[bold]Описание[/]").Centered());

            foreach (Project project in projects)
            {
                table.AddRow(
                    project.IDProject,
                    project.NameProject,
                    project.Description ?? "-"
                );
            }

            AnsiConsole.Write(table);
        }
        /// <summary>
        /// Визуализация таблицы с задачами
        /// </summary>
        /// <param name="tasks">Лист задач</param>
        public static void DisplayTaskTable(List<Task> tasks)
        {
            var table = new Table();
            table.Border = TableBorder.Square;
            table.AddColumn(new TableColumn($"[bold]ID Задачи[/]").Centered());
            table.AddColumn(new TableColumn($"[bold]Название[/]").Centered());
            table.AddColumn(new TableColumn($"[bold]Описание[/]").Centered());
            table.AddColumn(new TableColumn($"[bold]Статус[/]").Centered());
            table.AddColumn(new TableColumn($"[bold]Дедлайн[/]").Centered());

            foreach (Task task in tasks)
            {
                table.AddRow(
                    task.IDTask,
                    task.NameTask,
                    task.DescriptionTask ?? "-",
                    task.StatusTask.ToString(),
                    task.DeadLineTask.ToString()
                );
            }
            AnsiConsole.Write(table);

        }
        /// <summary>
        /// Визуализация дерева проектов и их задач
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public static void DisplayProjectTree(List<Project> projects)
        {
            var root = new Tree("[bold blue]Проекты[/]");
            foreach (Project project in projects)
            {
                var projectNode = root.AddNode($"[bold]{project.NameProject}[/] (ID: {project.IDProject})");
                if (project.Tasks.Count > 0)
                {
                    var taskNode = projectNode.AddNode("[bold purple]Задачи[/]");
                    foreach (Task task in project.Tasks)
                    {
                        taskNode.AddNode($"{task.NameTask} (ID: {task.IDTask}) - {task.StatusTask.ToString()}");

                    }
                }
                else
                {
                    projectNode.AddNode("[red]Нет задач[/]");
                }
            }

            AnsiConsole.Write(root);
        }
        /// <summary>
        /// Визуализация прогресса выполнения проектов по количеству выполненных задач
        /// </summary>
        /// <param name="projects">Лист проектов</param>
        public static void DisplayProjectProgress(List<Project> projects)
        {
            foreach (Project project in projects)
            {
                if (project.Tasks.Count == 0)
                {
                    AnsiConsole.MarkupLine($"[bold grey]Проект: '{project.NameProject}' не содержит задач.[/]");
                    continue;
                }
                int doneTasks = project.Tasks.Count(t => t.StatusTask == Task.TaskStatus.Done);
                double progress = (double)doneTasks / (double)projects.Count;
                AnsiConsole.MarkupLine($"[bold]Проект: {project.NameProject}[/]");
                AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask($"[green]Прогресс выполнения[/]");
                    task.Value = progress * 100; // Устанавливает текущее значение прогресса
                    task.MaxValue = 100;        // Устанавливает максимальное значение (100%)
                });
            }
        }
        /// <summary>
        /// Визуализация Kanban-доски
        /// </summary>
        /// <param name="projects"></param>
        public static void Kanban(List<Project> projects)
        {
            foreach (Project project in projects)
            {
                var table = new Table();
                table.Border = TableBorder.Square;
                table.Title = new TableTitle($"[bold]Проект: {project.NameProject}[/] (ID: {project.IDProject}) ", Style.Parse("blue"));
                table.AddColumn(new TableColumn($"[bold]To do[/]").Centered());
                table.AddColumn(new TableColumn($"[bold]In Progress[/]").Centered());
                table.AddColumn(new TableColumn($"[bold green]Done[/]").Centered());

                if (project.Tasks.Count == 0)
                {
                    table.AddRow(new Markup("[grey]Нет задач[/]").Centered());
                }
                else
                {
                    foreach (Task task in project.Tasks)
                    {
                        if (task.StatusTask == Task.TaskStatus.NotStarted)
                        {
                            table.AddRow($"{task.NameTask}", "", "");
                        }
                        else if (task.StatusTask == Task.TaskStatus.InProgress)
                        {
                            table.AddRow("", $"{task.NameTask}", "");
                        }
                        else if (task.StatusTask == Task.TaskStatus.Done)
                        {
                            table.AddRow("", "", $"{task.NameTask}");
                        }
                    }
                }

                AnsiConsole.Write(table);

            }

        }
    }
}
