using romashka_core;
using oma_structure.Data;

namespace oma_structure
{
    public class PersistenceService
    {
        public void saveDocuments(List<Document> docs)
        {
            using (AppDbContext db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                db.Documents.RemoveRange(db.Documents);

                db.SaveChanges();

                db.Documents.AddRange(docs);

                db.SaveChanges();
            }
        }

        public List<Document> loadDocuments()
        {
            using (AppDbContext db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                return db.Documents.ToList();
            }
        }
    }
}