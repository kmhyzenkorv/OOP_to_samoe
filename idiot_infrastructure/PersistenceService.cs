using romashka_core;
using System.Text.Json;

namespace structure
{
    /// <summary>
    /// Сервис для сохранения и загрузки документов в/из JSON-файла.
    /// </summary>
    public class PersistenceService
    {
        private const string FileName = "documents.json";

        /// <summary>
        /// Сохраняет список документов в JSON-файл с форматированием.
        /// </summary>
        /// <param name="docs">Список документов для сохранения.</param>
        public void saveDocuments(List<Document> docs)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(docs, options);
            File.WriteAllText(FileName, json);
        }

        /// <summary>
        /// Загружает документы из JSON-файла и восстанавливает конкретные типы документов.
        /// </summary>
        /// <returns>Список документов или пустой список, если файл отсутствует или повреждён.</returns>
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
}