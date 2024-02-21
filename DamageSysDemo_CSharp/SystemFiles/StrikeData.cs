using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp
{
    public abstract class StrikeData : IEquatable<StrikeData>, IComparable<StrikeData>
    {
        public string ID { get; private set; }

        public StrikeData(string id)
        {
            this.ID = id;
        }
        //Deep copy constructors used when editing data in IReceiveStrikes objects before sending back to StrikeDispatcher
        // StrikeData is class and not struct for inheritance
        // Interface plus struct not used due to boxing as all forms of StrikeData need to be stored in StrikeDataComposite 
        public StrikeData(StrikeData other)
        {
            this.ID = String.Copy(other.ID);
        }
        public abstract StrikeData Clone();//return a deep copy

        public override string ToString()
        {
            return ID;
        }

        public bool Equals(StrikeData other)
        {
            if (other == null) return false;
            return ID.Equals(other.ID);
        }

        public int CompareTo(StrikeData other)
        {
            if (other == null)
                return 1;
            else
                return ID.CompareTo(other.ID);
        }
    }
}
