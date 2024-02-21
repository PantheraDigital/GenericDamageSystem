using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp
{
    abstract public class HitObjectContainer<T>
    {
        public abstract bool Contains(T obj);
        public abstract void AddObject(T obj);
        public abstract void Remove(T obj);
        public abstract void Clear();
    }
}
