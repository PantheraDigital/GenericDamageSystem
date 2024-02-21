using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DamageSysDemo_CSharp.ExampleExtensions;

namespace DamageSysDemo_CSharp
{
    //see TestObjects.cs for objects used in SystemTest and ComplexTest
    //see EcampleExtensions folder for examples on how to extend the system for more versatility and different uses. these are used in ComplexTest and SystemTest.
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            do
            {
                Console.WriteLine("Options:\n\t" +
                    "Q - quit\n\t" +
                    "1 - Run StrikeData Test\n\t" +
                    "2 - Run System Test\n\t" +
                    "3 - Run Complex Test\n");
                input = Console.ReadKey(true);

                switch (input.KeyChar)
                {
                    case 'q':
                    case 'Q':
                        Console.Write("Press <Enter> to exit... ");
                        break;
                    case '1':
                        StrikeDataTest();
                        Console.WriteLine("\n");
                        break;
                    case '2':
                        SystemTest();
                        Console.WriteLine("\n");
                        break;
                    case '3':
                        ComplexTest();
                        Console.WriteLine("\n");
                        break;
                    default:
                        Console.WriteLine("...Input Not Recognized...");
                        break;
                }
                
            } while (input.KeyChar != 'q' && input.KeyChar != 'Q');

            
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }


        public static void StrikeDataTest()
        {
            StrikeData[] strikeDatas =
            {
                new DamageData("damage", 10, "fire"),
                new DamageData("damage", 3, "slash"),
                new EmptyStrikeData("complex damage"),
                new EmptyStrikeData("complex damage"),
                new EmptyStrikeData("status"),
                new EmptyStrikeData("crit chance"),
                new EmptyStrikeData("meta data"),
                new EmptyStrikeData("random damage"),
                new EmptyStrikeData("force")
            };
            StrikeData[] strikeDatas2 =
            {
                new EmptyStrikeData("boost"),
                new EmptyStrikeData("status"),
                new EmptyStrikeData("meta data"),
                new EmptyStrikeData("random damage"),
                new EmptyStrikeData("force")
            };

            StrikeDataComposite complexAttack = new StrikeDataComposite("complex attack");
            foreach (StrikeData data in strikeDatas)
            {
                complexAttack.ToReceiver.AddData(data);
            }
            foreach (StrikeData data in strikeDatas2)
            {
                complexAttack.ToAttacker.AddData(data);
            }

            Func<string, string> GetDataTest = id => {
                StrikeData temp = complexAttack.ToReceiver.GetData(id);
                return (temp == null) ? "Null" : temp.ToString();
            };
            Func<string, string> SubcompositeTest = id => {
                StrikeDataComposite.Container temp = complexAttack.ToReceiver.SubComposite(id);
                return (temp == null) ? "Null" : temp.ToString();
            };
            Func<string[], string> MultiSubcompositeTest = ids => {
                StrikeDataComposite.Container temp = complexAttack.ToReceiver.SubComposite(ids);
                return (temp == null) ? "Null" : temp.ToString();
            };
            Func<string, string> SubcompositeExcludeTest = id => {
                StrikeDataComposite.Container temp = complexAttack.ToReceiver.SubCompositeExclude(id);
                return (temp == null) ? "Null" : temp.ToString();
            };
            Func<StrikeData, string> AddDataTest = (data) => {
                complexAttack.ToReceiver.AddData(data);
                return complexAttack.ToString();
            };
            Func<string> Clear = () => {
                complexAttack.ToReceiver.Clear();
                complexAttack.ToAttacker.Clear();
                return complexAttack.ToString();
            };

            Func<StrikeData[], string> StrikeArrayToString = array => {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < array.Length; ++i)
                {
                    sb.Append("\t" + array[i].ToString());
                    if (i + 1 < array.Length)
                        sb.Append("\n");
                }
                return sb.ToString();
            };

            Dictionary<string, string> results = new Dictionary<string, string>()
            {
                {"Data 1:", StrikeArrayToString(strikeDatas)},
                {"Data 2:", StrikeArrayToString(strikeDatas2)},
                {"Init StrikeDataComposite", complexAttack.ToString()},
                {"ToReceiver HasData : \"status\"", complexAttack.ToReceiver.HasData("status").ToString()},
                {"ToReceiver HasData : \"boost\"", complexAttack.ToReceiver.HasData("boost").ToString()},
                {"ToReceiver GetData : \"damage\"", GetDataTest("damage")},
                {"ToReceiver GetData : \"boost\"", GetDataTest("boost")},
                {"ToReceiver SubComposite : \"damage\"", SubcompositeTest("damage")},
                {"ToReceiver SubComposite : \"boost\"", SubcompositeTest("boost")},
                {"ToReceiver SubComposite : \"damage\", \"boost\", \"status\"", MultiSubcompositeTest(new string[]{"damage" , "boost", "status"})},
                {"ToReceiver SubCompositeExclude : \"damage\"", SubcompositeExcludeTest("damage")},
                {"ToReceiver SubCompositeExclude : \"boost\"", SubcompositeExcludeTest("boost")},
                {"ToReceiver AddData : \"boost\"",  AddDataTest(strikeDatas2[0])},
                {"CLEAR", Clear()}
            };
            foreach (KeyValuePair<string, string> entry in results)
            {
                Console.WriteLine(entry.Key + "\n" + entry.Value + "\n\n");
            }
        }


        public static void DamagedEvent(StrikeData strikeData)
        {
            Console.WriteLine($"damageable object  {"OnDamaged:",15} strike data - {{{strikeData.ToString()}}}\n");
        }
        public static void DamagedEventLate(StrikeData strikeData)
        {
            Console.WriteLine($"damageable object  {"OnDamagedLate:",15} adjusted strike data - {{{strikeData.ToString()}}}\n");
        }

        public static void SystemTest()
        {
            //create the attacker
            StrikeData strikeData = new DamageData(StrikeDataIDs.DamageData.ToString(), 20f, "slash");

            Collider collider = new Collider();
            SimpleVolumeContainer colliderContainer = new SimpleVolumeContainer(new[] {collider});
            SimpleHitObjContainer<GameObject> hitObjContainer = new SimpleHitObjContainer<GameObject>();
            SimpleStrikeDispatcher strikeDispatcher = new SimpleStrikeDispatcher(hitObjContainer, colliderContainer);
            collider.Owner = strikeDispatcher;

            //create targets
            DamageableObject damageableObject = new DamageableObject("target");
            DamageableObject damageableObject2 = new DamageableObject("target2");
            Console.WriteLine(damageableObject.ToString() + "\n");

            //utilize events
            strikeDispatcher.ValidateObject += (StrikeDispatcher sender, GameObject objDetected) => {
                Console.WriteLine($"strike dispatcher  {"OnDetectObj:", 15} obj - {{ID: {objDetected.ID}}}\n");
                if (objDetected.ID == "target2")//obj filter logic
                {
                    objDetected = null;
                    Console.WriteLine("friendly object ignored");
                    return false;
                }
                else if (objDetected.ID == "target")
                {
                    strikeDispatcher.RemoveObjectFromHitList(objDetected);
                }
                return true;
            };
            strikeDispatcher.HitObject += (StrikeDispatcher sender, GameObject objHit, StrikeData sd) => {
                Console.WriteLine($"strike dispatcher  {"OnHitObj:", 15} obj - {{{objHit.ID}}}; strikeData - {{{sd.ToString()}}}\n");
            };

            damageableObject.Damaged += DamagedEvent;
            damageableObject.DamagedLate += DamagedEventLate;

            damageableObject2.Damaged += DamagedEvent;
            damageableObject2.DamagedLate += DamagedEventLate;


            // attack begins, set data and activate colliders
            strikeDispatcher.CurrentStrike = strikeData;
            strikeDispatcher.ActivateVolumes();

            Console.WriteLine("---Test 1: 2 objects, 1 friendly---\n");
            //object detected by collider
            collider.CollideWithObject(damageableObject);
            collider.CollideWithObject(damageableObject);//object hit twice, ignore second
            Console.WriteLine("\n" + damageableObject.ToString());

            Console.WriteLine("\n\n" + damageableObject2.ToString() + "\n");
            collider.CollideWithObject(damageableObject2);
            Console.WriteLine("\n" + damageableObject2.ToString());

            //end attack
            strikeDispatcher.DeactivateVolumes();

            ////// new attack //////
            StrikeData[] strikeDatas =
            {
                new DamageData(StrikeDataIDs.DamageData.ToString(), 20, "fire"),
                new DamageData(StrikeDataIDs.DamageData.ToString(), 3, "slash")
            };
            StrikeDataComposite complexAttack = new StrikeDataComposite(StrikeDataIDs.Composite.ToString());
            foreach (StrikeData data in strikeDatas)
            {
                complexAttack.ToReceiver.AddData(data);
            }

            // attack begins, set data and activate colliders
            strikeDispatcher.CurrentStrike = complexAttack;
            strikeDispatcher.ActivateVolumes();


            Console.WriteLine("\n\n---Test 2: 1 object, strike data composite, multi attack---\n");
            //object detected by collider
            collider.CollideWithObject(damageableObject);
            collider.CollideWithObject(damageableObject);
            Console.WriteLine("\n" + damageableObject.ToString());

            //end attack
            strikeDispatcher.DeactivateVolumes();
        }

        public static void ComplexTest()
        {
            //create the attacker
            StrikeData[] strikeDatas =
            {
                new DamageData(StrikeDataIDs.DamageData.ToString(), 20, "fire"),
                new DamageData(StrikeDataIDs.DamageData.ToString(), 3, "slash")
            };
            StrikeDataComposite complexAttack = new StrikeDataComposite(StrikeDataIDs.Composite.ToString());
            foreach (StrikeData data in strikeDatas)
            {
                complexAttack.ToReceiver.AddData(data);
            }

            ComplexCollider collider1 = new ComplexCollider(VolumeType.HurtboxAlpha);
            ComplexCollider collider2 = new ComplexCollider(VolumeType.HurtboxBeta);
            ComplexVolumeContainer colliderContainer = new ComplexVolumeContainer(new[] { collider1, collider2 });

            ComplexHitObjContainer<GameObject> hitObjContainer = new ComplexHitObjContainer<GameObject>();

            CustomStrikeDispatcher strikeDispatcher = new CustomStrikeDispatcher(hitObjContainer, colliderContainer);
            strikeDispatcher.CurrentStrike = complexAttack;
            collider1.Owner = strikeDispatcher;
            collider2.Owner = strikeDispatcher;


            //create targets
            DamageableObject damageableObject = new DamageableObject("target");


            Console.WriteLine("---Inactive Collider Test---");
            strikeDispatcher.ActivateVolumes(VolumeType.Pushbox);
            collider1.CollideWithObject(damageableObject);
            collider2.CollideWithObject(damageableObject);
            strikeDispatcher.DeactivateVolumes(VolumeType.Pushbox);
            Console.WriteLine("No text should appear between this line and the test indicator line as the inactive colliders were used.\n\n\n");


            Console.WriteLine("---HurtboxAlpha Active, 2 hits per collider---\n" +
                "Expect: 1 duplicate; 10dmg to obj; Hit Obj Container has target with tag 100\n");
            strikeDispatcher.ActivateVolumes(VolumeType.HurtboxAlpha);
            collider1.CollideWithObject(damageableObject);
            collider1.CollideWithObject(damageableObject);
            collider2.CollideWithObject(damageableObject);//These colliders will do nothing as they are inactive
            collider2.CollideWithObject(damageableObject);

            Console.WriteLine("\nHit Object Container: " + hitObjContainer.ToString());
            Console.WriteLine("\n" + damageableObject.ToString());
            strikeDispatcher.DeactivateVolumes();


            Console.WriteLine("\n\n---Both Volume Types Active, 2 hits each---\n" +
                "Expect: 2 duplicates; 20 more dmg applied to obj; Hit Obj Container has target with tag 110\n");
            strikeDispatcher.ActivateVolumes();//activate all volumes
            collider1.CollideWithObject(damageableObject);
            collider2.CollideWithObject(damageableObject);
            collider1.CollideWithObject(damageableObject);
            collider2.CollideWithObject(damageableObject);

            Console.WriteLine("\nHit Object Container: " + hitObjContainer.ToString());
            Console.WriteLine("\n" + damageableObject.ToString());
            strikeDispatcher.DeactivateVolumes();//deactivate all volumes

            Console.WriteLine("\nHit Object Container Cleared" + hitObjContainer.ToString());

        }
    }
}
