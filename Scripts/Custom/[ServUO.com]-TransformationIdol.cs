using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

// Po0ka - April 2016
// Version 1.0.1

namespace Server.Items
{
	public class TransformationIdol : Item
	{
		private static int m_TransformSound = 501;

		private string m_StatueName;
		private int m_BodyMod;

		[CommandProperty( AccessLevel.GameMaster )]
		public string StatueName
		{
			get { return m_StatueName; }
			set { m_StatueName = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int BodyMod
		{
			get { return m_BodyMod; }
			set { m_BodyMod = value; }
		}

		public override string DefaultName
		{
			get { return String.Format( "{0} idol", m_StatueName ); }
		}

		[Constructable]
		public TransformationIdol( int itemid, string name, int body ) : base( itemid )
		{
			m_StatueName = name;
			m_BodyMod = body;

			ItemID = itemid;
			Movable = true;
		}

		public TransformationIdol( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (string) m_StatueName );
			writer.Write( (int) m_BodyMod );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_StatueName = reader.ReadString();
			m_BodyMod = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from.Mounted )
			{
				from.SendLocalizedMessage( 1042561 ); // Please dismount first.
			}
			else if ( Factions.Sigil.ExistsOn( from ) )
			{
				from.SendLocalizedMessage(1010521); // You cannot polymorph while you have a Town Sigil
			}
			else if ( TransformationSpellHelper.UnderTransformation( from ) )
			{
				from.SendLocalizedMessage(1061633); // You cannot polymorph while in that form.
			}
			else if ( DisguiseTimers.IsDisguised( from ) )
			{
				from.SendLocalizedMessage(502167); // You cannot polymorph while disguised.
			}
			else if (from.BodyMod == 183 || from.BodyMod == 184)
			{
				from.SendLocalizedMessage(1042512); // You cannot polymorph while wearing body paint
			}
			else
			{
				if (from.BodyMod != 0)
					from.BodyMod = 0;
				else
					from.BodyMod = m_BodyMod;

				from.PlaySound( m_TransformSound );
				from.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
			}
		}
	}

	public class OgreIdol : TransformationIdol
	{
		[Constructable]
		public OgreIdol() : base( 0x20DF, "ogre", 1 )
		{
		}
		
		public OgreIdol( Serial serial ) : base( serial )
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
	
	public class EtherealWarriorIdol : TransformationIdol
	{
		[Constructable]
		public EtherealWarriorIdol() : base( 0x2589, "ethereal warrior", 123 )
		{
		}
		
		public EtherealWarriorIdol( Serial serial ) : base( serial )
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
	
	public class PixieIdol : TransformationIdol
	{
		[Constructable]
		public PixieIdol() : base( 0x25B6, "pixie", 128 )
		{
		}
		
		public PixieIdol( Serial serial ) : base( serial )
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
	
	public class WispIdol : TransformationIdol
	{
		[Constructable]
		public WispIdol() : base( 0x2100, "wisp", 58 )
		{
		}
		
		public WispIdol( Serial serial ) : base( serial )
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
	
	public class GazerIdol : TransformationIdol
	{
		[Constructable]
		public GazerIdol() : base( 0x258F, "gazer", 22 )
		{
		}
		
		public GazerIdol( Serial serial ) : base( serial )
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
	
	public class SphinxIdol : TransformationIdol
	{
		[Constructable]
		public SphinxIdol() : base( 0x2618, "sphinx", 788 )
		{
		}
		
		public SphinxIdol( Serial serial ) : base( serial )
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
	
	public class OrcIdol : TransformationIdol
	{
		[Constructable]
		public OrcIdol() : base( 0x25AE, "orc", 17 )
		{
		}
		
		public OrcIdol( Serial serial ) : base( serial )
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
	
	public class RatmanIdol : TransformationIdol
	{
		[Constructable]
		public RatmanIdol() : base( 0x25B7, "ratman", 42 )
		{
		}
		
		public RatmanIdol( Serial serial ) : base( serial )
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
	
	public class ZombieIdol : TransformationIdol
	{
		[Constructable]
		public ZombieIdol() : base( 0x25B8, "zombie", 3 )
		{
		}
		
		public ZombieIdol( Serial serial ) : base( serial )
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
	
	public class LichIdol : TransformationIdol
	{
		[Constructable]
		public LichIdol() : base( 0x25A4, "lich", 24 )
		{
		}
		
		public LichIdol( Serial serial ) : base( serial )
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
	
	public class SkeletonIdol : TransformationIdol
	{
		[Constructable]
		public SkeletonIdol() : base( 0x25BC, "skeleton", 50 )
		{
		}
		
		public SkeletonIdol( Serial serial ) : base( serial )
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

	// TODO: Check for ones you would like.
	#region MobsWithShrink
	/*
		1	0x20DF	ogre
		2	0x20D8	ettin
		3	0x25B8	zombie
		4	0x20D9	gargoyle
		5	0x211D	eagle
		6	0x211A	bird
		7	0x25AF	orcish lord
		8	0x2582	corpser
		9	0x2584	daemon
		10	0x2584	daemon - AOS
		11	0x25C4	dread spider
		12	0x20D6	dragon brown
		13	0x25D7	air elemental
		14	0x20D7	earth elemental
		15	0x25D9	fire elemental
		16	0x25DD	water elemental
		17	0x25AE	orc
		18	0x20D8	ettin mace
		19	0x25C4	dread spider
		20	0x25C5	frost spider
		21	0x25BF	giant serpent
		22	0x258F	gazer
		23	0x25D0	dire wolf
		24	0x25A4	lich
		25	0x25D1	grey wolf
		26	0x25C7	shade
		27	0x25D1	grey wolf
		28	0x25C6	giant spider
		29	0x2592	gorilla
		30	0x20DC	harpy
		31	0x2595	headless one
		33	0x20DE	lizardman fist
		34	0x25D2	
		35	0x20DE	lizardman spear
		36	0x20DE	lizardman mace
		37	0x25D2	white wolf
		38	0x2585	black daemon
		39	0x25A6	mongbat
		40	0x2586	balron
		41	0x25AE	orc mace
		42	0x25B7	ratman fists
		43	0x2587	ice fiend
		44	0x25B7	ratman mace
		45	0x25B7	ratman sword
		46	0x20D6	ancient wyrm
		47	0x20FA	reaper
		48	0x25B9	scorpion
		49	0x20D6	white wyrm
		50	0x25BC	skeleton
		51	0x20E8	slime
		52	0x25C2	snake
		53	0x20E9	troll axe
		54	0x20E9	troll fists
		55	0x20E9	frost troll axe
		56	0x25BC	skeleton axe
		57	0x25BC	skeleton knight
		58	0x2100	wisp
		59	0x20D6	dragon red
		60	0x20D6	drake brown
		61	0x20D6	drake red
		62	0x20D6	wyvern
		63	0x2583	cougar
		64	0x25A3	cougar?
		65	0x25A3	cougar?
		66	0x25C8	swamp tentacles
		67	0x258E	stone gargoyle
		68	0x258F	gazer
		69	0x258F	gazer
		70	0x25CC	terathan warrior
		71	0x25C9	terathan drone
		72	0x25CB	terathan matriarch
		73	0x2594	stone harpy
		74	0x259F	imp
		75	0x212D	cyclops hammer
		76	0x25CD	titan
		77	0x25A2	kraken
		78	0x25A5	ancient lich
		79	0x25A5	lich
		80	0x258C	giant bullfrog
		81	0x2130	bullfrog
		82	0x25A5	lich
		83	0x25A8	ogre
		84	0x25A8	ogre
		85	0x25AB	ophidian mage
		86	0x25AD	ophidian warrior
		87	0x25AC	ophidian matriarch
		88	0x2108	goat
		89	0x25C0	giant ice serpent
		90	0x25C1	giant lava serpent
		91	0x25C2	silver serpent
		92	0x25C2	silver serpent
		93	0x25C2	silver serpent
		94	0x20E8	frost ooze
		96	0x20E8	frost ooze
		97	0x2596	hellhound
		98	0x2596	hellhound
		99	0x25D1	dire wolf?
		100	0x25D2	white wolf
		101	0x2581	centaur
		102	0x2584	exodus
		103	0x20D6	serpentine dragon
		104	0x20D6	skeletal dragon
		105	0x20D6	greater dragon?
		106	0x20D6	ethereal dragon
		107	0x20D7	copper elemental
		108	0x20D7	bronze elemental
		109	0x20D7	gold elemental
		110	0x20D7	dull copper elemental
		111	0x20D7	shadow iron elemental
		112	0x20D7	valorite elemental
		113	0x20D7	verite elemental
		114	0x259B	nightmare
		115	0x259B	ethereal horse
		116	0x259C	nightmare
		117	0x259D	silver horse
		118	0x259E	war horse
		119	0x259E	war horse
		120	0x259E	war horse
		121	0x259E	war horse
		122	0x25CE	unicorn
		123	0x2589	ethereal warrior
		124	0x258A	evil mage
		125	0x258B	evil mage lord
		126	0x258B	evil mage lord
		127	0x2597	predator hellcat
		128	0x25B6	pixie
		129	0x25C8	harrower tentacles
		130	0x258D	fire gargoyle
		131	0x2590	efreet
		132	0x25A0	kirin
		133	0x2131	alligator
		134	0x25A1	lava lizard
		135	0x25A8	arctic ogre lord
		136	0x25A9	ophidian mage
		137	0x25AA	ophidian warrior
		138	0x25AF	orcish lord
		139	0x25AF	orcish lord
		140	0x25B1	orc
		141	0x2106	human male
		142	0x25B7	ratman archer
		143	0x25B7	ratman mage
		144	0x25BA	sea horse
		145	0x20FB	sea serpent
		146	0x25BB	harrower
		147	0x25BD	skeleton knight
		148	0x25BE	skeleton mage
		rip succubus.
		150	0x20FB	sea serpent
		151	0x20F1	dolphin
		152	0x25CA	terathan avenger
		153	0x2591	ghoul
		154	0x25A7	mummy
		155	0x20EC	rotting corpse
		157	0x25C3	black spider
		158	0x25D6	acid elemental
		159	0x25D8	blood elemental
		160	0x25D8	blood elemental
		161	0x25DA	ice elemental
		162	0x25DB	poison elemental
		163	0x25DC	snow elemental
		164	0x20ED	energy vortex
		165	0x2100	shadow wisp
		166	0x20D7	gold elemental
		167	0x2118	brown bear
		168	0x259F	shadow fiend
		170	0x20F6	ethereal llama
		171	0x25B2	ethereal ostard
		172	0x20D6	rikktor
		173	0x25C3	mephitis
		rip semidar
		175	0x2589	lord oak
		176	0x25B6	silvani
		177	0x259C	nightmare
		178	0x259C	nightmare
		179	0x259C	nightmare
		180	0x20D6	white wyrm
		181	0x25AE	orc scout
		182	0x25AE	orc brute
		183	0x2106	male
		184	0x2107	female
		185	0x2106	male
		186	0x2107	female
		187	0x2615	ridgeback
		188	0x2615	savage ridgeback
		189	0x25B0	orc brute
		190	0x21F1	fire steed
		191	0x25A0	kirin
		192	0x25CE	unicorn
		193	0x2615	ridgeback
		194	0x2619	swamp dragon
		195	0x260F	giant beetle
		197	0x20D6	kaze kimono
		198	0x20D6	- crash
		200	0x2598	horse beige
		201	0x211B	cat
		202	0x2131	alligator
		203	0x2101	pig
		204	0x259A	horse brown
		205	0x2125	rabbit
		206	0x2131	lava lizard
		207	0x20EB	sheep
		208	0x20D1	chicken
		209	0x2580	goat
		210	0x25B2	desert ostard
		211	0x2118	black bear
		212	0x211E	grizzly bear
		213	0x20E1	polar bear
		214	0x25B5	panther
		215	0x20D0	giant rat
		216	0x2103	cow
		217	0x2588	dog
		218	0x25B3	frenzied ostard
		219	0x25B4	forest ostard
		220	0x20F6	llama
		221	0x20FF	walrus
		223	0x20EB	sheep shaved
		225	0x25D3	timber wolf
		226	0x2599	horse grey
		228	0x259E	horse beige wild
		231	0x20EF	cow brown
		232	0x20EF	bull brown
		233	0x20F0	bull pattern
		234	0x20D4	great hart
		237	0x20D4	hind
		238	0x2123	rat
		290	0x2101	boar
		291	0x2126	pack horse
		292	0x2127	pack llama
		300	0x2620	crystal elemental
		301	0x2621	tree fellow
		302	0x2622	skittering hopper
		303	0x2623	devourer
		304	0x2624	flesh golem
		305	0x2625	gore fiend
		306	0x2626	impaler
		307	0x2627	gibberling
		308	0x2628	bone demon
		309	0x2629	patchwork skeleton
		310	0x262A	wailing banshee
		311	0x262B	shadow knight
		312	0x262C	abysmal horror
		313	0x262D	darknight creeper
		314	0x262E	ravager
		315	0x262F	flesh renderer
		316	0x2630	void wanderer
		317	0x2631	vampire bat
		318	0x2632	demon knight
		319	0x2633	mound of maggots
		400	0x2106	human male
		401	0x2107	human female
		402	0x2106	
		403	0x2107	
		744	0x2106	
		745	0x2107	
		746	0x260E	moloch
		747	0x262A	wailing banshee
		748	0x25C7	spikey ghoul
		749	0x25A4	lich
		750	0x2106	golem controller male
		751	0x2107	golem controller female
		752	0x2610	golem
		753	0x258D	gargoyle enforcer?
		754	0x258D	gargoyle enforcer?
		755	0x258D	gargoyle destroyer
		756	0x260C	small exodus overseer
		757	0x260B	big exodus overseer
		758	0x258D	gargoyle vendor
		763	0x260B	giant exodus overseer
		764	0x261F	juka warrior
		765	0x261E	juka mage
		766	0x25FC	juke lord
		767	0x25F9	betrayer
		768	0x2612	juggernaut
		769	0x25F9	blacktorn
		770	0x261C	meer mage
		771	0x261D	meer warrior
		772	0x25F8	meer eternal
		773	0x25FA	meer captain
		774	0x25FB	meer knight
		775	0x2613	plague beast
		776	0x2611	horde minion
		777	0x260D	doppleganger
		778	0x261A	gazer larva
		779	0x2607	bogling
		780	0x2608	bogthing
		781	0x2604	ant worker red
		782	0x2603	ant warrior red
		783	0x2602	ant queen red
		784	0x2605	arcane daemon
		785	0x260E	moloch
		786	0x260A	
		787	0x260F	antlion
		788	0x2618	sphinx
		789	0x2614	quagmire
		790	0x2616	sand vortex
		791	0x260F	giant beetle
		792	0x2609	chaos daemon
		793	0x2617	hellsteed
		794	0x2619	swamp dragon
		795	0x2611	horde minion medium
		796	0x2611	horde minion giant
		797	0x20D6	rikktor
		798	0x20D6	ancient wyrm
		799	0x2619	swamp dragon armored
		804	0x2602	ant queen red
		805	0x2604	ant worker red
		806	0x2603	ant warrior red
		807	0x2602	ant queen red
		808	0x2602	ant queen red
		820	0x259A	
		825	0x25B2	
		826	0x25B3	
		827	0x25B4	
		828	0x20F6	
		831	0x211A	
		832	0x211A	
		833	0x211A	
		834	0x2597	
		835	0x2125	
		837	0x25C0	
		838	0x25C1	
		839	0x2615	
		840	0x2615	
		841	0x2615	
		842	0x2615	
		843	0x2123	
		846	0x2599	
		848	0x259E	
		970	0x2106	shroud
		990	0x2106	british
		991	0x2106	blacktorn
		994	0x2106	dupre
		998	0x25A0	

		#SE Mobiles

		169	0x281C	giant beetle
		196	0x2769	kaze kimono
		199	0x2766	electric elemental thing
		240	0x276B	kappa
		241	0x276D	oni
		242	0x2765	beetle hatchling
		243	0x276A	hiryu
		244	0x276F	rune bettle
		245	0x281B	yomotsu warrior
		246	0x2763	bake kitsune
		247	0x2767	fan dancer
		248	0x2768	gaman
		249	0x2771	yamandon
		250	0x2770	tsuki wolf
		251	0x276E	revenant lion
		252	0x276C	lady snow
		253	0x2773	yomotsu priest
		254	0x2764	crane
		255	0x2772	yomotsu elder

		#ML Mobiles

		276	0x2D95	chimera
		277	0x2D96	cu sidhe

		#SA Mobiles

		1254 0x9844
		1255 0x9844
	*/
	#endregion
}