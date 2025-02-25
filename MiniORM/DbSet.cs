using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiniORM
{
    public class DbSet<T> : ICollection<T> where T : class, new()
    {
        private readonly IList<T> entities;
        public ChangeTracker<T> ChangeTracker { get; private set; }

        public DbSet(IEnumerable<T> entities)
        {
            this.entities = entities.ToList();
            this.ChangeTracker = new ChangeTracker<T>(this.entities);
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item), "Item cannot be null");

            this.entities.Add(item);
            this.ChangeTracker.Add(item);
        }

        public void Clear()
        {
            while (this.entities.Any())
            {
                var entity = this.entities.First();
                this.Remove(entity);
            }
        }

        public bool Contains(T item) => this.entities.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => this.entities.CopyTo(array, arrayIndex);
        public int Count => this.entities.Count;
        public bool IsReadOnly => this.entities.IsReadOnly;

        public bool Remove(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item), "Item cannot be null");

            bool removed = this.entities.Remove(item);
            if (removed) this.ChangeTracker.Remove(item);

            return removed;
        }

        public IEnumerator<T> GetEnumerator() => this.entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
