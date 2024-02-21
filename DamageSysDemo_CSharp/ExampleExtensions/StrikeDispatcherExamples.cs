using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp.ExampleExtensions
{
    //a bare bones example of how to use the StrikeDispatcher base class to allow an attacker to send strike data to a strikeable object.
    // this is also the intended use of both HitObjectContainer and CombatVolumeContainer to allow the management of already hit objects and volumes.
    class SimpleStrikeDispatcher : StrikeDispatcher
    {
        HitObjectContainer<GameObject> hitObjContainer;
        SimpleVolumeContainer volumeContainer;

        public SimpleStrikeDispatcher(HitObjectContainer<GameObject> hitObjContainer, SimpleVolumeContainer volumeContainer)
        {
            this.hitObjContainer = hitObjContainer;
            this.volumeContainer = volumeContainer;
        }


        override public void StrikeObject(ICombatVolume sender, GameObject obj)
        {
            if (base.OnValidateObjectEvent(this, obj))
            {
                IReceiveStrikes castObj = obj as IReceiveStrikes;
                if (castObj != null && !hitObjContainer.Contains(obj))
                {
                    hitObjContainer.AddObject(obj);
                    base.OnHitObjectEvent(this, obj, castObj.Apply(CurrentStrike.Clone()));//send a clone to avoid changes to base strike data
                }
                else
                    Console.WriteLine("obj duplicate ignored");
            }
        }

        override public void ActivateVolumes()
        {
            volumeContainer.ActivateVolumes();
        }
        override public void DeactivateVolumes()
        {
            volumeContainer.DeactivateVolumes();
            hitObjContainer.Clear();
        }

        override public void RemoveObjectFromHitList(GameObject obj)
        {
            hitObjContainer.Remove(obj);
        }
    }

    //Uses Volume Type to leverage benefits of added functionality in both the expanded HitObjContainer and VolumeContainer
    class CustomStrikeDispatcher : StrikeDispatcher
    {
        ComplexHitObjContainer<GameObject> hitObjContainer;
        ComplexVolumeContainer volumeContainer;

        public CustomStrikeDispatcher(ComplexHitObjContainer<GameObject> hitObjContainer, ComplexVolumeContainer volumeContainer)
        {
            this.hitObjContainer = hitObjContainer;
            this.volumeContainer = volumeContainer;
        }

        public override void StrikeObject(ICombatVolume sender, GameObject obj)
        {
            if (base.OnValidateObjectEvent(this, obj))
            {
                IReceiveStrikes castObj = obj as IReceiveStrikes;
                if (castObj != null && !hitObjContainer.Contains(sender, obj))//be sure to use the expanded version of the function
                {
                    hitObjContainer.AddObject(sender, obj);//be sure to use the expanded version of the function
                    base.OnHitObjectEvent(this, obj, castObj.Apply(CurrentStrike.Clone()));//send a clone to avoid changes to base strike data
                }
                else
                    Console.WriteLine("obj duplicate ignored");
            }
        }

        public override void ActivateVolumes()
        {
            volumeContainer.ActivateVolumes();
        }
        public void ActivateVolumes(VolumeType volumeType)
        {
            volumeContainer.ActivateVolumes(volumeType);
        }

        public override void DeactivateVolumes()
        {
            volumeContainer.DeactivateVolumes();
            hitObjContainer.Clear();
        }
        public void DeactivateVolumes(VolumeType volumeType)
        {
            volumeContainer.DeactivateVolumes(volumeType);

            if (volumeContainer.AllVolumesDeactivated)
                hitObjContainer.Clear();
        }

        public override void RemoveObjectFromHitList(GameObject obj)
        {
            hitObjContainer.Remove(obj);
        }


    }
}
