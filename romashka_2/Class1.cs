namespace romashka_2
{
    /// <summary>
    /// Менеджер шаблонов: хранит предопределённые шаблоны документов и предоставляет поиск по типу.
    /// </summary>
    public class TemplateManager
    {
        private List<Template> templates = new()
    {
        new Template { TemplateId = 1, Name = "Contract Template", DocumentType = "contract" },
        new Template { TemplateId = 2, Name = "Application Template", DocumentType = "application" },
        new Template { TemplateId = 3, Name = "Memo Template", DocumentType = "memo" }
    };

        /// <summary>Возвращает шаблон по типу документа или null.</summary>
        public Template getTemplateByType(string type)
            => templates.FirstOrDefault(t => t.DocumentType == type);
    }

    /// <summary>
    /// Сервис сохранения/загрузки документов в JSON-файл.
    /// </summary>
    public class PersistenceService
    {
        private const string FileName = "documents.json";

        /// <summary>Сохраняет документы в JSON с форматированием.</summary>
        public void saveDocuments(List<Document> docs)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(docs, options);
            File.WriteAllText(FileName, json);
        }

        /// <summary>Загружает документы из JSON-файла, восстанавливая их конкретные типы.</summary>
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

    /// <summary>Абстрактный создатель документов (паттерн "Фабричный метод").</summary>
    public abstract class DocumentCreator
    {
        /// <summary>Создаёт экземпляр конкретного документа.</summary>
        public abstract Document createDocument();
    }

    /// <summary>Создатель документов-контрактов.</summary>
    public class ContractDocumentCreator : DocumentCreator
    {
        /// <summary>Создаёт новый ContractDocument.</summary>
        public override Document createDocument() => new ContractDocument();
    }

    /// <summary>Создатель документов-заявлений.</summary>
    public class ApplicationDocumentCreator : DocumentCreator
    {
        /// <summary>Создаёт новый ApplicationDocument.</summary>
        public override Document createDocument() => new ApplicationDocument();
    }

    /// <summary>Создатель документов-служебных записок.</summary>
    public class MemoDocumentCreator : DocumentCreator
    {
        /// <summary>Создаёт новый MemoDocument.</summary>
        public override Document createDocument() => new MemoDocument();
    }

    /// <summary>Общий интерфейс команды (паттерн "Команда").</summary>
    public interface Command
    {
        /// <summary>Выполняет действие, связанное с командой.</summary>
        void execute();
    }

    /// <summary>Команда создания и заполнения нового документа.</summary>
    public class CreateDocumentCommand : Command
    {
        private DocumentCreator creator;
        private DocumentManager manager;

        /// <summary>Создаёт команду создания документа.</summary>
        public CreateDocumentCommand(DocumentCreator c, DocumentManager m)
        {
            creator = c;
            manager = m;
        }

        /// <summary>Создаёт документ через фабрику, заполняет его и сохраняет в менеджере.</summary>
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
