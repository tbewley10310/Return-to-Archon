/***************************************************************************
 *                            CrystalPortal.cs
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
	public class CrystalPortal : BaseBook, Engines.VeteranRewards.IRewardItem
	{
		private Point3D m_PointDest;
		private Map m_MapDest;

		private bool m_IsRewardItem;

		private static readonly BookContent Content = new BookContent
		(
		    "Crystal Portal", "    unknown",
		    new BookPageInfo
		    (
		        "This crystal portal",
		        "allows you to teleport",
		        "directly to a bank or",
		        "a moongate.",
		        "For Trammel ruleset,",
		        "say the city's name",
		        "followed by \"mint\"",
		        "(e.g. \"minoc mint\")."
		    ),
		    new BookPageInfo
		    (
		        "For a moongate, say",
		        "the gate's name and",
		        "\"moongate\" (eg.",
		        "\"minoc moongate\").",
		        "For Felucca, say",
		        "\"fel\" then same",
		        "rules as above. So",
		        "\"fel minoc mint\", or",
		        "\"fel minoc moongate\"."
		    ),
		    new BookPageInfo
		    (
		        "CITY NAMES:",
		        "britain, bucs, cove,",
		        "delucia, haven, jhelom,",
		        "magincia, minoc,",
		        "moonglow, nujelm, ocllo,",
		        "papua, serpent, skara,",
		        "trinsic, vesper, wind,",
		        "yew, luna, umbra, zento,",
		        "termur, ilshenar"
		    ),
		    new BookPageInfo
		    (
		        "MOONGATE NAMES",
		        "moonglow, britain, jhelom,",
		        "yew, minoc, trinsic,",
		        "skara, magincia, haven,",
		        "bucs, vesper, compassion,",
		        "honesty, honor, humility,",
		        "justice, sacrifice,",
		        "spirituality, valor, chaos,"
		    ),
		    new BookPageInfo
		    (
		        "luna, umbra, isamu,",
		        "makoto, homare, termur",
		        " ",
		        "The same teleportation",
		        "rules apply regarding",
		        "criminal flagging,",
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
				return 1113945;    // crystal portal
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

		[Constructable]
		public CrystalPortal() : this(false)
		{
		}

		public CrystalPortal(bool isRewardItem) : base(0x468B)
		{
			LootType = LootType.Blessed;
			m_IsRewardItem = isRewardItem;
			Level = SecureLevel.Anyone;
			Title = "Crystal Portal";
			Author = "      unknown";
		}

		public CrystalPortal(Serial serial) : base(serial)
		{
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
			CPList[] checkLists;

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
						checkLists = CPList.SALists;
					}
					else if (Core.SE && (flags & ClientFlags.Tokuno) != 0)
					{
						checkLists = CPList.SELists;
					}
					else if (Core.AOS && (flags & ClientFlags.Malas) != 0)
					{
						checkLists = CPList.AOSLists;
					}
					else if ((flags & ClientFlags.Ilshenar) != 0)
					{
						checkLists = CPList.LBRLists;
					}
					else
					{
						checkLists = CPList.UORLists;
					}
				}
				else
				{
					checkLists = CPList.SALists;
				}

				bool isMatch = false;

				for (int i = 0; i < checkLists.Length; i++)
				{
					foreach (CPEntry entry in checkLists[i].Entries)
					{
						if (entry.Word == e.Speech.ToLower())
						{
							isMatch = true;
							m_MapDest = checkLists[i].Map;
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

		private class CPEntry
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

			public CPEntry(string word, Point3D loc)
			{
				m_Word = word;
				m_Location = loc;
			}
		}

		private class CPList
		{
			private Map m_Map;
			private CPEntry[] m_Entries;

			public Map Map
			{
				get
				{
					return m_Map;
				}
			}

			public CPEntry[] Entries
			{
				get
				{
					return m_Entries;
				}
			}

			public CPList(Map map, CPEntry[] entries)
			{
				m_Map = map;
				m_Entries = entries;
			}

			private static readonly CPList Trammel =
			    new CPList(Map.Trammel, new CPEntry[]
			{
				new CPEntry("moonglow moongate",	new Point3D(4467, 1283,   5)),     // Moonglow
				new CPEntry("britain moongate",		new Point3D(1336, 1997,   5)),     // Britain
				new CPEntry("jhelom moongate",		new Point3D(1499, 3771,   5)),     // Jhelom
				new CPEntry("yew moongate",         new Point3D( 771,  752,   5)),     // Yew
				new CPEntry("minoc moongate",		new Point3D(2701,  692,   5)),     // Minoc
				new CPEntry("trinsic moongate",     new Point3D(1828, 2948, -20)),     // Trinsic
				new CPEntry("skara moongate",		new Point3D( 643, 2067,   5)),     // Skara Brae
				new CPEntry("magincia moongate",	new Point3D(3563, 2139,  34)),     // Magincia
				new CPEntry("haven moongate",		new Point3D(3450, 2677,  25)),     // New Haven

				new CPEntry("moonglow mint",		new Point3D(4463, 1172,   0)),     // Moonglow
				new CPEntry("britain mint",         new Point3D(1431, 1696,   0)),     // Britain
				new CPEntry("jhelom mint",			new Point3D(1323, 3779,   0)),     // Jhelom
				new CPEntry("yew mint",             new Point3D( 644,  819,   0)),     // Yew
				new CPEntry("minoc mint",			new Point3D(2502,  563,   0)),     // Minoc
				new CPEntry("trinsic mint",         new Point3D(1822, 2827,   0)),     // Trinsic
				new CPEntry("skara mint",			new Point3D( 593, 2155,   0)),     // Skara Brae
				new CPEntry("magincia mint",		new Point3D(3730, 2161,  20)),     // Magincia
				new CPEntry("haven mint",	 		new Point3D(3497, 2572,  14)),     // New Haven
				new CPEntry("bucs mint",	 		new Point3D(2725, 2193,   0)),     // Buccaneer's Den
				new CPEntry("cove mint",	 		new Point3D(2235, 1198,   0)),     // Cove
				new CPEntry("nujelm mint",	 		new Point3D(3770, 1316,   0)),     // Nujelm
				new CPEntry("serpent mint",         new Point3D(2894, 3478,  15)),     // Serpent's Hold
				new CPEntry("vesper mint",	 		new Point3D(2899,  676,   0)),     // Vesper
				new CPEntry("papua mint",	 		new Point3D(5674, 3142,  12)),     // Papua
				new CPEntry("delucia mint",         new Point3D(5271, 3991,  37)),     // Delucia
				new CPEntry("wind mint",	 		new Point3D(5344,   92,  15))      // Wind
			});

			private static readonly CPList Felucca =
			    new CPList(Map.Felucca, new CPEntry[]
			{
				new CPEntry("fel moonglow moongate",	new Point3D(4467, 1283,   5)),     // Moonglow
				new CPEntry("fel britain moongate",     new Point3D(1336, 1997,   5)),     // Britain
				new CPEntry("fel jhelom moongate",		new Point3D(1499, 3771,   5)),     // Jhelom
				new CPEntry("fel yew moongate",         new Point3D( 771,  752,   5)),     // Yew
				new CPEntry("fel minoc moongate",		new Point3D(2701,  692,   5)),     // Minoc
				new CPEntry("fel trinsic moongate",     new Point3D(1828, 2948, -20)),     // Trinsic
				new CPEntry("fel skara moongate",		new Point3D( 643, 2067,   5)),     // Skara Brae
				new CPEntry("fel magincia moongate",	new Point3D(3563, 2139,  34)),     // Magincia
				new CPEntry("fel bucs moongate",		new Point3D(2711, 2234,   0)),     // Buccaneer's Den

				new CPEntry("fel moonglow mint",		new Point3D(4463, 1172,   0)),     // Moonglow
				new CPEntry("fel britain mint",         new Point3D(1431, 1696,   0)),     // Britain
				new CPEntry("fel jhelom mint",			new Point3D(1323, 3779,   0)),     // Jhelom
				new CPEntry("fel yew mint",             new Point3D( 644,  819,   0)),     // Yew
				new CPEntry("fel minoc mint",			new Point3D(2502,  563,   0)),     // Minoc
				new CPEntry("fel trinsic mint",         new Point3D(1822, 2827,   0)),     // Trinsic
				new CPEntry("fel skara mint",			new Point3D( 593, 2155,   0)),     // Skara Brae
				new CPEntry("fel magincia mint",		new Point3D(3730, 2161,  20)),     // Magincia
				new CPEntry("fel ocllo mint",	 		new Point3D(3689, 2523,   0)),     // Ocllo
				new CPEntry("fel bucs mint",	 		new Point3D(2725, 2193,   0)),     // Buccaneer's Den
				new CPEntry("fel cove mint",	 		new Point3D(2235, 1198,   0)),     // Cove
				new CPEntry("fel nujelm mint",	 		new Point3D(3770, 1316,   0)),     // Nujelm
				new CPEntry("fel serpent mint",         new Point3D(2894, 3478,  15)),     // Serpent's Hold
				new CPEntry("fel vesper mint",	 		new Point3D(2899,  676,   0)),     // Vesper
				new CPEntry("fel papua mint",	 		new Point3D(5674, 3142,  12)),     // Papua
				new CPEntry("fel delucia mint",         new Point3D(5271, 3991,  37)),     // Delucia
				new CPEntry("fel wind mint",	 		new Point3D(5344,   92,  15))      // Wind
			});

			private static readonly CPList Ilshenar =
			    new CPList(Map.Ilshenar, new CPEntry[]
			{
				new CPEntry("compassion moongate",		new Point3D(1215,  467, -13)),     // Compassion
				new CPEntry("honesty moongate",         new Point3D( 722, 1366, -60)),     // Honesty
				new CPEntry("honor moongate",			new Point3D( 744,  724, -28)),     // Honor
				new CPEntry("humility moongate",		new Point3D( 281, 1016,   0)),     // Humility
				new CPEntry("justice moongate",         new Point3D( 987, 1011, -32)),     // Justice
				new CPEntry("sacrifice moongate",		new Point3D(1174, 1286, -30)),     // Sacrifice
				new CPEntry("spirituality moongate",	new Point3D(1532, 1340,  -3)),     // Spirituality
				new CPEntry("valor moongate",			new Point3D( 528,  216, -45)),     // Valor
				new CPEntry("chaos moongate",			new Point3D(1721,  218,  96)),     // Chaos

				new CPEntry("ilshenar mint",	 		new Point3D( 849,  674, -40))      // Ilshenar
			});

			private static readonly CPList Malas =
			    new CPList(Map.Malas, new CPEntry[]
			{
				new CPEntry("luna moongate",			new Point3D(1015,  527, -65)),     // Luna
				new CPEntry("umbra moongate",			new Point3D(1997, 1386, -85)),     // Umbra

				new CPEntry("luna mint",				new Point3D( 991,  519, -50)),     // Luna
				new CPEntry("umbra mint",				new Point3D(2057, 1343, -85))      // Umbra
			});

			private static readonly CPList Tokuno =
			    new CPList(Map.Tokuno, new CPEntry[]
			{
				new CPEntry("isamu moongate",			new Point3D(1169,  998,  41)),     // Isamu-Jima
				new CPEntry("makoto moongate",			new Point3D( 802, 1204,  25)),     // Makoto-Jima
				new CPEntry("homare moongate",			new Point3D( 270,  628,  15)),     // Homare-Jima

				new CPEntry("zento mint",				new Point3D( 735, 1255,  30))      // Zento
			});

			private static readonly CPList TerMur =
			    new CPList(Map.TerMur, new CPEntry[]
			{
				new CPEntry("termur moongate",			new Point3D(850, 3525, -38)),      // Royal City

				new CPEntry("termur mint",				new Point3D(847, 3450, -20))       // Royal City
			});

			public static readonly CPList[] UORLists = new CPList[] { Trammel, Felucca };
			public static readonly CPList[] LBRLists = new CPList[] { Trammel, Felucca, Ilshenar };
			public static readonly CPList[] AOSLists = new CPList[] { Trammel, Felucca, Ilshenar, Malas };
			public static readonly CPList[] SELists  = new CPList[] { Trammel, Felucca, Ilshenar, Malas, Tokuno };
			public static readonly CPList[] SALists  = new CPList[] { Trammel, Felucca, Ilshenar, Malas, Tokuno, TerMur };
		}
	}
}