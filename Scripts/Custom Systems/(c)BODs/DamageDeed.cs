using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server;

namespace Server.Items
{
    public class DamageTargetx : Target
    {
        private DamageDeed m_Deed;

        public DamageTargetx(DamageDeed deed)
            : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Deed.Deleted || m_Deed.RootParent != from)
            {
                from.SendMessage("You cannot add Damage to that.");
                return;
            }
            if (target is BaseWeapon)
            {
                BaseWeapon item = (BaseWeapon)target;
                if (item is BaseWeapon)
                {              
					((BaseWeapon)item).MaxDamage += 1;
					((BaseWeapon)item).MinDamage += 1;
					from.SendMessage("Damage successfully added to item.");
					m_Deed.Delete();
                }                  
            }
            else
            {
                from.SendMessage("You cannot put Damage on that.");
            }
        }
    }

    public class DamageDeed : Item
    {
        [Constructable]
        public DamageDeed()
            : base(0x14F0)
        {
            LootType = LootType.Blessed;
            Name = "a Plus Damage Deed";
            Hue = 1150;
            Weight = 1.0;
        }

        public DamageDeed(Serial serial)
            : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The item needs to be in your pack");
            }
            else
            {
                from.SendMessage("Which item would you like to add Damage to?");
                from.Target = new DamageTargetx(this);
            }
        }
    }
}