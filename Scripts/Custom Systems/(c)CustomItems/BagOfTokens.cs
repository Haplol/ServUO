using System;

namespace Server.Items 
{ 
    public class BagOfTokens : Bag 
    { 
		
        [Constructable] 
        public BagOfTokens(){
		switch (Utility.Random(10))		
			 {
                case 0:
                    this.AddItem(new Daat99Tokens(100));
                    break;
				case 1:
                    this.AddItem(new Daat99Tokens(100));
                    break;
				case 2:
                    this.AddItem(new Daat99Tokens(100));
                    break;
				case 3:
                    this.AddItem(new Daat99Tokens(500));
                    break;
				case 4:
                    this.AddItem(new Daat99Tokens(500));
                    break;
				case 5:
                    this.AddItem(new Daat99Tokens(500));
                    break;
				case 6:
                    this.AddItem(new Daat99Tokens(500));
                    break;
				case 7:
                    this.AddItem(new Daat99Tokens(500));
                    break;
				case 8:
                    this.AddItem(new Daat99Tokens(1000));
                    break;
				case 9:
                    this.AddItem(new Daat99Tokens(1000));
                    break;
			}
		}
        public BagOfTokens(Serial serial)
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