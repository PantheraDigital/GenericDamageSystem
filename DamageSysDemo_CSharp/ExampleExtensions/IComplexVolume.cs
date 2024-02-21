using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp.ExampleExtensions
{
    //an extension that adds a type to combat volumes for more specific activation
    // this can be used in many ways depending on system needs such as filtering data sent to hit object or marking which volumes have detected an object.
    public enum VolumeType
    {
        HurtboxAlpha,
        HurtboxBeta,
        Pushbox
    }
    public interface IComplexVolume : ICombatVolume
    {
        VolumeType VType { get; }
    }
}
