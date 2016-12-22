///////////////////////////////////
//       Layer Change Deed       //
//         by: ViWinfii          //
//             v0.1              //
///////////////////////////////////
using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;
using Server.Gumps;

namespace Server.Items
{
    public class LayerTarget : Target
    {
        public static Layer[] ValidLayers
        {
            get
            {
                Layer[] lays = 
                { 
                    Layer.TwoHanded,
					Layer.OneHanded
                   
                };
                return lays;
            }
        }

        public static bool IsValid( Layer lay)
        {
            for (int i = 0; i < ValidLayers.Length; i++)
                if (ValidLayers[i] == lay)
                    return true;

            return false;
        }
        private OneHandedDeed m_Deed;

        public LayerTarget(OneHandedDeed deed)
            : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

		protected override void OnTarget( Mobile from, object target )
		{
            if (m_Deed.Deleted || m_Deed.RootParent != from)
            {
                m_Deed.Movable = true;
                return;
            }

            if (target is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)target;

                if ((weapon.RootParent != from) || !(weapon.IsChildOf(from.Backpack)))
                {
                    from.SendMessage("You must target something in your pack that is not equipped.");
                    m_Deed.Movable = true;
                    return;
                }

                if (!(IsValid(weapon.Layer)))
                {
                    from.SendMessage("You must target something in your pack that has a valid Layer.");
                    m_Deed.Movable = true;
                    return;
                }

                m_Deed.Movable = false;
                weapon.Movable = false;

                if (from.HasGump(typeof(LayerChangeGump)))
                    from.CloseGump(typeof(LayerChangeGump));

                from.SendGump(new LayerChangeGump(m_Deed, weapon));
            }
            else
                from.SendMessage("Invalid target.");
		}
	}

	public class OneHandedDeed : Item
	{
        private int m_uses;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Uses
        {
            get{ return m_uses; }
            set
            {
                if (!m_InfiniteUse)
                {
                    m_uses = value;
                    if (m_uses <=0)
                        this.Delete();
                }

                return;
            }
        }

        private bool m_InfiniteUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool InfiniteUse
        {
            get { return m_InfiniteUse; }
            set
            {
                m_InfiniteUse = value;
                return;
            }
        }

         

		public override string DefaultName
		{
            get { return "One Handed Deed"; }
		}

        [Constructable]
        public OneHandedDeed() : this(1)
        {

        }

        [Constructable]
        public OneHandedDeed(int uses) : base(0x14F0)
        {
            m_uses = uses;
            m_InfiniteUse = false;
            this.Weight = 1.0;
            LootType = LootType.Blessed;
        }

        [Constructable]
        public OneHandedDeed(bool IsInfinite)
            : base(0x14F0)
        {
            m_uses = 1;
            m_InfiniteUse = IsInfinite;
            this.Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public OneHandedDeed(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
            writer.Write((int)m_uses);
            writer.Write((bool)m_InfiniteUse);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_uses = reader.ReadInt();
                        m_InfiniteUse = reader.ReadBool();
                        break;
                    }
                case 0:
                    {
                        m_uses = 1;
                        m_InfiniteUse = false;
                        break;
                    }
            }
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target
		{
			if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
                this.Movable = false;
                from.SendMessage("Please target your item to change layer.");
				from.Target = new LayerTarget( this ); // Call our target
			 }
		}	
	}
}

namespace Server.Gumps
{
    public class LayerChangeGump : Gump
    {
        private BaseWeapon m_weap;
        private Layer m_selection;
        private OneHandedDeed m_deed;
        private Layer[] lays;

        public LayerChangeGump(OneHandedDeed deed, BaseWeapon weap)
            : this(deed, weap, Layer.Invalid)
        {
        }
        public LayerChangeGump(OneHandedDeed deed, BaseWeapon weap, Layer selection)
            : base(50, 50)
        {
            m_weap = weap;
            m_selection = selection;
            m_deed = deed;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            lays = LayerTarget.ValidLayers;

            int space = 25;

            AddBackground();
            for (int i = 1; i < 3; i++)
            {
                AddPage(i);
                for (int j = 0; j < 9; j++)
                {
                    if ((lays.Length >= (i - 1) * 9 + j + 1) && (lays[(i - 1) * 9 + j] != m_selection))
                        AddButton(80, 117 + space * j, 1202, 1204, (i - 1) * 9 + j + 1, GumpButtonType.Reply, 0);
                }
                AddBackground(76, 116, 165, 225, 9200);
                for (int j = 0; j < 9; j++)
                {
                    if ((lays.Length >= (i - 1) * 9 + j + 1) && (lays[(i - 1) * 9 + j] != m_selection))
                        AddLabel(103, 115 + space * j, 0, lays[(i - 1) * 9 + j].ToString());
                }

                if (i == 1)
                    AddButton(213, 341, 5224, 248, 30, GumpButtonType.Page, 2);
                else if (i == 2)
                    AddButton(84, 341, 5223, 5223, 40, GumpButtonType.Page, 1);

                if (m_selection != Layer.Invalid)
                    AddButton(139, 372, 247, 248, 50, GumpButtonType.Reply, 0);

                AddButton(42, 372, 242, 243, 0, GumpButtonType.Reply, 0);

            }

        }
        private void AddBackground()
        {
            AddPage(0);
            AddBackground(4, 4, 244, 404, 9200);
            AddItem(18, 138, m_weap.ItemID, m_weap.Hue);
            AddLabel(49, 12, 0, @"Change Layer Menu");
            AddLabel(20, 39, 0, @"Item Name: " + (String.IsNullOrEmpty(m_weap.Name) ? m_weap.GetType().Name : m_weap.Name ));
            AddLabel(13, 62, 0, @"Current Layer: " + m_weap.Layer.ToString());

            if (m_selection != Layer.Invalid)
                AddLabel(20, 87, 0, @"Change To: " + m_selection.ToString());
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        m_weap.Movable = true;
                        m_deed.Movable = true;
                        break;
                    }
                case 50:
                    {
                        if (m_selection != Layer.Invalid)
                        {
                            m_weap.Layer = m_selection;
                            m_weap.Movable = true;
                            m_deed.Movable = true;
                            m_deed.Uses--;
                        }
                        break;
                    }
                default:
                    {
                        if (from.HasGump(typeof(LayerChangeGump)))
                            from.CloseGump(typeof(LayerChangeGump));

                        from.SendGump(new LayerChangeGump(m_deed, m_weap, lays[info.ButtonID - 1]));
                        break;
                    }

            }
        }
    }
}