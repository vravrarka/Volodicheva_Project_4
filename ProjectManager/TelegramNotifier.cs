using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace ProjectManager
{
    /// <summary>
    /// Класс для отправки работы с ТГ-ботом 
    /// </summary>
    public static class TelegramNotifier
    {
        /// <summary>
        ///  Обрабатывает входящие обновления от ТГ-бота.
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="update">Входящее обновление в чате</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции</param>
        /// <param name="projects">Лист проектов</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        public static async System.Threading.Tasks.Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, List<Project> projects)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text?.ToLower() == "/start")
                {
                    // Отправка уведомлений о просроченных задачах и задачах с истекающим сроком
                    await botClient.SendTextMessageAsync(message.Chat, Notification.NoticeToTelegramOverdue(projects), cancellationToken: cancellationToken);
                    await botClient.SendTextMessageAsync(message.Chat, Notification.NoticeToTelegramdeadlineEnds(projects), cancellationToken: cancellationToken);
                    return;
                }
                // Если неверное сообщение в боте
                await botClient.SendTextMessageAsync(message.Chat, "Начни диалог со /start", cancellationToken: cancellationToken);
            }
        }
        /// <summary>
        /// Обрабатывает ошибки, возникающие в Telegram-боте
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="exception">Исключение, которое произошло</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        public static System.Threading.Tasks.Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка в Telegram-боте: {exception.Message}");
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }


}
