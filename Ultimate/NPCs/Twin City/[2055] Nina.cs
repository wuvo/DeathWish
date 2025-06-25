using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public enum BroadCastLoc
    {
        World,
        Map,
        Score,
        Title
    }

    public class NPC_2055 : NPCBase
    {

        public NPC_2055(Main.GameClient _client)
            : base(_client)
        {
            //12 and 108
            ID = 2055;
            Face = 112;
        }
            public void Broadcast(string msg, BroadCastLoc loc, uint index = 0, ushort _chatType = 2011)
    {
        if (loc == BroadCastLoc.World)
            World.SendMsgToAll("[System]", msg, 2005, 0);

        else if (loc == BroadCastLoc.Map)
        {
            foreach (Character C in PlayerList.Values.ToList())
                C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, _chatType, 0U));
        }
        else if (loc == BroadCastLoc.Score)
        {
            foreach (Character C in PlayerList.Values.ToList())
                C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83d, 0));
        }
        else if (loc == BroadCastLoc.Title)
        {
            foreach (Character C in PlayerList.Values.ToList())
                C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83c, 0));
        }
    }
        public Dictionary<uint, Character> PlayerList = new Dictionary<uint, Character>();
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            try
            {
                switch (_linkback)
                {
                    //case 0:
                    //    AddText("A true warrior is wrought by practice! Don't ever forget that! Do you wish to duel other players?");
                    //    AddOption("Challenge Player", 1);
                    //    AddOption("Just passing by", 254);
                    //    break;
                    case 0:
                        AddText("What kind of duel do you want to challenge a player for?");
                        AddOption("Free Duel", 2);
                        AddOption("Bet Duel", 3);
                        AddOption("Watch Duel", 14);
                        AddOption("Nevermind", 254);
                        break;
                    case 14:
                        AddText("You need a door number. If you know door number you can write here and  you can watch your friends..");
                        AddInput("DoorNumber:", 15);
                        AddOption("Nevermind", 254);
                        break;

                    case 15:
                        uint MapID = Convert.ToUInt32(ReadString(_data));
                        {
                            if (DMaps.EventMaps.ContainsKey(MapID))
                            {
                                Broadcast($"{GC.MyChar.Name} - entered the arena to watch you. Enjoy the show..", BroadCastLoc.World);
                                GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                GC.MyChar.Teleport(MapID, 50, 50);
                                Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                GC.MyChar.Invisible = true;
                                GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                            }
                                else
                            {
                                AddText("No such room number. Please write right door number..");
                                AddOption("Thanks.", 254);
                            }

                        }
                        break;

                    case 2:
                        GC.MyChar.Arena = new Features.Arena();
                        AddText("In a Free duel, players will just be fighting for fun meaning that none of the parties will lose anything. Which kind of duel do you want to choose?");
                        AddOption("Single", 5);
                        AddOption("Team Based", 6);
                        AddOption("Nevermind", 254);
                        break;
                    case 3:
                        GC.MyChar.Arena = new Features.Arena();
                        GC.MyChar.Arena.Wager = true;
                        AddText("When dueling with a bet, each player will bet a bet on themselves. How much would you like to place as a bet?");
                        AddInput("Amount:", 4);
                        AddOption("Nevermind", 254);
                        break;
                    case 4:
                        uint Amount;
                        bool canDo = uint.TryParse(ReadString(_data), out Amount);
                        //uint Amount = Convert.ToUInt32(ReadString(_data));
                        if (GC.MyChar.Silvers >= Amount && Amount > 0)
                        {
                            if (Amount * 2 + GC.MyChar.Silvers >= 2000000000)
                            {
                                AddText("That's too much gold! Please input a lower value:");
                                AddInput("Amount:", 4);
                                AddOption("Nevermind", 254);
                            }
                            else
                            {
                                GC.MyChar.Arena.WagerAmount = Amount;
                                AddText("Great ! Which kind of duel do you prefer?");
                                AddOption("Single", 5);
                                AddOption("Team Based", 6);
                                AddOption("Nevermind", 254);
                            }
                        }
                        else if (Amount == 0)
                        {
                            AddText("Please input a valid amount of silvers.");
                            AddOption("I see", 254);
                            GC.MyChar.Arena = null;
                        }
                        else
                        {
                            AddText("You don't have enough silvers !");
                            AddOption("I see", 254);
                            GC.MyChar.Arena = null;
                        }
                        break;
                    case 5:
                    case 6:
                        if (_linkback == 5)
                            GC.MyChar.Arena.Against = Features.Arena.Opponent.Single;
                        else
                            GC.MyChar.Arena.Against = Features.Arena.Opponent.Team;
                        if (GC.MyChar.Arena.Wager)
                            AddText($"Great! Now that you've choosen to play a {GC.MyChar.Arena.Against.ToString()} Duel with a bet of {GC.MyChar.Arena.WagerAmount} silvers, which kind of duel do you want to choose?");
                        else
                            AddText($"Great! Now that you've choosen to play a {GC.MyChar.Arena.Against.ToString()} Duel with no bet, which kind of duel do you want to choose?");
                        AddOption("Standard Duel", 7);
                        AddOption("Leech Duel", 8);
                        AddOption("Unlimited Stamina Duel", 9);
                        AddOption("Nevermind", 254);
                        break;
                    case 7:
                    case 8:
                    case 9:
                        if (_linkback == 7)
                        {
                            GC.MyChar.Arena.Type = Features.Arena.DuelType.Standard;
                            AddText("In a standard type duel, players will fight normally with stamina costs, how many hits would you like to fight to?");
                        }
                        else if (_linkback == 8)
                        {
                            GC.MyChar.Arena.Type = Features.Arena.DuelType.Leech;
                            AddText("In a leech type duel, everytime you hit your enemy you won't be wasting stamina, how many hits would you like to fight to?");
                        }
                        else if (_linkback == 9)
                        {
                            GC.MyChar.Arena.Type = Features.Arena.DuelType.UnlimitedStamina;
                            AddText("In an unlimited stamina type duel, players won't have their stamina reduced, how many hits would you like to fight to?");
                        }
                        AddOption("10 Hits", 10);
                        AddOption("100 Hits", 11);
                        AddOption("Nevermind", 254);
                        break;
                    case 10:
                    case 11:
                        if (_linkback == 10)
                            GC.MyChar.Arena.Count = Features.Arena.Hits.Ten;
                        else if (_linkback == 11)
                            GC.MyChar.Arena.Count = Features.Arena.Hits.Hundred;
                        if (!GC.MyChar.Arena.Wager)
                            AddText($"Great ! Everything is ready, who would you like to challenge for a {GC.MyChar.Arena.Against.ToString()} {GC.MyChar.Arena.Count.ToString()} Hit {GC.MyChar.Arena.Type.ToString()} based duel?");
                        else
                            AddText($"Great ! Everything is ready, who would you like to challenge for a {GC.MyChar.Arena.Against.ToString()} {GC.MyChar.Arena.Count.ToString()} Hit {GC.MyChar.Arena.Type.ToString()} based duel with a bet of {GC.MyChar.Arena.WagerAmount} silvers?");
                        AddInput("Player Name:", 12);
                        AddOption("Nevermind", 254);
                        break;
                    case 12:
                        string PlayerName = ReadString(_data);
                        Character C = World.CharacterFromName2(PlayerName);
                        if (C != null && World.H_Chars.ContainsKey(C.EntityID) && C.Loc.Map == GC.MyChar.Loc.Map)
                        {
                            if (GC.MyChar.Arena.Against == Features.Arena.Opponent.Team && (C.MyTeam == null || GC.MyChar.MyTeam == null || (C.MyTeam != null && !C.TeamLeader) || (GC.MyChar.MyTeam != null && !GC.MyChar.TeamLeader)))
                            {
                                AddText("Either you or your opponent aren't the Team Leaders! Please make sure both of you own a team of 3 at max!");
                                AddOption("I see", 254);
                                break;
                            }
                            GC.MyChar.Inviting = C.EntityID;
                            if (!GC.MyChar.Arena.Wager)
                                AddText($"Are you sure you want to challenge {World.H_Chars[GC.MyChar.Inviting].Name} for a {GC.MyChar.Arena.Against.ToString()} {GC.MyChar.Arena.Count.ToString()} Hit {GC.MyChar.Arena.Type.ToString()} based duel?");
                            else
                                AddText($"Are you sure you want to challenge {World.H_Chars[GC.MyChar.Inviting].Name} for a {GC.MyChar.Arena.Against.ToString()} {GC.MyChar.Arena.Count.ToString()} Hit {GC.MyChar.Arena.Type.ToString()} based duel with a bet of {GC.MyChar.Arena.WagerAmount} silvers?");
                            AddOption("Yeah", 13);
                            AddOption("Nevermind", 254);
                        }
                        else
                        {
                            AddText("It seems the player is either not online or in the same map as you are! Would you like to invite someone else?");
                            AddInput("Player Name:", 12);
                            AddOption("Nevermind", 254);
                        }
                        break;
                    case 13:
                        if (World.H_Chars.ContainsKey(GC.MyChar.Inviting) && GC.MyChar.Loc.Map == World.H_Chars[GC.MyChar.Inviting].Loc.Map)
                        {
                            World.H_Chars[GC.MyChar.Inviting].Dueler = GC.MyChar.EntityID;
                            if (!GC.MyChar.Arena.Wager)
                                World.H_Chars[GC.MyChar.Inviting].MyClient.LocalMessage(2000, $"{GC.MyChar.Name} has invited you for a {GC.MyChar.Arena.Against.ToString()} {GC.MyChar.Arena.Count.ToString()} Hit {GC.MyChar.Arena.Type.ToString()} based Duel at arena! Type /duel if you want to join!");
                            else
                                World.H_Chars[GC.MyChar.Inviting].MyClient.LocalMessage(2000, $"{GC.MyChar.Name} has invited you for a {GC.MyChar.Arena.Against.ToString()} {GC.MyChar.Arena.Count.ToString()} Hit {GC.MyChar.Arena.Type.ToString()} based Duel with a bet of {GC.MyChar.Arena.WagerAmount} silvers! Type /acceptbet if you want to join!");
                            AddText(World.H_Chars[GC.MyChar.Inviting].Name + " was invited successfuly! Please wait for him/her to accept the challenge!");
                            AddOption("Alright!", 255);
                        }
                        else
                        {
                            AddText("It seems the player is either not online or in the same map as you are! Would you like to invite someone else?");
                            AddInput("Player Name:", 12);
                            AddOption("Nevermind", 254);
                        }
                        break;
                    case 254:
                        if (GC.MyChar.Arena != null && GC.MyChar.Arena.MapID != GC.MyChar.Loc.Map)
                            GC.MyChar.Arena = null;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            AddFinish();
            Send();
        }
    }
}