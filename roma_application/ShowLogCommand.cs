using System;
using System.Collections.Generic;
using System.Text;

namespace roma_application
{
    /// <summary>
    /// Команда отображения истории операций из логгера.
    /// </summary>
    public class ShowLogCommand : Command
    {
        private Logger logger;

        /// <summary>
        /// Создаёт команду показа истории.
        /// </summary>
        /// <param name="l">Логгер, из которого читается история.</param>
        public ShowLogCommand(Logger l)
        {
            logger = l;
        }

        /// <summary>
        /// Выводит содержимое лога в консоль.
        /// </summary>
        public void execute()
        {
            logger.show();
        }
    }
}
