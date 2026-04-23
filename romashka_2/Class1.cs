namespace romashka_2
{
    public class TemplateManager
    {
        private List<Template> templates = new()
    {
        new Template { TemplateId = 1, Name = "Contract Template", DocumentType = "contract" },
        new Template { TemplateId = 2, Name = "Application Template", DocumentType = "application" },
        new Template { TemplateId = 3, Name = "Memo Template", DocumentType = "memo" }
    };

        public Template getTemplateByType(string type)
            => templates.FirstOrDefault(t => t.DocumentType == type);
    }

    public class PersistenceService
    {
        private const string FileName = "documents.json";

        public void saveDocuments(List<Document> docs)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(docs, options);
            File.WriteAllText(FileName, json);
        }

        public List<Document> loadDocuments()
        {
            if (!File.Exists(FileName))
                return new List<Document>();

            try
            {
                var json = File.ReadAllText(FileName);

                var rawList = JsonSerializer.Deserialize<List<JsonElement>>(json);

                var result = new List<Document>();

                foreach (var item in rawList)
                {
                    if (!item.TryGetProperty("Type", out var typeProp))
                        continue;

                    string type = typeProp.GetString();

                    Document doc = type switch
                    {
                        "contract" => JsonSerializer.Deserialize<ContractDocument>(item.GetRawText()),
                        "application" => JsonSerializer.Deserialize<ApplicationDocument>(item.GetRawText()),
                        "memo" => JsonSerializer.Deserialize<MemoDocument>(item.GetRawText()),
                        _ => null
                    };

                    if (doc != null)
                        result.Add(doc);
                }

                return result;
            }
            catch
            {
                return new List<Document>();
            }
        }
    }

    public abstract class DocumentCreator
    {
        public abstract Document createDocument();
    }

    public class ContractDocumentCreator : DocumentCreator
    {
        public override Document createDocument() => new ContractDocument();
    }

    public class ApplicationDocumentCreator : DocumentCreator
    {
        public override Document createDocument() => new ApplicationDocument();
    }

    public class MemoDocumentCreator : DocumentCreator
    {
        public override Document createDocument() => new MemoDocument();
    }

    public interface Command
    {
        void execute();
    }

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
