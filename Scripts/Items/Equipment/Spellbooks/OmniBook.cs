using System;

namespace Server.Items
{
    public class OmniBook : Spellbook
    {
        [Constructable]
        public OmniBook()
            : this((ulong)0x3F)
        {
        }

        [Constructable]
        public OmniBook(ulong content)
            : base(content, 0x238C)
        {
            this.Layer = (Core.ML ? Layer.OneHanded : Layer.Invalid);
        }

        public OmniBook(Serial serial)
            : base(serial)
        {
        }

        public override SpellbookType SpellbookType
        {
            get
            {
                return SpellbookType.Omni;
            }
        }
        public override int BookOffset
        {
            get
            {
                return 2000;
            }
        }
        public override int BookCount
        {
            get
            {
                return 100;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Core.ML)
                this.Layer = Layer.OneHanded;
        }
    }
}