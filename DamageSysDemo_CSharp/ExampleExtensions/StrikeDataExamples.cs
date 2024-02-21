using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp.ExampleExtensions
{
    //enum allows for easier tracking and compairison of used IDs but custom ones are still allowed in constructors
    public enum StrikeDataIDs
    {
        EmptyData,
        DamageData,
        Composite
    }

    public class EmptyStrikeData : StrikeData
    {
        public EmptyStrikeData(string id) : base(id)
        {
        }
        public EmptyStrikeData(EmptyStrikeData other)
            : base(other)
        {
        }

        public override StrikeData Clone()
        {
            return new EmptyStrikeData(String.Copy(ID));
        }
    }

    public class DamageData : StrikeData
    {
        public float Damage { get; set; }
        public string Type { get; set; }

        public DamageData(string id, float damage, string type)
            : base(id)
        {
            Damage = damage;
            Type = type;
        }
        public DamageData(DamageData other)
            : base(other)
        {
            Damage = other.Damage;
            Type = String.Copy(other.Type);
        }
        public override StrikeData Clone()
        {
            return new DamageData(String.Copy(ID), Damage, String.Copy(Type));
        }

        public override string ToString()
        {
            return "ID: " + ID + "; damage: " + Damage + "; type: " + Type;
        }
    }
}
