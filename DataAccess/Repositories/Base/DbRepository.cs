using DataAccess.Entities.Base;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        private readonly BookShopContext _db;
        private readonly DbSet<T> _Set;

        public bool AutoSaveChanges { get; set; } = true;

        public DbRepository(BookShopContext db)
        {
            _db = db;
            _Set = db.Set<T>();
        }

        public virtual IQueryable<T> Items => _Set;

        public T Get(int id) => Items.SingleOrDefault(item => item.Id == id);

        public async Task<T> GetAsync(int id, CancellationToken Cancel = default) => await Items
           .SingleOrDefaultAsync(item => item.Id == id, Cancel)
           .ConfigureAwait(false);

        public List<T> Get(Func<T, bool> filter) {
            var results = Items.Where(filter);
            if (results.Count() == 0)
                return null;
            return results.ToList();
        }
        public T GetFirstOrDefault(Func<T, bool> filter)
        {
            var result = Items.FirstOrDefault(filter);
            return result;
        }

        public T Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                _db.SaveChanges();
            return item;
        }

        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }

        public void Update(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                _db.SaveChanges();
        }

        public async Task UpdateAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }

        public void Remove(int id)
        {
            var item = _Set.FirstOrDefault(i => i.Id == id);
            if (item == null) return;
            _Set.Remove(item);

            if (AutoSaveChanges)
                _db.SaveChanges();
        }

        public async Task RemoveAsync(int id, CancellationToken Cancel = default)
        {
            var item = _Set.FirstOrDefault(i => i.Id == id);
            if (item == null) return;
            _Set.Remove(item);

            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }

        public void Remove(Func<T, bool> filter)
        {
            var item = _Set.FirstOrDefault(filter);
            if (item == null) return;
            _Set.Remove(item);

            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        public async Task RemoveAsync(Expression<Func<T, bool>> filter, CancellationToken Cancel = default)
        {
            var item = _Set.FirstOrDefault(filter);
            if (item == null) return;
            _Set.Remove(item);

            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
    }
}
