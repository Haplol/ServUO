using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTyrant : XmlAttachment
    {
        #region private properties
        // default artifact types
        private static Type[] m_Artifacts = new Type[]
		{
			typeof( AlchemistsBaubleplus ), typeof( GwennosHarpplus ), 
			typeof( ArcticDeathDealerplus ), typeof( PeasantsBokutoplus ),
			typeof( EyesOfHateplus ), typeof( PolarBearMaskplus )
		};

        private int m_Hue;

        private double m_HitsBuff;
        private double m_StrBuff;
        private double m_IntBuff;
        private double m_DexBuff;
        private double m_SkillsBuff;
        private double m_SpeedBuff;
        private double m_FameBuff;
        private double m_KarmaBuff;
        private int m_DamageBuff;

        private bool m_EnableTyrant = true;
        private string m_TyrantLabel;
        private double m_ConvertFactor = 1;
        private double m_ArtifactFactor = 1;
        #endregion

        #region public properties

        public virtual Type[] Artifacts { get { return m_Artifacts; } set { m_Artifacts = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double ConvertFactor { get { return m_ConvertFactor; } set { m_ConvertFactor = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double ArtifactFactor { get { return m_ArtifactFactor; } set { m_ArtifactFactor = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int Hue { get { return m_Hue; } set { m_Hue = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double HitsBuff { get { return m_HitsBuff; } set { m_HitsBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double StrBuff { get { return m_StrBuff; } set { m_StrBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double IntBuff { get { return m_IntBuff; } set { m_IntBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double DexBuff { get { return m_DexBuff; } set { m_DexBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SkillsBuff { get { return m_SkillsBuff; } set { m_SkillsBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SpeedBuff { get { return m_SpeedBuff; } set { m_SpeedBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double FameBuff { get { return m_FameBuff; } set { m_FameBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double KarmaBuff { get { return m_KarmaBuff; } set { m_KarmaBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int DamageBuff { get { return m_DamageBuff; } set { m_DamageBuff = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool EnableTyrant { get { return m_EnableTyrant; } set { m_EnableTyrant = value; InvalidateParentProperties();  } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual string TyrantLabel { get { return m_TyrantLabel; } set { m_TyrantLabel = value; } }
        #endregion

        #region static interface methods

        public static XmlTyrant GetXmlTyrant(BaseCreature bc)
        {
            // check for an XmlTyrant attachment on the spawner of the creature
            if (bc != null && bc.Spawner != null)
            {
                return (XmlTyrant)XmlAttach.FindAttachment(bc.Spawner, typeof(XmlTyrant));
            }

            return null;
        }

        public static bool IsCustomTyrant(BaseCreature bc)
        {
            return (GetXmlTyrant(bc) != null);
        }

        public static string GetTyrantLabel(BaseCreature bc)
        {
            XmlTyrant xa = GetXmlTyrant(bc);

            if (xa != null)
            {
                return xa.XmlGetTyrantLabel(bc);
            }
            else
            {
                return "(Tyrant)";
            }
        }

        // static method hooks that interface with the distro Tyrant system

        public static bool CheckConvertTyrant(BaseCreature bc, Point3D location, Map m)
        {
            XmlTyrant xa = GetXmlTyrant(bc);

            if (xa != null)
            {
                return xa.XmlCheckConvert(bc, location, m);
            }
            else
            {
                return Tyrant.CheckConvertTyrant(bc, location, m);
            }
        }


        public static double GetHitsBuff(BaseCreature bc)
        {
            XmlTyrant xa = GetXmlTyrant(bc);

            if (xa != null)
            {
                return xa.HitsBuff;
            }
            else
            {
                return Tyrant.HitsBuff;
            }
        }

        public static void Convert(BaseCreature bc)
        {
            XmlTyrant xa = GetXmlTyrant(bc);

            if (xa != null)
            {
                xa.XmlConvert(bc);
            }
            else
            {
                Tyrant.ConvertTyrant(bc);
            }
        }

        public static void UnConvert(BaseCreature bc)
        {
            XmlTyrant xa = GetXmlTyrant(bc);

            if (xa != null)
            {
                xa.XmlUnConvert(bc);
            }
            else
            {
                Tyrant.UnConvertTyrant(bc);
            }
        }

        public static bool CheckArtifactChance(Mobile m, BaseCreature bc)
        {
            XmlTyrant xa = GetXmlTyrant(bc);

            if (xa != null)
            {
                return xa.XmlCheckArtifactChance(m, bc);
            }
            else
            {
                return Tyrant.CheckArtifactChance(m, bc);
            }
        }

        public static void GiveArtifactTo(Mobile m, BaseCreature bc)
        {
            XmlTyrant xa = GetXmlTyrant(bc);

            if (xa != null)
            {
                xa.XmlGiveArtifactTo(m, bc);
            }
            else
            {
                Tyrant.GiveArtifactTo(m);
            }
        }

        #endregion

        // modify these custom conversion methods to create unique Tyrants for spawners with this attachment
        #region custom conversion methods


        public virtual string XmlGetTyrantLabel(BaseCreature bc)
        {
            return TyrantLabel;
        }

        public virtual void XmlConvert(BaseCreature bc)
        {
            if (bc == null || bc.IsTyrant)
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

        public virtual void XmlUnConvert(BaseCreature bc)
        {
            if (bc == null || !bc.IsTyrant)
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

            bc.DamageMin -= DamageBuff;
            bc.DamageMax -= DamageBuff;

            if (bc.Fame > 0)
                bc.Fame = (int)(bc.Fame / FameBuff);
            if (bc.Karma != 0)
                bc.Karma = (int)(bc.Karma / KarmaBuff);
        }

        public virtual bool XmlCheckConvert(BaseCreature bc, Point3D location, Map m)
        {
            if (!EnableTyrant || bc == null)
                return false;

            if (bc is BaseChampion || bc is Harrower || bc is BaseVendor || bc is BaseEscortable || bc is Clone || bc is Paragon)
                return false;

            int fame = bc.Fame;

            if (fame > 32000)
                fame = 32000;

            double chance = ConvertFactor / Math.Round(20.0 - (fame / 3200));

            return (chance > Utility.RandomDouble());
        }

        public virtual bool XmlCheckArtifactChance(Mobile m, BaseCreature bc)
        {
            if (m == null || bc == null) return false;

            double fame = (double)bc.Fame;

            if (fame > 32000)
                fame = 32000;

            double chance = ArtifactFactor / (Math.Max(10, 100 * (0.83 - Math.Round(Math.Log(Math.Round(fame / 6000, 3) + 0.001, 10), 3))) * (100 - Math.Sqrt(m.Luck)) / 100.0);

            return chance > Utility.RandomDouble();
        }

        public virtual void XmlGiveArtifactTo(Mobile m, BaseCreature bc)
        {
            if (m == null) return;

            Item item = (Item)Activator.CreateInstance(Artifacts[Utility.Random(Artifacts.Length)]);

            if (m.AddToBackpack(item))
                m.SendMessage("As a reward for slaying the mighty Tyrant, an artifact has been placed in your backpack.");
            else
                m.SendMessage("As your backpack is full, your reward for destroying the legendary Tyrant has been placed at your feet.");
        }

        #endregion

        #region attachment method overrides
        public override string OnIdentify(Mobile from)
        {
            return String.Format("Tyrant: {0}", EnableTyrant ? "Enabled" : "Disabled");
        }

        public override void OnAttach()
        {
            base.OnAttach();

            InvalidateParentProperties();
        }

        public override void OnDelete()
        {
            base.OnDelete();

            InvalidateParentProperties();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            // version 0
            writer.Write(m_Hue);
            writer.Write(m_HitsBuff);
            writer.Write(m_StrBuff);
            writer.Write(m_IntBuff);
            writer.Write(m_DexBuff);
            writer.Write(m_SkillsBuff);
            writer.Write(m_SpeedBuff);
            writer.Write(m_FameBuff);
            writer.Write(m_KarmaBuff);
            writer.Write(m_DamageBuff);
            writer.Write(m_EnableTyrant);
            writer.Write(m_TyrantLabel);
            writer.Write(m_ConvertFactor);
            writer.Write(m_ArtifactFactor);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            // version 0
            m_Hue = reader.ReadInt();
            m_HitsBuff = reader.ReadDouble();
            m_StrBuff = reader.ReadDouble();
            m_IntBuff = reader.ReadDouble();
            m_DexBuff = reader.ReadDouble();
            m_SkillsBuff = reader.ReadDouble();
            m_SpeedBuff = reader.ReadDouble();
            m_FameBuff = reader.ReadDouble();
            m_KarmaBuff = reader.ReadDouble();
            m_DamageBuff = reader.ReadInt();
            m_EnableTyrant = reader.ReadBool();
            m_TyrantLabel = reader.ReadString();
            m_ConvertFactor = reader.ReadDouble();
            m_ArtifactFactor = reader.ReadDouble();
        }
        #endregion

        #region constructors
        public XmlTyrant(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlTyrant()
        {
            Hue = 0x780;

            // standard buff modifiers
            HitsBuff = 25.0;
            StrBuff = 3.00;
            IntBuff = 3.00;
            DexBuff = 3.00;
            SkillsBuff = 2.00;
            SpeedBuff = 1.80;
            FameBuff = 1.00;
            KarmaBuff = 1.00;
            DamageBuff = 25;

            ConvertFactor = 1;
            ArtifactFactor = 1;
            TyrantLabel = "(Tyrant)";
        }
        #endregion
    }
}