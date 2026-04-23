using romashka_core;
using System.Collections.Generic;
using System.Linq;

namespace roma_application
{
    public class DocumentManager
    {
        private List<Document> documents = new();
        private int nextId = 1;
        private Logger logger;

        public void setLogger(Logger l)
        {
            logger = l;
        }
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

        public void addDocument(Document doc)
        {
            documents.Add(doc);
            logger?.log($"CREATE | ID={doc.Id} | TYPE={doc.Type}");
        }

        public void editDocument(Document doc)
        {
            doc.updateContent();
        }

        public void deleteDocument(Document doc)
        {
            documents.Remove(doc);
            reindexDocuments();

            logger?.log($"DELETE | ID={doc.Id} | TYPE={doc.Type}");
        }

        public Document getDocumentById(int id)
            => documents.FirstOrDefault(d => d.Id == id);

        public List<Document> getAllDocuments() => documents;

        public void setDocuments(List<Document> docs)
        {
            documents = docs;
        }

        private void reindexDocuments()
        {
            int id = 1;

            foreach (var doc in documents)
            {
                doc.Id = id++;
            }

            nextId = id;
        }

        private void log(string message)
        {
            File.AppendAllText("log.txt",
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}");
        }
    }

public class Logger
    {
        private List<string> logs = new();

        public void log(string message)
        {
            logs.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");
        }

        public void show()
        {
            if (logs.Count == 0)
            {
                Console.WriteLine("История пуста");
                return;
            }

            foreach (var l in logs)
                Console.WriteLine(l);
        }

        public void clear()
        {
            logs.Clear();
        }
    }
}