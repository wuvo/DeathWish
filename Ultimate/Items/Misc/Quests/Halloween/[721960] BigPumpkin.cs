using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721960 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (!World.NoPKMaps.Contains(C.Loc.Map))
            {
                C.RemoveItem(I.UID);
                Mob M = new Mob()
                {
                    LastMove = DateTime.Now,
                    Level = 70,
                    MaxHP = 30000,
                    CurrentHP = 30000,
                    Defense = 3000,
                    MDef = 0,
                    MAttack = 0,
                    MinAttack = 1500,
                    MaxAttack = 3000,
                    Type = MobBehaveour.HuntPlayers,
                    DmgReduceTimes = 1,
                    Dodge = 36,
                    AtkType = AttackType.Melee,
                    Gives = true,
                    AttackDist = 2,
                    MinSilvers = 1,
                    MaxSilvers = 50,
                    MoveSpeed = 1000,
                    SpawnSpeed = 0,
                    Name = "PumpkinKing",
                    Mesh = 282,
                    MobID = 809
                };
                M.Loc.Map = C.Loc.Map;
                M.StartLoc.XFrom = (ushort)(C.Loc.X + 5);
                M.StartLoc.YFrom = (ushort)(C.Loc.Y + 5);
                M.StartLoc.XTo = (ushort)(C.Loc.X + 5);
                M.StartLoc.Yto = (ushort)(C.Loc.Y + 5);
                M.AddMob();
            }
            else
                C.MyClient.LocalMessage(2005, "You can't spawn monsters in this map!");
        }
    }
}