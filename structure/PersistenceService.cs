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
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Documents.RemoveRange(db.Documents);

                try { db.SaveChanges(); }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(ex.InnerException?.ToString());
                    throw;
                }


                db.Documents.AddRange(docs);

                try { db.SaveChanges(); }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(ex.InnerException?.ToString());
                    throw;
                }

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