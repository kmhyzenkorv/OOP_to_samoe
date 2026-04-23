using romashka_core;
using System.Text.Json;

namespace oma_structure
{
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
}
