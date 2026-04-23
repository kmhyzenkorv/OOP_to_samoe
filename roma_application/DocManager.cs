using romashka_core;
using System.Collections.Generic;
using System.Linq;

namespace roma_application
{
    /// <summary>
    /// Менеджер документов: хранит коллекцию документов, управляет идентификаторами и логированием операций.
    /// </summary>
    public class DocumentManager
    {
        private List<Document> documents = new();
        private int nextId = 1;
        private Logger logger;

        /// <summary>
        /// Устанавливает логгер для записи операций над документами.
        /// </summary>
        /// <param name="l">Экземпляр логгера.</param>
        public void setLogger(Logger l)
        {
            logger = l;
        }

        /// <summary>
        /// Генерирует очередной уникальный идентификатор документа.
        /// </summary>
        public int generateId()
        {
            return nextId++;
        }

        /// <summary>
        /// Синхронизирует счётчик ID с уже загруженными документами.
        /// </summary>
        public void syncIds()
        {
            if (documents.Count == 0)
            {
                nextId = 1;
                return;
            }

            nextId = documents.Max(d => d.Id) + 1;
        }

        /// <summary>
        /// Добавляет документ в коллекцию и записывает операцию в лог.
        /// </summary>
        /// <param name="doc">Документ для добавления.</param>
        public void addDocument(Document doc)
        {
            documents.Add(doc);
            logger?.log($"CREATE | ID={doc.Id} | TYPE={doc.Type}");
        }

        /// <summary>
        /// Редактирует содержимое документа через его собственный метод обновления.
        /// </summary>
        /// <param name="doc">Документ для редактирования.</param>
        public void editDocument(Document doc)
        {
            doc.updateContent();
        }

        /// <summary>
        /// Удаляет документ из коллекции, выполняет переиндексацию и пишет лог.
        /// </summary>
        /// <param name="doc">Документ для удаления.</param>
        public void deleteDocument(Document doc)
        {
            documents.Remove(doc);
            reindexDocuments();

            logger?.log($"DELETE | ID={doc.Id} | TYPE={doc.Type}");
        }

        /// <summary>
        /// Возвращает документ по идентификатору или null, если он отсутствует.
        /// </summary>
        /// <param name="id">Идентификатор документа.</param>
        public Document getDocumentById(int id)
            => documents.FirstOrDefault(d => d.Id == id);

        /// <summary>
        /// Возвращает список всех документов.
        /// </summary>
        public List<Document> getAllDocuments() => documents;

        /// <summary>
        /// Задаёт коллекцию документов (например, после загрузки из файла).
        /// </summary>
        /// <param name="docs">Загруженный список документов.</param>
        public void setDocuments(List<Document> docs)
        {
            documents = docs;
        }

        /// <summary>
        /// Переназначает идентификаторы документов последовательно начиная с 1.
        /// </summary>
        private void reindexDocuments()
        {
            int id = 1;

            foreach (var doc in documents)
            {
                doc.Id = id++;
            }

            nextId = id;
        }

        /// <summary>
        /// Дописывает строку сообщения в файл log.txt с отметкой времени.
        /// </summary>
        /// <param name="message">Сообщение для записи.</param>
        private void log(string message)
        {
            File.AppendAllText("log.txt",
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}");
        }
    }

    /// <summary>
    /// Простой in-memory логгер: хранит историю сообщений и умеет выводить её в консоль.
    /// </summary>
    public class Logger
    {
        private List<string> logs = new();

        /// <summary>
        /// Добавляет сообщение в историю с текущей меткой времени.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void log(string message)
        {
            logs.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");
        }

        /// <summary>
        /// Выводит всю накопленную историю в консоль.
        /// </summary>
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

        /// <summary>
        /// Очищает накопленную историю сообщений.
        /// </summary>
        public void clear()
        {
            logs.Clear();
        }
    }
}