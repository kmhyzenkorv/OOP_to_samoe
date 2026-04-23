using oma_structure;
using romashka_core;

namespace roma_application
{
    /// <summary>
    /// Команда создания нового документа с помощью переданного создателя (паттерн "Фабричный метод").
    /// </summary>
    public class CreateDocumentCommand : Command
    {
        private DocumentCreator creator;
        private DocumentManager manager;

        /// <summary>
        /// Создаёт команду создания документа.
        /// </summary>
        /// <param name="c">Конкретный создатель документа.</param>
        /// <param name="m">Менеджер документов.</param>
        public CreateDocumentCommand(DocumentCreator c, DocumentManager m)
        {
            creator = c;
            manager = m;
        }

        /// <summary>
        /// Создаёт документ, заполняет его данными и добавляет в менеджер документов.
        /// </summary>
        public void execute()
        {
            var doc = creator.createDocument();

            doc.Type = doc.getDocumentType();
            doc.Id = manager.generateId();

            doc.fillData();
            manager.addDocument(doc);

            Console.WriteLine($"Документ создан. ID: {doc.Id}");
        }
    }
}