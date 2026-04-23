using System.Reflection.Metadata;

namespace romashka
{
    /// <summary>
    /// Документ типа "contract" (договор).
    /// </summary>
    public class ContractDocument : Document
    {
        /// <summary>Возвращает строковый идентификатор типа.</summary>
        public override string getDocumentType() => "contract";
    }

    /// <summary>
    /// Документ типа "application" (заявление).
    /// </summary>
    public class ApplicationDocument : Document
    {
        /// <summary>Возвращает строковый идентификатор типа.</summary>
        public override string getDocumentType() => "application";
    }

    /// <summary>
    /// Документ типа "memo" (служебная записка).
    /// </summary>
    public class MemoDocument : Document
    {
        /// <summary>Возвращает строковый идентификатор типа.</summary>
        public override string getDocumentType() => "memo";
    }

    /// <summary>
    /// Шаблон документа: содержит метаданные (идентификатор, имя, тип и путь к файлу).
    /// </summary>
    public class Template
    {
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Менеджер документов (версия без логгера): хранит коллекцию и управляет ID.
    /// </summary>
    public class DocumentManager
    {
        private List<Document> documents = new();
        private int nextId = 1;

        /// <summary>Генерирует следующий уникальный идентификатор.</summary>
        public int generateId()
        {
            return nextId++;
        }

        /// <summary>Синхронизирует счётчик ID с документами из коллекции.</summary>
        public void syncIds()
        {
            if (documents.Count == 0)
            {
                nextId = 1;
                return;
            }

            nextId = documents.Max(d => d.Id) + 1;
        }

        /// <summary>Добавляет документ в коллекцию.</summary>
        public void addDocument(Document doc) => documents.Add(doc);

        /// <summary>Редактирует содержимое документа через его собственный метод.</summary>
        public void editDocument(Document doc)
        {
            doc.updateContent();
        }

        /// <summary>Удаляет документ из коллекции.</summary>
        public void deleteDocument(Document doc)
        {
            documents.Remove(doc);
        }

        /// <summary>Возвращает документ по идентификатору или null.</summary>
        public Document getDocumentById(int id)
            => documents.FirstOrDefault(d => d.Id == id);

        /// <summary>Возвращает список всех документов.</summary>
        public List<Document> getAllDocuments() => documents;

        /// <summary>Устанавливает коллекцию документов (например, после загрузки).</summary>
        public void setDocuments(List<Document> docs)
        {
            documents = docs;
        }
    }
}