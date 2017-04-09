/***************************************************************************
 *                            CorruptedCrystalPortal.cs
 *                            -------------------
 *   begin                : Jan 16, 2015
 *   copyright            : Higoo
 *   email                : runuohg@gmail.com
 *   remark               :
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using Server.Engines.VeteranRewards;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class CorruptedCrystalPortal : BaseBook, Engines.VeteranRewards.IRewardItem
	{
		private Point3D m_PointDest;
		private Map m_MapDest;

		private bool m_IsRewardItem;

		private static readonly BookContent Content = new BookContent
		(
		    "Corrupted Crystal Portal", "    unknown",
		    new BookPageInfo
		    (
		        "This corrupted portal",
		        "allows you to teleport",
		        "directly to a dungeon.",
		        "For Trammel ruleset, say",
		        "\"dungeon\" followed by",
		        "the name of the dungeon",
		        "(e.g. \"dungeon shame\"). "
		    ),
		    new BookPageInfo
		    (
		        "For Felucca, say \"fel\"",
		        "then same rules as",
		        "above.",
		        "So \"fel dungeon shame\"."
		    ),
		    new BookPageInfo
		    (
		        "DUNGEON NAMES:",
		        "covetous, deceit,",
		        "despise, destard, ice,",
		        "fire, hythloth, orc,",
		        "shame, wrong, wind,",
		        "doom, citadel, fandancer,",
		        "mines, bedlam, labyrinth,",
		        "underworld, abyss, grove,"
		    ),
		    new BookPageInfo
		    (
		        "caves, palace, prism,",
		        "sanctuary",
		        " ",
		        "The same teleportation",
		        "rules apply regarding",
		        "criminal flagging",
		        "weight, etc."
		    )
		);

		public override BookContent DefaultContent
		{
			get
			{
				return Content;
			}
		}

		[Constructable]
		public CorruptedCrystalPortal() : this(false)
		{
		}

		public CorruptedCrystalPortal(bool isRewardItem)
		: base(0x468A, 4, false)
		{
			LootType = LootType.Blessed;
			m_IsRewardItem = isRewardItem;
			Level = SecureLevel.Anyone;
			Hue = 0xA29;
			Title = "Corrupted Crystal Portal";
			Author = "     unknown";
		}

		public CorruptedCrystalPortal(Serial serial) : base(serial)
		{
		}

		public override bool HandlesOnSpeech
		{
			get
			{
				return true;
			}
		}

		public override int LabelNumber
		{
			get
			{
				return 1150074;    // Corrupted Crystal Portal
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsRewardItem
		{
			get
			{
				return m_IsRewardItem;
			}
			set
			{
				m_IsRewardItem = value;
				InvalidateProperties();
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (Core.ML && m_IsRewardItem)
				list.Add(RewardSystem.GetRewardYearLabel(this, new object[] { }));    // X Year Veteran Reward
		}

		public bool CheckAccess(Mobile m)
		{
			BaseHouse house = BaseHouse.FindHouseAt(this);

			if (house != null && (house.Public ? house.IsBanned(m) : !house.HasAccess(m)))
			{
				return false;
			}

			return (house != null && house.HasSecureAccess(m, Level));
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.LocalOverheadMessage(MessageType.Regular, 906, 1019045);   // I can't reach that.
				return;
			}
			else if (!from.InLOS(this))
			{
				from.SendLocalizedMessage(502800); // You can't see that.
				return;
			}

			base.OnDoubleClick(from);
		}

		public override bool OnMoveOver(Mobile m)
		{
			return false;
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			Mobile mobile = e.Mobile;
			CCPList[] checkLists;

			if (!mobile.InLOS(this) || !mobile.InRange(GetWorldLocation(), 5))
			{
				return;
			}

			if (!e.Handled && IsLockedDown)
			{
				if (mobile.Player)
				{
					ClientFlags flags = mobile.NetState == null ? ClientFlags.None : mobile.NetState.Flags;

					if (Core.SA && (flags & ClientFlags.TerMur) != 0)
					{
						checkLists = CCPList.SALists;
					}
					else if (Core.SE && (flags & ClientFlags.Tokuno) != 0)
					{
						checkLists = CCPList.SELists;
					}
					else if (Core.AOS && (flags & ClientFlags.Malas) != 0)
					{
						checkLists = CCPList.AOSLists;
					}
					else if ((flags & ClientFlags.Ilshenar) != 0)
					{
						checkLists = CCPList.LBRLists;
					}
					else
					{
						checkLists = CCPList.UORLists;
					}
				}
				else
				{
					checkLists = CCPList.SALists;
				}

				bool isMatch = false;

				for (int i = 0; i < checkLists.Length; i++)
				{
					foreach (CCPEntry entry in checkLists[i].Entries)
					{
						if (entry.Word == e.Speech.ToLower())
						{
							isMatch = true;

							if (e.Speech.ToLower() == "dungeon underworld")
							{
								m_MapDest = Map.Trammel;
							}
							else
							{
								m_MapDest = checkLists[i].Map;
							}

							m_PointDest = entry.Location;
							break;
						}
					}
				}

				if (!isMatch)
				{
					return;
				}

				if (!CheckAccess(mobile))
				{
					this.PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3E9, 1061637);   // You are not allowed to access this.
					return;
				}

				e.Handled = true;
				DoTeleport(mobile);
			}
		}

		public virtual void DoTeleport(Mobile m)
		{
			Map map = m_MapDest;

			if (map == null || map == Map.Internal)
			{
				map = m.Map;
			}

			Point3D p = m_PointDest;

			if (p == Point3D.Zero)
			{
				p = m.Location;
			}

			if (Factions.Sigil.ExistsOn(m))
			{
				m.SendLocalizedMessage(1061632);   // You can't do that while carrying the sigil.
			}
			else if (map == Map.Felucca && m is PlayerMobile && ((PlayerMobile)m).Young)
			{
				m.SendLocalizedMessage(1049543);   // You decide against traveling to Felucca while you are still young.
			}
			else if (m.Kills >= 5 && map != Map.Felucca)
			{
				m.SendLocalizedMessage(1019004);   // You are not allowed to travel there.
			}
			else if (m.Criminal)
			{
				m.SendLocalizedMessage(1005561, "", 0x22);   // Thou'rt a criminal and cannot escape so easily.
			}
			else if (SpellHelper.CheckCombat(m))
			{
				m.SendLocalizedMessage(1005564, "", 0x22);   // Wouldst thou flee during the heat of battle??
			}
			else if (Server.Misc.WeightOverloading.IsOverloaded(m))
			{
				m.SendLocalizedMessage(502359, "", 0x22);   // Thou art too encumbered to move.
			}
			else
			{
				Server.Mobiles.BaseCreature.TeleportPets(m, p, map);

				bool sendEffect = (!m.Hidden || m.AccessLevel == AccessLevel.Player);

				if (sendEffect)
				{
					Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 10, 10);
				}

				m.MoveToWorld(p, map);

				if (sendEffect)
				{
					Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 10, 10);
					Effects.PlaySound(m.Location, m.Map, 0x1FE);
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);   // version

			writer.Write((bool) m_IsRewardItem);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_IsRewardItem = reader.ReadBool();
		}

		private class CCPEntry
		{
			private string m_Word;
			private Point3D m_Location;

			public string Word
			{
				get
				{
					return m_Word;
				}
			}

			public Point3D Location
			{
				get
				{
					return m_Location;
				}
			}

			public CCPEntry(string word, Point3D loc)
			{
				m_Word = word;
				m_Location = loc;
			}
		}

		private class CCPList
		{
			private Map m_Map;
			private CCPEntry[] m_Entries;

			public Map Map
			{
				get
				{
					return m_Map;
				}
			}

			public CCPEntry[] Entries
			{
				get
				{
					return m_Entries;
				}
			}

			public CCPList(Map map, CCPEntry[] entries)
			{
				m_Map = map;
				m_Entries = entries;
			}

			public static readonly CCPList Trammel =
			    new CCPList(Map.Trammel, new CCPEntry[]
			{
				new CCPEntry("dungeon covetous",	    new Point3D(2498,  920,   0)),      // Covetous
				new CCPEntry("dungeon deceit",	        new Point3D(4111,  433,   5)),      // Deceit
				new CCPEntry("dungeon despise",		    new Point3D(1301, 1080,   0)),      // Despise
				new CCPEntry("dungeon destard",		    new Point3D(1176, 2639,   0)),      // Destard
				new CCPEntry("dungeon ice",		        new Point3D(1999,   80,   4)),      // Ice
				new CCPEntry("dungeon fire",	        new Point3D(2923, 3410,   5)),      // Fire
				new CCPEntry("dungeon hythloth",	    new Point3D(4721, 3823,   0)),      // Hythloth
				new CCPEntry("dungeon orc",	            new Point3D(1017, 1434,   0)),      // Orc Cave
				new CCPEntry("dungeon shame",		    new Point3D( 513, 1563,   0)),      // Shame
				new CCPEntry("dungeon wrong",		    new Point3D(2044,  237,  10)),      // Wrong
				new CCPEntry("dungeon wind",		    new Point3D(1362,  896,   0)),      // Wind
				new CCPEntry("dungeon grove",	        new Point3D( 587, 1641,  -1)),      // Blighted Grove
				new CCPEntry("dungeon caves",	        new Point3D(1717, 2996,  -1)),      // Painted Caves
				new CCPEntry("dungeon palace",		    new Point3D(5586, 3022,  36)),      // Palace of Paroxysmus
				new CCPEntry("dungeon prism",		    new Point3D(3785, 1106,  20)),      // Prism of Light
				new CCPEntry("dungeon sanctuary",		new Point3D( 762, 1645,   0)),      // Sanctuary
			});

			private static readonly CCPList Felucca =
			    new CCPList(Map.Felucca, new CCPEntry[]
			{
				new CCPEntry("fel dungeon covetous",	new Point3D(2498,  920,   0)),      // Covetous
				new CCPEntry("fel dungeon deceit",	    new Point3D(4111,  433,   5)),      // Deceit
				new CCPEntry("fel dungeon despise",		new Point3D(1301, 1080,   0)),      // Despise
				new CCPEntry("fel dungeon destard",		new Point3D(1176, 2639,   0)),      // Destard
				new CCPEntry("fel dungeon ice",		    new Point3D(1999,   80,   4)),      // Ice
				new CCPEntry("fel dungeon fire",	    new Point3D(2923, 3410,   5)),      // Fire
				new CCPEntry("fel dungeon hythloth",	new Point3D(4721, 3823,   0)),      // Hythloth
				new CCPEntry("fel dungeon orc",	        new Point3D(1017, 1434,   0)),      // Orc Cave
				new CCPEntry("fel dungeon shame",		new Point3D( 513, 1563,   0)),      // Shame
				new CCPEntry("fel dungeon wrong",		new Point3D(2044,  237,  10)),      // Wrong
				new CCPEntry("fel dungeon wind",		new Point3D(1362,  896,   0)),      // Wind
				new CCPEntry("fel dungeon grove",	    new Point3D( 587, 1641,  -1)),      // Blighted Grove
				new CCPEntry("fel dungeon caves",	    new Point3D(1717, 2996,  -1)),      // Painted Caves
				new CCPEntry("fel dungeon prism",		new Point3D(3785, 1106,  20)),      // Prism of Light
				new CCPEntry("fel dungeon sanctuary",	new Point3D( 762, 1645,   0)),      // Sanctuary
			});

			private static readonly CCPList Malas =
			    new CCPList(Map.Malas, new CCPEntry[]
			{
				new CCPEntry("dungeon bedlam",          new Point3D(2079, 1375, -70)),      // Bedlam
				new CCPEntry("dungeon labyrinth",	    new Point3D(1732,  979, -80)),      // Labyrinth
				new CCPEntry("dungeon doom",		    new Point3D(2367, 1268, -85)),      // Doom
			});

			private static readonly CCPList Tokuno =
			    new CCPList(Map.Tokuno, new CCPEntry[]
			{
				new CCPEntry("dungeon citadel",		    new Point3D(1342,  770,  20)),      // Citadel
				new CCPEntry("dungeon fandancer",	    new Point3D( 977,  216,  23)),      // Fandancer
				new CCPEntry("dungeon mines",		    new Point3D( 258,  787,  64)),      // Yomotsu Mines
			});

			private static readonly CCPList TerMur =
			    new CCPList(Map.TerMur, new CCPEntry[]
			{
				new CCPEntry("dungeon underworld",	    new Point3D(4195, 3263,   5)),      // Underworld
				new CCPEntry("dungeon abyss",		    new Point3D( 997, 3843, -41))       // Abyss
			});

			public static readonly CCPList[] UORLists = new CCPList[] { Trammel, Felucca };
			public static readonly CCPList[] LBRLists = new CCPList[] { Trammel, Felucca };
			public static readonly CCPList[] AOSLists = new CCPList[] { Trammel, Felucca, Malas };
			public static readonly CCPList[] SELists  = new CCPList[] { Trammel, Felucca, Malas, Tokuno };
			public static readonly CCPList[] SALists  = new CCPList[] { Trammel, Felucca, Malas, Tokuno, TerMur };
		}
	}
}