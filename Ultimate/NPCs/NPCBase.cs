using Ultimate.Main;
using System;
using System.Collections.Generic;

namespace Ultimate.NPCs
{
    public class NPCBase
    {
        #region Properties

        public uint ID;
        public ushort Face;
        private readonly Main.GameClient GC;
        public bool IsGlobal = false;
        public List<COPacket> Responses;
        #endregion

        public string ReadString(byte[] Data)
        {
            string Name = "";
            for (int i = 14; i < 14 + Data[13]; i++)
                Name += Convert.ToChar(Data[i]);
            return Name;
        }

        public NPCBase(Main.GameClient _client)
        {
            GC = _client;
        }

        public virtual void Run(Main.GameClient _client, byte[] Data, ushort _linkback)
        {

        }

        public void Send()
        {
            if (Responses.Count > 2)
                foreach (var response in Responses)
                    GC.AddSend(response);
        }

        public void AddText(string _value)
        {
            var packet = Packets.NPCSay(_value);
            Responses.Add(packet);
        }

        public void AddInput(string _value, byte _linkback)
        {
            var packet = Packets.NPCLink2(_value, _linkback);
            Responses.Add(packet);
        }

        public void AddAvatar()
        {
            var packet = Packets.NPCSetFace(Face);
            Responses.Add(packet);
        }

        public void AddOption(string _value, byte _linkback)
        {
            var packet = Packets.NPCLink(_value, _linkback);
            Responses.Add(packet);
        }

        public void AddFinish()
        {
            var packet = Packets.NPCFinish();
            Responses.Add(packet);
        }

        public void RemoveItem(uint _id)
        {
            GC.MyChar.RemoveItem(GC.MyChar.NextItem(_id));
        }

        public void Popup(ushort _type)
        {
            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, _type, 0, 0, 126));
        }

        public void PlusItemReward(byte Plus, byte Amount)
        {
            for (int a = 0; a < Amount; a++)
            {
                top:
                Game.Item I2 = new Game.Item();
                I2.UID = (uint)Program.Rnd.Next(10000000);
                Game.Item.ItemQuality Q = Game.Item.ItemQuality.Normal;
                uint ItemID = 0;
                List<uint> From = new List<uint>();
                int Type = Program.Rnd.Next(0, 255);
                uint Part = 0;
                if (Type < 10) Part = 111;
                else if (Type < 20) Part = 113;
                else if (Type < 30) Part = 114;
                else if (Type < 40) Part = 117;
                else if (Type < 50) Part = 118;
                else if (Type < 60) Part = 120;
                else if (Type < 70) Part = 121;
                else if (Type < 80) Part = 130;
                else if (Type < 90) Part = 131;
                else if (Type < 100) Part = 133;
                else if (Type < 110) Part = 134;
                else if (Type < 120) Part = 141;
                else if (Type < 130) Part = 142;
                else if (Type < 140) Part = 150;
                else if (Type < 150) Part = 151;
                else if (Type < 160) Part = 152;
                else if (Type < 165) Part = 160;
                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                {
                    if (D.LevReq >= 5 && D.LevReq <= 110)
                    {
                        if (D.LevReq != 0)
                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                From.Add(D.ID);
                    }
                }
                if (From != null)
                {
                    if (From.Count > 0)
                    {
                        byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                        ItemID = (uint)From[Tries];
                    }
                }
                if (ItemID != 0)
                {
                    I2.ID = ItemID;
                    if (I2.DBInfo.LevReq != 1)
                    {
                        Game.ItemIDManipulation E = new Game.ItemIDManipulation(ItemID);
                        E.QualityChange(Q);
                        I2.ID = E.ToID();
                    }
                    I2.Color = Game.Item.ArmorColor.Orange;
                    I2.Plus = Plus;
                    I2.MaxDur = I2.DBInfo.Durability;
                    I2.CurDur = I2.MaxDur;
                    GC.MyChar.AddItem(I2);
                }
                else goto top;
            }
        }
    }
}
