using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp
{
    public delegate void OnDamaged(StrikeData strikeData);
    public interface IReceiveStrikes
    {
        event OnDamaged Damaged;
        event OnDamaged DamagedLate;

        StrikeData Apply(StrikeData strikeData);
        //fire Damaged
        //do stuff...
        //fire DamagedLate
        //return adjusted strikeData
    }
}
