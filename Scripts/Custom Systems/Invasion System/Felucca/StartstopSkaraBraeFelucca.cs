using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network; 
using System.Collections;

namespace Server.Gumps
{
	public class StartStopSkaraBraefel : Gump
	{
		private Mobile m_Mobile;
		public StartStopSkaraBraefel(Mobile from) : base(0,0)
		{
			m_Mobile = from;
			Closable = false;
			Dragable = true;

			AddPage(0);

			AddImage( 112, 73, 39);
			AddButton( 135, 123, 9804, 9806, 1, GumpButtonType.Reply, 1 );
			AddButton( 138, 194, 9804, 9806, 2, GumpButtonType.Reply, 2 );
			AddButton( 277, 311, 2453, 2455, 0, GumpButtonType.Reply, 0 );
			AddLabel( 216, 140, 0, "Start an Invasion");
			AddLabel( 218, 208, 0, "Stop an Invasion");

		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			switch ( info.ButtonID ) 
		{ 
		case 0:
		{ 
                                    from.CloseGump( typeof( StartStopSkaraBraefel ) ); 
		        from.SendGump( new CityInvasion( from ) );
                                    break;  
                             }  
		case 1:
		{
			Point3D loc = new Point3D( 568, 1311, 0 );
			WayPoint point = new WayPoint();
			WayPoint point1 = new WayPoint();
			WayPoint point2 = new WayPoint();
			//WayPoint point3 = new WayPoint();

			point.Name = "SkaraBraeInvasionFelucca";
			point1.Name = "SkaraBraeInvasionFelucca";
			point2.Name = "SkaraBraeInvasionFelucca";

			Spawner spawner1 = new Spawner( 1, 1440, 1441, 0, 6, "SavageShaman" );
			spawner1.MoveToWorld( new Point3D(  956, 273, 0  ), Map.Malas );
			spawner1.WayPoint = point;
			point.MoveToWorld( new Point3D(  950, 265, -90  ), Map.Malas );
			point.NextPoint = point2;
			point2.MoveToWorld( new Point3D(  950, 275, -90  ), Map.Malas );
          			spawner1.Name = "SkaraBraeInvasionFelucca";
			spawner1.Respawn();

			Spawner spawner2 = new Spawner( 1, 1440, 1441, 0, 6, "Savage" );
			spawner2.MoveToWorld( new Point3D(  952, 284, -90  ), Map.Malas );
			spawner2.WayPoint = point1;
			point1.MoveToWorld( new Point3D(  952, 265, -90  ), Map.Malas );
			spawner2.Name = "SkaraBraeInvasionFelucca";
			spawner2.Respawn();

			Spawner spawner3 = new Spawner( 1, 1440, 1441, 0, 6, "Savage" );
			spawner3.MoveToWorld( new Point3D(  950, 244, -90  ), Map.Malas );
			spawner3.WayPoint = point;
			spawner3.Name = "SkaraBraeInvasionFelucca";
			spawner3.Respawn();

			Spawner spawner4 = new Spawner( 1, 1440, 1441, 0, 6, "SavageRider" );
			spawner4.MoveToWorld( new Point3D(  952, 244, -90  ), Map.Malas );
			spawner4.WayPoint = point1;
			spawner4.Name = "SkaraBraeInvasionFelucca";
			spawner4.Respawn();

                                           World.Broadcast( 33, true, "SkaraBrae Felucca is under invasion." );
		               from.SendGump( new CityInvasion( from ) );
                                   	 break; 
                            }  
		case 2:
		{
			SkaraBraeInvasionStone SkaraBraefel = new SkaraBraeInvasionStone();
			SkaraBraefel.StopSkaraBraeFelucca();
                                          World.Broadcast( 33, true, "SkaraBrae Felucca's invasion was successfully beaten back. No more invaders are left in the city." );
		              from.SendGump( new CityInvasion( from ) );
                                   	break; 
				}
			}
		}
	}
}
