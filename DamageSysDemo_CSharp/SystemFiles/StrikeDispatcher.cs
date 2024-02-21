using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageSysDemo_CSharp
{
    //replace GameObject with game engine equivlent
    abstract public class StrikeDispatcher
    {
        public delegate bool OnValidateObject<T>(StrikeDispatcher sender, T objDetected);
        public delegate void OnHitObject<T>(StrikeDispatcher sender, T objHit, StrikeData strikeData);
        public event OnValidateObject<GameObject> ValidateObject;
        public event OnHitObject<GameObject> HitObject;

        public StrikeData CurrentStrike { get; set; }

        protected bool OnValidateObjectEvent(StrikeDispatcher sender, GameObject objDetected)
        {
            if (ValidateObject != null)
                return (bool)ValidateObject.Invoke(sender, objDetected);
            else
                return true;
        }
        protected void OnHitObjectEvent(StrikeDispatcher sender, GameObject objHit, StrikeData strikeData)
        {
            HitObject?.Invoke(sender, objHit, strikeData);
        }

        abstract public void StrikeObject(ICombatVolume sender, GameObject obj); //used by CombatVolumes to send hit objects
        abstract public void ActivateVolumes();
        abstract public void DeactivateVolumes();
        abstract public void RemoveObjectFromHitList(GameObject obj);
    }
}
