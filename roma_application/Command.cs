using oma_structure;

namespace roma_application
{
    /// <summary>
    /// Общий интерфейс команды в паттерне "Команда".
    /// </summary>
    public interface Command
    {
        /// <summary>
        /// Выполняет действие, связанное с командой.
        /// </summary>
        void execute();
    }

    /// <summary>
    /// Команда генерации содержимого документа по шаблону, соответствующему его типу.
    /// </summary>
    public class GenerateDocumentCommand : Command
    {
        private DocumentManager manager;
        private TemplateManager templateManager;

        /// <summary>
        /// Создаёт команду генерации документа.
        /// </summary>
        /// <param name="m">Менеджер документов.</param>
        /// <param name="t">Менеджер шаблонов.</param>
        public GenerateDocumentCommand(DocumentManager m, TemplateManager t)
        {
            manager = m;
            templateManager = t;
        }

        /// <summary>
        /// Запрашивает ID документа и генерирует его содержимое по соответствующему шаблону.
        /// </summary>
        public void execute()
        {
            Console.Write("ID документа: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID");
                return;
            }

            var doc = manager.getDocumentById(id);
            if (doc == null)
            {
                Console.WriteLine("Документ не найден");
                return;
            }

            var template = templateManager.getTemplateByType(doc.Type);

            if (template == null)
            {
                Console.WriteLine("Шаблон не найден");
                return;
            }

            doc.generate(template);
            Console.WriteLine("Документ сгенерирован");
        }
    }

    /// <summary>
    /// Команда сохранения всех документов через сервис персистентности.
    /// </summary>
    public class SaveDocumentCommand : Command
    {
        private DocumentManager manager;
        private PersistenceService persistence;

        /// <summary>
        /// Создаёт команду сохранения документов.
        /// </summary>
        /// <param name="m">Менеджер документов.</param>
        /// <param name="p">Сервис персистентности.</param>
        public SaveDocumentCommand(DocumentManager m, PersistenceService p)
        {
            manager = m;
            persistence = p;
        }

        /// <summary>
        /// Сохраняет все документы в файл.
        /// </summary>
        public void execute()
        {
            persistence.saveDocuments(manager.getAllDocuments());
            Console.WriteLine("Данные сохранены");
        }
    }

    /// <summary>
    /// Команда вывода списка всех документов в консоль.
    /// </summary>
    public class OpenDocumentCommand : Command
    {
        private DocumentManager manager;

        /// <summary>
        /// Создаёт команду просмотра документов.
        /// </summary>
        /// <param name="m">Менеджер документов.</param>
        public OpenDocumentCommand(DocumentManager m)
        {
            manager = m;
        }

        /// <summary>
        /// Выводит все документы в формате "ID | тип | содержимое".
        /// </summary>
        public void execute()
        {
            foreach (var d in manager.getAllDocuments())
            {
                Console.WriteLine($"{d.Id} | {d.Type} | {d.Content}");
            }
        }
    }

    /// <summary>
    /// Команда удаления документа по ID.
    /// </summary>
    public class DeleteDocumentCommand : Command
    {
        private DocumentManager manager;

        /// <summary>
        /// Создаёт команду удаления документа.
        /// </summary>
        /// <param name="m">Менеджер документов.</param>
        public DeleteDocumentCommand(DocumentManager m)
        {
            manager = m;
        }

        /// <summary>
        /// Запрашивает ID и удаляет соответствующий документ из списка.
        /// </summary>
        public void execute()
        {
            Console.Write("ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID");
                return;
            }

            var doc = manager.getDocumentById(id);

            if (doc != null)
            {
                manager.deleteDocument(doc);
                Console.WriteLine("Удалено");
            }
            else
            {
                Console.WriteLine("Документ не найден");
            }
        }
    }

    /// <summary>
    /// Инвокер паттерна "Команда": хранит текущую команду и запускает её на выполнение.
    /// </summary>
    public class Invoker
    {
        private Command command;

        /// <summary>
        /// Устанавливает текущую команду для последующего выполнения.
        /// </summary>
        /// <param name="cmd">Команда для установки.</param>
        public void setCommand(Command cmd)
        {
            command = cmd;
        }

        /// <summary>
        /// Выполняет ранее установленную команду.
        /// </summary>
        public void execute()
        {
            command.execute();
        }
    }
}