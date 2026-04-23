using oma_structure;
using romashka_core;

namespace roma_application
{
    public class CreateDocumentCommand : Command
    {
        private DocumentCreator creator;
        private DocumentManager manager;

        public CreateDocumentCommand(DocumentCreator c, DocumentManager m)
        {
            creator = c;
            manager = m;
        }

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