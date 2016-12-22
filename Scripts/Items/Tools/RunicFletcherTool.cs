using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class RunicFletcherTool : BaseRunicTool
	{
		public override CraftSystem CraftSystem{ get{ return DefBowFletching.CraftSystem; } }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            string v = " ";

            if (!CraftResources.IsStandard(Resource))
            {
                int num = CraftResources.GetLocalizationNumber(Resource);

                if (num > 0)
                    v = String.Format("#{0}", num);
                else
                    v = CraftResources.GetName(Resource);
            }

            list.Add("{0} Runic Fletcher Tool", v); // ~1_LEATHER_TYPE~ runic sewing kit
        }

        public override void OnSingleClick(Mobile from)
        {
            string v = " ";

            if (!CraftResources.IsStandard(Resource))
            {
                int num = CraftResources.GetLocalizationNumber(Resource);

                if (num > 0)
                    v = String.Format("#{0}", num);
                else
                    v = CraftResources.GetName(Resource);
            }

            LabelTo(from, "{0} Runic Fletcher Tool", v); // ~1_LEATHER_TYPE~ runic sewing kit
        }
		[Constructable]
		public RunicFletcherTool( CraftResource resource ) : base( resource, 0x1022 )
		{
			Weight = 2.0;
         //   Name = "Runic Fletcher Tool";
			Hue = CraftResources.GetHue( resource );
		}

		[Constructable]
		public RunicFletcherTool( CraftResource resource, int uses ) : base( resource, uses, 0x1022 )
		{
			Weight = 2.0;
         //   Name = "Runic Fletcher Tool";
			Hue = CraftResources.GetHue( resource );
		}

		public RunicFletcherTool( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
        //daat99 OWTLR start - runic storage
        public override System.Type GetCraftableType()
        {
            switch (Resource)
            {
                case CraftResource.OakWood:
                    return typeof(OakRunicFletcherTools);
                case CraftResource.AshWood:
                    return typeof(AshRunicFletcherTools);
                case CraftResource.YewWood:
                    return typeof(YewRunicFletcherTools);
                case CraftResource.Heartwood:
                    return typeof(HeartwoodRunicFletcherTools);
                case CraftResource.Bloodwood:
                    return typeof(BloodwoodRunicFletcherTools);
                case CraftResource.Frostwood:
                    return typeof(FrostwoodRunicFletcherTools);
                case CraftResource.Ebony:
                    return typeof(EbonyRunicFletcherTools);
                case CraftResource.Bamboo:
                    return typeof(BambooRunicFletcherTools);
                case CraftResource.PurpleHeart:
                    return typeof(PurpleHeartRunicFletcherTools);
                case CraftResource.Redwood:
                    return typeof(RedwoodRunicFletcherTools);
                case CraftResource.Petrified:
                    return typeof(PetrifiedRunicFletcherTools);
                default:
                    return null;
            }
        }
        //daat99 OWLTR end - runic storage
	}
}
