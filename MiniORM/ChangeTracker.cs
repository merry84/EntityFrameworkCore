using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
    public class ChangeTracker<T> where T : class, new()
    {
        private readonly List<T> allEntities;
        private readonly List<T> added;
        private readonly List<T> removed;

        public IReadOnlyCollection<T> AllEntities => allEntities.AsReadOnly();
        public IReadOnlyCollection<T> Added => added.AsReadOnly();
        public IReadOnlyCollection<T> Removed => removed.AsReadOnly();

        public ChangeTracker(IEnumerable<T> entities)
        {
            this.added = new List<T>();
            this.removed = new List<T>();
            this.allEntities = CloneEntities(entities);
        }

        private static List<T> CloneEntities(IEnumerable<T> entities)
        {
            var clonedEntities = new List<T>();
            var properties = typeof(T)
                .GetProperties()
                .Where(pi => pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string))
                .ToArray();

            foreach (var entity in entities)
            {
                var clonedEntity = Activator.CreateInstance<T>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    property.SetValue(clonedEntity, value);
                }
                clonedEntities.Add(clonedEntity);
            }

            return clonedEntities;
        }

        public void Add(T entity) => this.added.Add(entity);
        public void Remove(T entity) => this.removed.Add(entity);

        public IEnumerable<T> GetModifiedEntities(IEnumerable<T> currentEntities)
        {
            List<T> modifiedEntities = new List<T>();
            PropertyInfo[] primaryKeys = typeof(T)
                .GetProperties()
                .Where(pi => pi.Name.ToLower().EndsWith("id"))
                .ToArray();

            foreach (var proxyEntity in allEntities)
            {
                var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity);
                var realEntity = currentEntities
                    .FirstOrDefault(e => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

                if (IsModified(proxyEntity, realEntity))
                {
                    modifiedEntities.Add(realEntity);
                }
            }

            return modifiedEntities;
        }

        private static bool IsModified(T original, T proxy)
        {
            var properties = typeof(T).GetProperties()
                .Where(pi => pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                var originalValue = property.GetValue(original);
                var proxyValue = property.GetValue(proxy);
                if (!Equals(originalValue, proxyValue))
                {
                    return true;
                }
            }
            return false;
        }

        private static object[] GetPrimaryKeyValues(PropertyInfo[] primaryKeys, T entity)
        {
            return primaryKeys.Select(pk => pk.GetValue(entity)).ToArray();
        }
    }
}
