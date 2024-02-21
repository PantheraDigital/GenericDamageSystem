using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DamageSysDemo_CSharp.ExampleExtensions;

namespace DamageSysDemo_CSharp
{
    //These are the general objects the Damage system is ment to interact with (GameObjects and Colliders)
    //
    //GameObjects are a base representation of the objects within the game. The base class for things such as characters, entities, dynamic/static objects, and so on.
    //
    //By creating a class such as DamageableObject we can attach it to an object that will react to strikes. This class utilizes the IReceiveStrikes interface
    // and is responsible for handling the data that is passed by a strike. It may either directly execute a reaction, such as reducing health, or 
    // it can indirectly handle reactions by sending the data to appropriate classes on the object like an HP manager or animation manager.
    //
    //Colliders are the volumes used for collision detection. These can be anything that detects other objects, even raycasts.
    // Use the ICombatVolume interface on a class attached to a collider to allow for StrikeDispatcher to activate collision detection as well
    // as allow the collider to send objects back to the StrikeDispatcher. This is how an attacker knows what to send strike data to. 

    public class GameObject
    {
        public string ID { get; set; }
        public GameObject(string id) { ID = id; }
    }
    public class DamageableObject : GameObject, IReceiveStrikes
    {
        float armor = 10f;
        float hp = 100f;

        public event OnDamaged Damaged;
        public event OnDamaged DamagedLate;

        public DamageableObject(string id) : base(id)
        {
        }

        //only damage data is implemented but this is how you could add support for other data types such as force
        // expansion may also warent external data handlers
        public StrikeData Apply(StrikeData strikeData)
        {
            Damaged?.Invoke(strikeData);

            if (strikeData.ID == StrikeDataIDs.DamageData.ToString())
            {
                DamageData damageData = strikeData as DamageData;
                damageData = HandleDamage(damageData);
                DamagedLate?.Invoke(damageData);
                return damageData;
            }
            else if (strikeData.ID == StrikeDataIDs.Composite.ToString())
            {
                StrikeDataComposite composite = strikeData as StrikeDataComposite;
                foreach (StrikeData data in composite.ToReceiver.GetData())
                {
                    if (data.ID == StrikeDataIDs.DamageData.ToString())
                    {
                        DamageData damageData = data as DamageData;
                        HandleDamage(damageData);
                    }
                }

                DamagedLate?.Invoke(composite);
                return composite;
            }

            DamagedLate?.Invoke(strikeData);
            return strikeData;
        }

        public override string ToString()
        {
            return "ID: " + ID + " HP: " + hp + " Armor: " + armor;
        }

        DamageData HandleDamage(DamageData damageData)
        {
            damageData.Damage -= armor;//adjust value of damage due to armor blocking
            if (damageData.Damage < 0f)
                damageData.Damage = 0f;

            hp -= damageData.Damage;

            return damageData;//adjusted damageData is returned
        }
    }


    public class Collider : ICombatVolume
    {
        bool isActive;
        StrikeDispatcher owner;
        public bool Active { get => isActive; set => isActive = value; }
        public StrikeDispatcher Owner { get => owner; set => owner = value; }

        public void CollideWithObject(GameObject obj)
        {
            if (Active)
                owner.StrikeObject(this, obj);
        }
    }
    public class ComplexCollider : IComplexVolume
    {
        VolumeType vType;
        public VolumeType VType => vType;

        bool active;
        public bool Active { get => active; set => active = value; }

        StrikeDispatcher owner;
        public StrikeDispatcher Owner { get => owner; set => owner = value; }

        public ComplexCollider(VolumeType volumeType)
        {
            vType = volumeType;
        }

        public void CollideWithObject(GameObject obj)
        {
            if (Active)
                owner.StrikeObject(this, obj);
        }
    }
}