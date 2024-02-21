using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp.ExampleExtensions
{
    // Examples of implementation //
    public class SimpleHitObjContainer<T> : HitObjectContainer<T>
    {
        List<T> hitObjs;

        public SimpleHitObjContainer()
        {
            hitObjs = new List<T>();
        }

        public override void AddObject(T obj)
        {
            hitObjs.Add(obj);
        }

        public override void Clear()
        {
            hitObjs.Clear();
        }

        public override bool Contains(T obj)
        {
            return hitObjs.Contains(obj);
        }

        public override void Remove(T obj)
        {
            hitObjs.Remove(obj);
        }
    }

    //Take advantage of Volume Types to track groups that have hit object allowing for multiple hits (once per group)
    // this is tracked using a bit array. The Volume Type is used to set the corresponding bit position to true
    // Be sure to use the overloaded function to utilize this feature
    public class ComplexHitObjContainer<T> : HitObjectContainer<T>
    {
        Dictionary<T, BitArray> hitObjs;

        public ComplexHitObjContainer()
        {
            hitObjs = new Dictionary<T, BitArray>();
        }

        public override void AddObject(T obj)
        {
            if (hitObjs.ContainsKey(obj))
            {
                hitObjs[obj].SetAll(true);
            }
            else
            {
                hitObjs[obj] = new BitArray(Enum.GetNames(typeof(VolumeType)).Length, true);
            }
        }
        public void AddObject(ICombatVolume sender, T obj)
        {
            IComplexVolume complex = sender as IComplexVolume;
            if (complex != null)
            {
                if (hitObjs.ContainsKey(obj))
                {
                    hitObjs[obj].Set((int)complex.VType, true);
                }
                else
                {
                    hitObjs[obj] = new BitArray(Enum.GetNames(typeof(VolumeType)).Length);
                    hitObjs[obj].Set((int)complex.VType, true);
                }
            }
            else
            {
                AddObject(obj);
            }
        }

        public override void Clear()
        {
            hitObjs.Clear();
        }

        public override bool Contains(T obj)
        {
            return hitObjs.ContainsKey(obj);
        }
        public bool Contains(ICombatVolume sender, T obj)
        {
            IComplexVolume complex = sender as IComplexVolume;
            if (complex != null)
            {
                if (hitObjs.ContainsKey(obj))
                {
                    return hitObjs[obj].Get((int)complex.VType);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return Contains(obj);
            }
        }

        public override void Remove(T obj)
        {
            hitObjs.Remove(obj);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<T, BitArray> entry in hitObjs)
            {
                sb.Append("{" + entry.Key.ToString() + "; tag: " + ToBitString(entry.Value) + "}; ");
            }
            return sb.ToString();
        }

        static string ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
    }

}
