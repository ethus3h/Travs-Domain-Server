﻿using System.Collections.Generic;
using System.Linq;
using wServer.networking.svrPackets;
using wServer.realm.terrain;

namespace wServer.realm.entities
{
  public partial class Player
  {
    private bool OxygenRegen;
    private long b;

    private void HandleGround(RealmTime time)
    {
      WmapTile tile = Owner.Map[(int)X, (int)Y];
      TileDesc tileDesc = Manager.GameData.Tiles[tile.TileId];
      if (time.tickTimes - b > 100)
      {
        if (Owner.Name != "The Void")
        {
          if (tileDesc.NoWalk)
          {
            if (time.tickCount % 30 == 0)
            {
              client.Disconnect();
            }
          }
        }

        if (Owner.Name == "Ocean Trench")
        {
          bool fObject = false;
          foreach (
              KeyValuePair<int, StaticObject> i in
                  Owner.StaticObjects.Where(i => i.Value.ObjectType == 0x0731)
                      .Where(i => (X - i.Value.X) * (X - i.Value.X) + (Y - i.Value.Y) * (Y - i.Value.Y) < 1))
            fObject = true;

          OxygenRegen = fObject;

          if (!OxygenRegen)
          {
            if (OxygenBar == 0)
              HP -= 2;
            else
              OxygenBar -= 1;

            UpdateCount++;

            if (HP <= 0)
              Death("server.damage_suffocation");
          }
          else
          {
            if (OxygenBar < 100)
              OxygenBar += 8;
            if (OxygenBar > 100)
              OxygenBar = 100;

            UpdateCount++;
          }
        }

        Owner.TileEvent(this, tile);

        b = time.tickTimes;
      }
    }

    public void CheckGroundOnMove()
    {
      WmapTile tile = Owner.Map[(int)X, (int)Y];
      TileDesc tileDesc = Manager.GameData.Tiles[tile.TileId];
      if (tile.Region == TileRegion.Encounter_1 || tile.Region == TileRegion.Encounter_2 || tile.Region == TileRegion.Encounter_3 || tile.Region == TileRegion.Encounter_4)
      {
        string[] pokemons = new string[] { };
        switch(tile.Region)
        {
          case TileRegion.Encounter_1:
            pokemons = new string[] { "Caterpie", "Weedle", "Kakuna", "Metapod" };
            break;
          case TileRegion.Encounter_2:
            pokemons = new string[] { "Pidgey", "Rattata" };
            break;
          case TileRegion.Encounter_3:
            pokemons = new string[] { "Bulbasaur", "Squirtle", "Charmander" };
            break;
          case TileRegion.Encounter_4:
            pokemons = new string[] { "Venusaur", "Blastoise", "Charizard" };
            break;
        }
        wRandom pokerand = new wRandom();
        if (pokerand.Next(0, 10) == 0)
        {
          Client.SendPacket(new EncounterStartPacket()
          {
            Pokemon = pokemons[pokerand.Next(0, pokemons.Length)]
          });
        }
      }
    }
  }
}