using System;

namespace Server.Items 
{ 
    public class BagOfBoards : Bag 
    { 
		
        [Constructable] 
        public BagOfBoards(){
		switch (Utility.Random(5))		
			 {
                case 0:
                    this.AddItem(new EbonyBoard(Utility.Random(100)));
                    break;
				case 1:
                    this.AddItem(new BambooBoard(Utility.Random(80)));
                    break;
				case 2:
                    this.AddItem(new PurpleHeartBoard(Utility.Random(60)));
                    break;
				case 3:
                    this.AddItem(new RedwoodBoard(Utility.Random(40)));
                    break;
				case 4:
                    this.AddItem(new PetrifiedBoard(Utility.Random(20)));
                    break;
			}
		}
        public BagOfBoards(Serial serial)
            : base(serial)
        { 
        }

        public override void Serialize(GenericWriter writer) 
        { 
            base.Serialize(writer); 

            writer.Write((int)0); // version 
        }

        public override void Deserialize(GenericReader reader) 
        { 
            base.Deserialize(reader); 

            int version = reader.ReadInt(); 
        }
    }
}