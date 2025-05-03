using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Persistance
{
    public interface IRepo<ID, E> where E : Entity<ID>
    {
        E? FindOne(ID id);


        List<E> FindAll();


        E? Save(E entity);


        E? Delete(ID id);


        E? Update(E entity);
    }

    public class Optional<T>
    {
        public T Value { get; }
        public bool HasValue { get; }

        private Optional(T value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
        }

        public static Optional<T> Of(T value) => new Optional<T>(value, true);
        public static Optional<T> Empty() => new Optional<T>(default, false);
    }
}
