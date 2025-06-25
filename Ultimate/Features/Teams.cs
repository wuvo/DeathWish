using System;
using System.Collections;
using Ultimate.Game;
namespace Ultimate.Features
{
    public class Team
    {
        public ArrayList Members = new ArrayList(6);
        public bool Items = false;
        public bool Money = false;
        public bool Forbid = false;
        public bool PKTournyAlive = true;
        public Team(Character C)
        {
            Members.Add(C);
            C.TeamLeader = true;
            C.MyClient.AddSend(Packets.TeamPacket(C.EntityID, 0));
            C.StatEff.Add(StatusEffectEn.TeamLeader);
        }
        public void LeaderCoords()
        {
            try
            {
                Character _Leader = Leader;
                if (_Leader != null && _Leader.Loc.X >= 0 && _Leader.Loc.Y >= 0 && _Leader.Loc.Map >= 0)
                {
                    if (_Leader.Loc.Map > 0 && _Leader.Loc.Map < 9999 && _Leader.Loc.X > 0 && _Leader.Loc.X < 2000 && _Leader.Loc.Y > 0 && _Leader.Loc.Y < 2000)
                    {
                        COPacket P = Packets.GeneralData(0, _Leader.EntityID, _Leader.Loc.X, _Leader.Loc.Y, 101);
                        foreach (Character Member in Members)
                            if (Member != null && Member.MyClient != null && Member != _Leader && Member.Loc.Map == _Leader.Loc.Map)
                                Member.MyClient.AddSend(P);
                    }
                }
            }
            catch (Exception e) { Program.WriteLine(e); }
        }
        public void Message(Character C, COPacket Data)
        {
            try
            {
                if (Members.Contains(C))
                {
                    foreach (Character P in Members)
                        if (P != C)
                        {
                            P.MyClient.AddSend(Data);
                        }
                }
            }
            catch (Exception e) { Program.WriteLine(e); }
        }
        public void Message(COPacket Data)
        {
            try
            {
                foreach (Character P in Members)
                {
                    P.MyClient.AddSend(Data);
                }
            }
            catch (Exception e) { Program.WriteLine(e); }
        }
        public Character Leader
        {
            get
            {
                foreach (Character C in Members)
                    if (C != null)
                        if (C.MyClient != null)
                            if (C.TeamLeader)
                                return C;
                return null;
            }
        }
        public void Dismiss(Character C)
        {
            try
            {
                if (C == Leader)
                {
                    C.StatEff.Remove(StatusEffectEn.TeamLeader);
                    foreach (Character P in Members)
                    {
                        if (P.MyClient != null)
                        {
                            P.MyClient.AddSend(Packets.TeamPacket(C.EntityID, 6));
                            P.MyTeam = null;
                        }
                    }
                    if (World.Archers.ContainsKey(C.EntityID))
                    {
                        World.Archers.Remove(C.EntityID);
                    }
                    C.TeamLeader = false;
                }
            }
            catch (Exception e) { Program.WriteLine(e); }
        }
        public bool Joins(Character C)
        {
            try
            {
                if (Members.Count < 6 && !Members.Contains(C))
                {
                    foreach (Character P in Members)
                    {
                        if (P.MyClient != null)
                        {
                            P.MyClient.AddSend(Packets.PlayerJoinsTeam(C));
                            C.MyClient.AddSend(Packets.PlayerJoinsTeam(P));
                        }
                    }
                    Members.Add(C);
                    C.MyClient.AddSend(Packets.PlayerJoinsTeam(C));
                    C.MyTeam = this;
                    return true;
                }
                return false;
            }
            catch (Exception e) { Program.WriteLine(e); return false; }
        }
        public void Leaves(Character C)
        {
            try
            {
                foreach (Character P in Members)
                    if (P.MyClient != null)
                    {
                        P.MyClient.AddSend(Packets.TeamPacket(C.EntityID, 2));
                    }
                C.MyClient.AddSend(Packets.TeamPacket(C.EntityID, 6));
                Members.Remove(C);
                C.MyTeam = null;
            }
            catch (Exception e) { Program.WriteLine(e); }
        }
    }
}