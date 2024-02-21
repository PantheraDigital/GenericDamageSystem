using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp
{
    public interface ICombatVolume
    {
        bool Active { get; set; }
        StrikeDispatcher Owner { get; set; }
    }
}
