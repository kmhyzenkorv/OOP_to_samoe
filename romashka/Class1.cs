using System.Reflection.Metadata;

namespace romashka
{
    public class ContractDocument : Document
    {
        public override string getDocumentType() => "contract";
    }

    public class ApplicationDocument : Document
    {
        public override string getDocumentType() => "application";
    }

    public class MemoDocument : Document
    {
        public override string getDocumentType() => "memo";
    }

    public class Template
    {
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
    }

    public class DocumentManager
    {
        private List<Document> documents = new();
        private int nextId = 1;

        public int generateId()
        {
            return nextId++;
        }

        public void syncIds()
        {
            if (documents.Count == 0)
            {
                nextId = 1;
                return;
            }

            nextId = documents.Max(d => d.Id) + 1;
        }

        public void addDocument(Document doc) => documents.Add(doc);

        public void editDocument(Document doc)
        {
            doc.updateContent();
        }

        public void deleteDocument(Document doc)
        {
            documents.Remove(doc);
        }

        public Document getDocumentById(int id)
            => documents.FirstOrDefault(d => d.Id == id);

        public List<Document> getAllDocuments() => documents;

        public void setDocuments(List<Document> docs)
        {
            documents = docs;
        }
    }
}