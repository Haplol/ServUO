using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute( 0x1028, 0x1029 )]
	public class RunicDovetailSaw : BaseRunicTool
	{
		public override CraftSystem CraftSystem{ get{ return DefCarpentry.CraftSystem; } }

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

            list.Add("{0} Runic Dovetail Saw", v); // ~1_LEATHER_TYPE~ runic sewing kit
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

            LabelTo(from, "{0} Runic Dovetail Saw", v); // ~1_LEATHER_TYPE~ runic sewing kit
        }
		[Constructable]
		public RunicDovetailSaw( CraftResource resource ) : base( resource, 0x1029 )
		{
			Weight = 2.0;
       //     Name = "Runic Dovetail Saw";
			Hue = CraftResources.GetHue( resource );
		}

		[Constructable]
		public RunicDovetailSaw( CraftResource resource, int uses ) : base( resource, uses, 0x1029 )
		{
			Weight = 2.0;
        //    Name = "Runic Dovetail Saw";
			Hue = CraftResources.GetHue( resource );
		}

		public RunicDovetailSaw( Serial serial ) : base( serial )
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
                    return typeof(OakRunicDovetailSaw);
				case CraftResource.AshWood:
                    return typeof(AshRunicDovetailSaw);
				case CraftResource.YewWood:
                    return typeof(YewRunicDovetailSaw);
				case CraftResource.Heartwood:
                    return typeof(HeartwoodRunicDovetailSaw);
				case CraftResource.Bloodwood:
                    return typeof(BloodwoodRunicDovetailSaw);
				case CraftResource.Frostwood:
                    return typeof(FrostwoodRunicDovetailSaw);
				case CraftResource.Ebony:
                    return typeof(EbonyRunicDovetailSaw);
				case CraftResource.Bamboo:
                    return typeof(BambooRunicDovetailSaw);
				case CraftResource.PurpleHeart:
                    return typeof(PurpleHeartRunicDovetailSaw);
				case CraftResource.Redwood:
                    return typeof(RedwoodRunicDovetailSaw);
				case CraftResource.Petrified:
                    return typeof(PetrifiedRunicDovetailSaw);
				default:
					return null;
			}
		}
		//daat99 OWLTR end - runic storage
	}
}