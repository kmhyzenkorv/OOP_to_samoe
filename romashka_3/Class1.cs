namespace romashka_3
{
    public class GenerateDocumentCommand : Command
    {
        private DocumentManager manager;
        private TemplateManager templateManager;

        public GenerateDocumentCommand(DocumentManager m, TemplateManager t)
        {
            manager = m;
            templateManager = t;
        }

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

    public class SaveDocumentCommand : Command
    {
        private DocumentManager manager;
        private PersistenceService persistence;

        public SaveDocumentCommand(DocumentManager m, PersistenceService p)
        {
            manager = m;
            persistence = p;
        }

        public void execute()
        {
            persistence.saveDocuments(manager.getAllDocuments());
            Console.WriteLine("Данные сохранены");
        }
    }

    public class OpenDocumentCommand : Command
    {
        private DocumentManager manager;

        public OpenDocumentCommand(DocumentManager m)
        {
            manager = m;
        }

        public void execute()
        {
            foreach (var d in manager.getAllDocuments())
            {
                Console.WriteLine($"{d.Id} | {d.Type} | {d.Content}");
            }
        }
    }

    public class DeleteDocumentCommand : Command
    {
        private DocumentManager manager;

        public DeleteDocumentCommand(DocumentManager m)
        {
            manager = m;
        }

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

    public class Invoker
    {
        private Command command;

        public void setCommand(Command cmd)
        {
            command = cmd;
        }

        public void execute()
        {
            command.execute();
        }
    }
}
