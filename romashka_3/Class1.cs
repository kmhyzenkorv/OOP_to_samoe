namespace romashka_3
{
    /// <summary>
    /// Команда генерации содержимого документа по связанному шаблону.
    /// </summary>
    public class GenerateDocumentCommand : Command
    {
        private DocumentManager manager;
        private TemplateManager templateManager;

        /// <summary>Создаёт команду генерации документа.</summary>
        public GenerateDocumentCommand(DocumentManager m, TemplateManager t)
        {
            manager = m;
            templateManager = t;
        }

        /// <summary>Находит документ по ID, подбирает шаблон и генерирует содержимое.</summary>
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

    /// <summary>Команда сохранения документов через сервис персистентности.</summary>
    public class SaveDocumentCommand : Command
    {
        private DocumentManager manager;
        private PersistenceService persistence;

        /// <summary>Создаёт команду сохранения документов.</summary>
        public SaveDocumentCommand(DocumentManager m, PersistenceService p)
        {
            manager = m;
            persistence = p;
        }

        /// <summary>Сохраняет все документы и выводит подтверждение.</summary>
        public void execute()
        {
            persistence.saveDocuments(manager.getAllDocuments());
            Console.WriteLine("Данные сохранены");
        }
    }

    /// <summary>Команда отображения списка всех документов.</summary>
    public class OpenDocumentCommand : Command
    {
        private DocumentManager manager;

        /// <summary>Создаёт команду просмотра документов.</summary>
        public OpenDocumentCommand(DocumentManager m)
        {
            manager = m;
        }

        /// <summary>Выводит документы в консоль.</summary>
        public void execute()
        {
            foreach (var d in manager.getAllDocuments())
            {
                Console.WriteLine($"{d.Id} | {d.Type} | {d.Content}");
            }
        }
    }

    /// <summary>Команда удаления документа по ID.</summary>
    public class DeleteDocumentCommand : Command
    {
        private DocumentManager manager;

        /// <summary>Создаёт команду удаления документа.</summary>
        public DeleteDocumentCommand(DocumentManager m)
        {
            manager = m;
        }

        /// <summary>Запрашивает ID и удаляет найденный документ.</summary>
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
    /// Инвокер паттерна "Команда": хранит и запускает текущую команду.
    /// </summary>
    public class Invoker
    {
        private Command command;

        /// <summary>Устанавливает команду для последующего выполнения.</summary>
        public void setCommand(Command cmd)
        {
            command = cmd;
        }

        /// <summary>Выполняет ранее установленную команду.</summary>
        public void execute()
        {
            command.execute();
        }
    }
}
