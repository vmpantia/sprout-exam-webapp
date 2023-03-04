using Microsoft.EntityFrameworkCore;
using Sprout.Exam.WebApp.Contractors;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _db;
        private readonly DbSet<T> _table;
        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            _table = db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await _table.ToListAsync();
            if (result == null)
                throw new ArgumentNullException("No records found in the system.");

            return result;
        }

        public async Task<T> GetById(object id)
        {
            var result = await _table.FindAsync(id);
            if (result == null)
                throw new ArgumentNullException("No record found in the system.");

            return result;
        }

        public async Task Add(T t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            await _table.AddAsync(t);
            var result = await _db.SaveChangesAsync();

            if (result <= 0)
                throw new Exception("Error in creating data.");
        }

        public async Task Update(object id, object model)
        {
            var data = await GetById(id);
            if (data != null)
            {
                _db.Entry(data).CurrentValues.SetValues(model);
                var result = await _db.SaveChangesAsync();

                if (result <= 0)
                    throw new Exception("Error in updating data.");
            }
        }

        public async Task Delete(object id)
        {
            var data = await GetById(id);
            if (data != null)
            {
                _table.Remove(data);
                var result = await _db.SaveChangesAsync();

                if (result <= 0)
                    throw new Exception("Error in deleting data.");
            }
        }
    }
}
