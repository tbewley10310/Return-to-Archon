
////////////////////////////////////////
//                                     //
//   Generated by CEO's YAAAG - Ver 2  //
// (Yet Another Arya Addon Generator)  //
//    Modified by Hammerhand for       //
//      SA & High Seas content         //
//                                     //
////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ChamnpSpawnPlatformAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1963, -3, 3, 0}, {1959, -3, 2, 0}, {1959, -3, 1, 0}// 1	2	3	
			, {1959, -3, 0, 0}, {1959, -3, -1, 0}, {1959, -3, -2, 0}// 4	5	6	
			, {1960, -3, -3, 0}, {1956, -2, 3, 0}, {1958, -2, -3, 0}// 7	8	9	
			, {1955, -2, -2, 0}, {1955, -2, -1, 0}, {1955, -2, 2, 0}// 10	11	12	
			, {1955, -2, 1, 0}, {1955, -2, 0, 0}, {1956, -1, 3, 0}// 13	14	15	
			, {1958, -1, -3, 0}, {1955, -1, -2, 0}, {1955, -1, -1, 0}// 16	17	18	
			, {1955, -1, 2, 0}, {1955, -1, 1, 0}, {1955, -1, 0, 0}// 19	20	21	
			, {1956, 0, 3, 0}, {1956, 1, 3, 0}, {1956, 2, 3, 0}// 22	23	24	
			, {1961, 3, 3, 0}, {1957, 3, 2, 0}, {1957, 3, 1, 0}// 25	26	27	
			, {1957, 3, 0, 0}, {1957, 3, -1, 0}, {1957, 3, -2, 0}// 28	29	30	
			, {1962, 3, -3, 0}, {1958, 2, -3, 0}, {1958, 1, -3, 0}// 31	32	33	
			, {1958, 0, -3, 0}, {1955, 0, -2, 0}, {1955, 1, -2, 0}// 34	35	36	
			, {1955, 2, -2, 0}, {1955, 0, -1, 0}, {1955, 1, -1, 0}// 37	38	39	
			, {1955, 2, -1, 0}, {1955, 0, 2, 0}, {1955, 1, 2, 0}// 40	41	42	
			, {1955, 2, 2, 0}, {1955, 0, 1, 0}, {1955, 1, 1, 0}// 43	44	45	
			, {1955, 2, 1, 0}, {1955, 0, 0, 0}, {1955, 1, 0, 0}// 46	47	48	
			, {1955, 2, 0, 0}, {3649, -1, 1, 0}, {3026, 0, 0, 20}// 49	60	62	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new ChamnpSpawnPlatformAddonDeed();
			}
		}

		[ Constructable ]
		public ChamnpSpawnPlatformAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 6228, -2, -2, 5, 38, 2, "", 1);// 50
			AddComplexComponent( (BaseAddon) this, 6228, 1, 1, 5, 0, 2, "", 1);// 51
			AddComplexComponent( (BaseAddon) this, 6228, 1, -1, 5, 0, 2, "", 1);// 52
			AddComplexComponent( (BaseAddon) this, 6228, -1, -1, 5, 0, 2, "", 1);// 53
			AddComplexComponent( (BaseAddon) this, 6228, 2, 0, 5, 38, 2, "", 1);// 54
			AddComplexComponent( (BaseAddon) this, 6228, 2, -1, 5, 38, 2, "", 1);// 55
			AddComplexComponent( (BaseAddon) this, 6228, 2, -2, 5, 38, 2, "", 1);// 56
			AddComplexComponent( (BaseAddon) this, 6228, 1, -2, 5, 38, 2, "", 1);// 57
			AddComplexComponent( (BaseAddon) this, 6228, 0, -2, 5, 38, 2, "", 1);// 58
			AddComplexComponent( (BaseAddon) this, 6228, -1, -2, 5, 38, 2, "", 1);// 59
			AddComplexComponent( (BaseAddon) this, 7960, 0, 0, 5, 0, -1, "Idol of the Champion", 1);// 61

		}

		public ChamnpSpawnPlatformAddon( Serial serial ) : base( serial )
		{
		}

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType) lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class ChamnpSpawnPlatformAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new ChamnpSpawnPlatformAddon();
			}
		}

		[Constructable]
		public ChamnpSpawnPlatformAddonDeed()
		{
			Name = "ChamnpSpawnPlatform";
		}

		public ChamnpSpawnPlatformAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}