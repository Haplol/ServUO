
namespace Server.Items
{
	public class Deco : Item
	{
		[Constructable] 
		public Deco( int itemid, string name ) : base( itemid ) 
		{ 
			Weight = 1.0;
			Name = name;
		} 

		public Deco( Serial serial ) : base( serial )
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
	}
}