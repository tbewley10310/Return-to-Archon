//
//	Santa Quest 2013 - version 2.0, by Arachaic
//
using System;using Server;namespace Server.Items
{
public class AstroDaemonDoll : Item
{
[Constructable]
public AstroDaemonDoll() : this( 1 )
{}
[Constructable]
public AstroDaemonDoll( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
{}
[Constructable]
public AstroDaemonDoll( int amount ) : base( 9606 )
{
Stackable = false;
Weight = 3.00;
Amount = amount;
Name = "Limited Edition Astro Daemon Doll";
LootType = LootType.Blessed;
Hue = 1175;
}
public AstroDaemonDoll( Serial serial ) : base( serial )
{}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 ); // version
}
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt(); }}}
