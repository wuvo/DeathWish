using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewestCOServer.Game;

namespace NewestCOServer.PacketHandling
{

    public unsafe struct WhoIsPacket
    {

        public int Version, Id, Hair, Action, Level, Strength, Vitality, Agility, Spirit, Profession, Reborn, Lookface;
        public NetStringPacker Spells, Items;
        public int Server;


        private fixed sbyte name[16];

        public string Name
        {
            get { fixed (sbyte* ptr = name) return new string(ptr, 0, 16, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = name)
                {
                    MSVCRT.memset(ptr, 0, 16);
                    value.CopyTo(ptr);
                }
            }
        }

        public static WhoIsPacket Create(Character player)
        {
            WhoIsPacket packet = new WhoIsPacket();
            packet.Version = 5165;
            packet.Id = (int)player.EntityID;
            packet.Name = player.Name;
            packet.Hair = player.Hair;
            packet.Profession = player.Job;
            packet.Lookface = (int)player.Mesh;
            packet.Vitality = player.Vit;
            packet.Strength = player.Str;
            packet.Agility = player.Agi;
            packet.Spirit = player.Spi;
            packet.Level = player.Level;            
            packet.Spells = new NetStringPacker();
            packet.Server = 6;//This is the Key I'm assigning Shannara Conquer
            foreach (var spell in player.Skills.Values)
                packet.Spells.AddString(spell.ID + " " + spell.Lvl);
            if (player.Skills.Count == 0)
                packet.Spells.AddString("1000 0");
            packet.Items = new NetStringPacker();

            //well thats messy but w/e

            //They get new gear on interserver, lets just not send for now so they enter naked

            //This is where you'd enter their rank from your server. Tickets bought for war, duels won, anything you want to determine their guild rank on interserver
            //It auto adjusts based on who is online. New user logs in = it recalculates who is guild leader
            packet.Items.AddString("LotteryTicket " + (player.Nobility.Donation + 1));
            return packet;
        }
        public static implicit operator byte[] (WhoIsPacket packet)
        {
            int len = 72 + packet.Spells.Length + packet.Items.Length;
            var buffer = new byte[len];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 6969);
                *((int*)(ptr + 4)) = packet.Version;
                *((int*)(ptr + 8)) = packet.Id;
                *((int*)(ptr + 12)) = packet.Hair;
                *((int*)(ptr + 16)) = packet.Action;
                *((int*)(ptr + 20)) = packet.Level;
                *((int*)(ptr + 24)) = packet.Strength;
                *((int*)(ptr + 28)) = packet.Agility;
                *((int*)(ptr + 32)) = packet.Vitality;
                *((int*)(ptr + 36)) = packet.Spirit;
                *((int*)(ptr + 40)) = packet.Profession;
                *((int*)(ptr + 44)) = packet.Reborn;
                *((int*)(ptr + 48)) = packet.Lookface;
                *((int*)(ptr + 52)) = packet.Server;
                MSVCRT.memcpy(ptr + 56, packet.name, 16);
                PacketBuilder.AppendNetStringPacker(ptr + 72, packet.Spells);
                PacketBuilder.AppendNetStringPacker(ptr + 72 + packet.Spells.Length, packet.Items);
            }
            return buffer;
        }
    }
}
