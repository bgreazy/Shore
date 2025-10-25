using SQLite;
using Sitting.Models;

namespace Sitting.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Worker>().Wait();
        }

        public Task<List<Worker>> GetWorkersAsync() => _db.Table<Worker>().ToListAsync();
        public Task<int> SaveWorkerAsync(Worker worker) => _db.InsertOrReplaceAsync(worker);
        public Task<int> DeleteWorkerAsync(Worker worker) => _db.DeleteAsync(worker);
    }
}