using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Tyrant
    {
        
        public static Type[] Artifacts = new Type[]
        {
			typeof( AlchemistsBaubleplus ), typeof( GwennosHarpplus ), 
			typeof( ArcticDeathDealerplus ), typeof( PeasantsBokutoplus ),
			typeof( EyesOfHateplus ), typeof( PolarBearMaskplus ),
			typeof( EverlastingBandage )
        };
        public static int Hue = 0x780;// Tyrant hue

        // Buffs
        public static double HitsBuff = 25.0;
        public static double StrBuff = 3.00;
        public static double IntBuff = 3.00;
        public static double DexBuff = 3.00;
        public static double SkillsBuff = 2.00;
        public static double SpeedBuff = 1.80;
        public static double FameBuff = 1.00;
        public static double KarmaBuff = 1.00;
        public static int DamageBuff = 25;
        public static void ConvertTyrant(BaseCreature bc)
        {
            if (bc.IsParagon || bc.IsTyrant || !bc.CanBeTyrant ||
				!bc.CanBeParagon)
                return;

            bc.Hue = Hue;

            if (bc.HitsMaxSeed >= 0)
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed * HitsBuff);

            bc.RawStr = (int)(bc.RawStr * StrBuff);
            bc.RawInt = (int)(bc.RawInt * IntBuff);
            bc.RawDex = (int)(bc.RawDex * DexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            for (int i = 0; i < bc.Skills.Length; i++)
            {
                Skill skill = (Skill)bc.Skills[i];

                if (skill.Base > 0.0)
                    skill.Base *= SkillsBuff;
            }

            bc.PassiveSpeed /= SpeedBuff;
            bc.ActiveSpeed /= SpeedBuff;
            bc.CurrentSpeed = bc.PassiveSpeed;

            bc.DamageMin += DamageBuff;
            bc.DamageMax += DamageBuff;

            if (bc.Fame > 0)
                bc.Fame = (int)(bc.Fame * FameBuff);

            if (bc.Fame > 32000)
                bc.Fame = 32000;

            // TODO: Mana regeneration rate = Sqrt( buffedFame ) / 4

            if (bc.Karma != 0)
            {
                bc.Karma = (int)(bc.Karma * KarmaBuff);

                if (Math.Abs(bc.Karma) > 32000)
                    bc.Karma = 32000 * Math.Sign(bc.Karma);
            }
        }

        public static void UnConvertTyrant(BaseCreature bc)
        {
            if (!bc.IsTyrant)
                return;

            bc.Hue = 0;

            if (bc.HitsMaxSeed >= 0)
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed / HitsBuff);

            bc.RawStr = (int)(bc.RawStr / StrBuff);
            bc.RawInt = (int)(bc.RawInt / IntBuff);
            bc.RawDex = (int)(bc.RawDex / DexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            for (int i = 0; i < bc.Skills.Length; i++)
            {
                Skill skill = (Skill)bc.Skills[i];

                if (skill.Base > 0.0)
                    skill.Base /= SkillsBuff;
            }

            bc.PassiveSpeed *= SpeedBuff;
            bc.ActiveSpeed *= SpeedBuff;
            bc.CurrentSpeed = bc.PassiveSpeed;

            bc.DamageMin -= DamageBuff;
            bc.DamageMax -= DamageBuff;

            if (bc.Fame > 0)
                bc.Fame = (int)(bc.Fame / FameBuff);
            if (bc.Karma != 0)
                bc.Karma = (int)(bc.Karma / KarmaBuff);
        }

        public static bool CheckConvertTyrant(BaseCreature bc)
        {
            return CheckConvertTyrant(bc, bc.Location, bc.Map);
        }

        public static bool CheckConvertTyrant(BaseCreature bc, Point3D location, Map m)
        {

            if (bc is BaseChampion || bc is Harrower || bc is BaseVendor || bc is BaseEscortable || bc is Clone || bc.IsParagon || bc.IsTyrant)
                return false;

            int fame = bc.Fame;

            if (fame > 32000)
                fame = 32000;

            double chance = 0.05 / Math.Round(20.0 - (fame / 3200));

            return (chance > Utility.RandomDouble());
        }

        public static bool CheckArtifactChance(Mobile m, BaseCreature bc)
        {
            double fame = (double)bc.Fame;

            if (fame > 32000)
                fame = 32000;

            double chance = 1 / (Math.Max(10, 100 * (0.83 - Math.Round(Math.Log(Math.Round(fame / 6000, 3) + 0.001, 10), 3))) * (100 - Math.Sqrt(m.Luck)) / 100.0);

            return chance > Utility.RandomDouble();
        }

        public static void GiveArtifactTo(Mobile m)
        {
            Item item = (Item)Activator.CreateInstance(Artifacts[Utility.Random(Artifacts.Length)]);

            if (m.AddToBackpack(item))
                m.SendMessage("As a reward for slaying the mighty Tyrant, an artifact has been placed in your backpack.");
            else
                m.SendMessage("As your backpack is full, your reward for destroying the legendary Tyrant has been placed at your feet.");
        }
    }
}