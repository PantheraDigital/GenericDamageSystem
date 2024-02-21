using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp.ExampleExtensions
{
    // Examples of implementation //
    public class SimpleVolumeContainer : CombatVolumeContainer
    {
        List<ICombatVolume> combatVolumes;

        public SimpleVolumeContainer(ICombatVolume[] combatVolumes)
        {
            this.combatVolumes = new List<ICombatVolume>(combatVolumes);
        }

        override public void ActivateVolumes()
        {
            foreach (ICombatVolume item in combatVolumes)
            {
                item.Active = true;
            }
        }
        override public void DeactivateVolumes()
        {
            foreach (ICombatVolume item in combatVolumes)
            {
                item.Active = false;
            }
        }
    }

    //Utilize Volume Types to activate and deactivate groups of volumes seperately
    public class ComplexVolumeContainer : CombatVolumeContainer
    {
        List<IComplexVolume> combatVolumes;

        public bool AllVolumesDeactivated { get; protected set; }

        public ComplexVolumeContainer(IComplexVolume[] combatVolumes)
        {
            this.combatVolumes = new List<IComplexVolume>(combatVolumes);
        }

        override public void ActivateVolumes()
        {
            foreach (IComplexVolume item in combatVolumes)
            {
                item.Active = true;
            }
        }
        public void ActivateVolumes(VolumeType volumeType)
        {
            foreach (IComplexVolume item in combatVolumes)
            {
                if (item.VType == volumeType)
                    item.Active = true;
            }
        }

        override public void DeactivateVolumes()
        {
            foreach (IComplexVolume item in combatVolumes)
            {
                item.Active = false;
                AllVolumesDeactivated = true;
            }
        }
        public void DeactivateVolumes(VolumeType volumeType)
        {
            AllVolumesDeactivated = true;
            foreach (IComplexVolume item in combatVolumes)
            {
                if (item.VType == volumeType)
                    item.Active = false;

                if (item.Active == true)
                    AllVolumesDeactivated = false;
            }
        }
    }
}
