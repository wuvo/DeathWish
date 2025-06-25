using Ultimate.Main;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    public class NPC_13653 : NPCBase
    {




        public NPC_13653(Main.GameClient _client)
            : base(_client)
        {
            ID = 13653;
            Face = 67;
            IsGlobal = true;
        }

        private static System.Timers.Timer aTimer;

        byte VoteAdeti;
        string[] OyVerenler = new string[3];

        private void OyVerdi(Main.GameClient OyVeren)
        {
            OyVeren.MyChar.Voted = true;

            aTimer = new System.Timers.Timer(30000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            bool Girdi = false;
            foreach (Character C in World.H_Chars.Values)
                if (C.Voted)
                {
                    Girdi = true;
                    C.Voted = false;
                    Game.World.SendMsgToAll("SYSTEM", C.Name + " - Thank you for voting!", 2005, 0);
                    C.VotePoints++;
                    Discord DCord = new Discord();
                    DCord.MesajVer = "Thanks for voting, __**" + C.Name + "**__ !";
                    break;
                }

            if (!Girdi)
                aTimer.Enabled = false;


        }
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (GC.MyChar.Level < 0)
                    {
                        AddText("Hello " + GC.MyChar.Name + ". Unfortunately players below 50 cannot vote. Please try again later..!");
                        AddOption("Okay, I'll vote later.", 255);

                    }
                    else
                    {

                        AddText("Hello " + GC.MyChar.Name + ". You may now vote for the server. Save up vote points for cool rewards like VIP! You must fill in the code that opens in your browser and click the vote button!");
                        AddOption("Okay, I'll go vote now.", 1);


                    }
                    break;
                case 1:
                    {

                        MySQL.MySqlCommand KayitDosyasi = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("OylamaKaydi").Where("IpAdres", GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString());
                        MySQL.MySqlReader KayitSatiri = new MySQL.MySqlReader(KayitDosyasi);

                        bool IP_Varligi = false, Isim_Varligi = false;
                        string Tarih = "";

                        while (KayitSatiri.Read())
                        {
                            IP_Varligi = true;
                            Tarih = KayitSatiri.ReadString("Tarih");
                        }

                        if (!IP_Varligi)
                        {
                            KayitDosyasi = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("OylamaKaydi").Where("OyuncuAdi", GC.MyChar.Name);
                            KayitSatiri = new MySQL.MySqlReader(KayitDosyasi);
                            while (KayitSatiri.Read())
                            {
                                Isim_Varligi = true;
                                Tarih = KayitSatiri.ReadString("Tarih");
                            }

                        }

                        if (Isim_Varligi || IP_Varligi)
                        {

                            if (DateTime.Now >= Convert.ToDateTime(Tarih).AddHours(12))
                            {


                                GC.LocalMessage(2105, "http://www.xtremetop100.com/in.php?site=1132375799&postback=" + GC.MyChar.Name);

                                MySQL.MySqlCommand KayitYeri = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);

                                if (Isim_Varligi)
                                {
                                    KayitYeri.Delete("OylamaKaydi", "OyuncuAdi", GC.MyChar.Name).Execute();
                                }
                                else
                                {
                                    KayitYeri.Delete("OylamaKaydi", "IpAdres", GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()).Execute();
                                }

                                KayitYeri = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                                KayitYeri.Insert("OylamaKaydi").Insert("OyuncuAdi", GC.MyChar.Name).Insert("IpAdres", GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()).Insert("Tarih", DateTime.Now).Execute();
                                OyVerdi(GC);

                            }
                            else
                            {
                                AddText("You have already voted in the last 12 hours from this IP.");
                                AddOption("I see", 255);

                            }
                        }
                        else
                        {

                            GC.LocalMessage(2105, "http://www.xtremetop100.com/in.php?site=1132375799&postback=" + GC.MyChar.Name);
                            MySQL.MySqlCommand AparatKayit = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                            AparatKayit.Insert("OylamaKaydi").Insert("OyuncuAdi", GC.MyChar.Name).Insert("IpAdres", GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()).Insert("Tarih", DateTime.Now).Execute();

                            OyVerdi(GC);
                        }


                        break;
                    }
            }
            AddFinish();
            Send();
        }

    }
}