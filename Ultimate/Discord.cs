using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace Ultimate
{

    class Discord
    {
        static DiscordClient discord;
        public async void Discord_Basladi()
        {

            Console.WriteLine("Discord Bot Active !");
            Console.WriteLine("");
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "MTI1MTE3OTgyOTk0MDk4MTc2MA.Gsqb5L.sOYGMFbHT9l8lZRLp5KNGtZxR8JMLXczEuq7uE",  /// DC botunun token kodu
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content == "/online")
                {
                    var channel = await discord.GetChannelAsync(1251182294769598604);
                    if (channel != null)
                    {
                        if (Game.World.H_Chars.Count < 1)
                        {
                            await channel.SendMessageAsync("Online Players : " + (Game.World.H_Chars.Count));
                        }
                        else if (Game.World.H_Chars.Count < 50)
                        {
                            await channel.SendMessageAsync("Online Players : " + (Game.World.H_Chars.Count + 25));
                        }
                        else
                        {
                            await channel.SendMessageAsync("Online Players : " + (Game.World.H_Chars.Count + 50));
                        }
                    }
                }

                if (e.Message.Content == "/vote")
                {
                    await e.Message.RespondAsync("Dont forget give to vote, invite more players :) https://www.xtremetop100.com/in.php?site=1132375799");
                }
                if (e.Message.Content == "/guildwar")
                {
                    if (Features.GuildWars.War)
                    {
                        var timeLeft = DateTime.Now;
                        if (timeLeft.DayOfWeek != DayOfWeek.Sunday)
                            timeLeft = timeLeft.AddDays(7 - (byte)timeLeft.DayOfWeek);
                        timeLeft = timeLeft.AddHours(19 - timeLeft.Hour).AddMinutes(-timeLeft.Minute);
                        var toDisplay = timeLeft.Subtract(DateTime.Now);

                        await e.Message.RespondAsync($"The Guild War will end in {toDisplay.Days} Days, {toDisplay.Hours} Hours and {toDisplay.Minutes} Minutes. Make sure you won't miss it!");
                    }
                }
                if (e.Message.Content == "/players")
                {
                    var channel1 = await discord.GetChannelAsync(1251182294769598604);
                    if (channel1 != null)
                    {
                        try
                        {
                            Dictionary<uint, Main.GameClient> Clients = Game.World.H_Clients;
                            List<string> IPs = new List<string>();
                            foreach (Main.GameClient C8 in Clients.Values)
                            {
                                if (C8.Soc.Connected)
                                {
                                    string IP = C8.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();
                                    if (!IPs.Contains(IP))
                                        IPs.Add(IP);
                                }
                            }
                            await channel1.SendMessageAsync("Online : " + (Game.World.H_Chars.Count + 35) + " IPs : " + IPs.Count + " Chars per IP " + ((float)Clients.Count / IPs.Count));

                        }
                        catch (Exception E) { Console.WriteLine(E.ToString()); }
                    }
                }
            };


            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        public string MesajVer
        {
            set
            {
                MesajVer2(value);
            }
        }

        public async void MesajVer2(string Yazi)
        {
            var channel = await discord.GetChannelAsync(1251182294769598604); // Vote
            if (channel != null)
            {
                await channel.SendMessageAsync("Vote : " + Yazi);
            }
        }

        public string MesajVer3
        {
            set
            {
                MesajVer4(value);
            }
        }

        public async void MesajVer4(string Yazi)
        {
            var channel = await discord.GetChannelAsync(1253451042025373738); // socket
            if (channel != null)
            {
                await channel.SendMessageAsync("Socket : " + Yazi);
            }
        }

        public string MesajVer5
        {
            set
            {
                MesajVer6(value);
            }
        }

        public async void MesajVer6(string Yazi) //GeneralChat
        {
            var channel = await discord.GetChannelAsync(1251182294769598604); // Kanal ID si
            if (channel != null)
            {
                await channel.SendMessageAsync(Yazi);
            }
        }

        public string MesajVer7 //QuestChannel
        {
            set
            {
                MesajVer8(value);
            }
        }

        public async void MesajVer8(string Yazi)
        {
            var channel = await discord.GetChannelAsync(1251182294769598604); // QuestChannel
            if (channel != null)
            {
                await channel.SendMessageAsync(Yazi);
            }
        }

        public string MesajVer9 //QuestChannel
        {
            set
            {
                MesajVer10(value);
            }
        }

        public async void MesajVer10(string Yazi)
        {
            var channel = await discord.GetChannelAsync(1251182294769598604); // QuestChannel
            if (channel != null)
            {
                await channel.SendMessageAsync(Yazi);
            }
        }

        public string MesajVer11 //QuestChannel
        {
            set
            {
                MesajVer12(value);
            }
        }

        public async void MesajVer12(string Yazi)
        {
            var channel = await discord.GetChannelAsync(1251182294769598604); // QuestChannel
            if (channel != null)
            {
                await channel.SendMessageAsync(Yazi);
            }
        }

    }
}
