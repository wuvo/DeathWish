using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using Ultimate.Structures;

namespace Ultimate.Items
{
    public class Item_721954 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.RemoveItem(I.UID);
            Buff B = new Buff();
            B.Eff = Features.SkillsClass.ExtraEffect.Transform;
            B.Lasts = 60;
            B.Value = 60;
            B.Started = DateTime.Now;
            B.StEff = Game.StatusEffectEn.Normal;
            C.TimeBuff = B.Lasts;

            List<uint> Meshes = new List<uint>() { 104, 111, 117, 130, 132, 152, 157, 180, 202, 203, 204, 208, 209, 211, 213, 215, 217, 218, 223, 226, 230, 232, 233, 240, 242, 243, 245, 246, 247, 248, 249, 250, 252, 255, 257, 258, 266, 269, 280, 282, 283, 284, 285, 295, 296, 304, 306, 307, 309, 310, 311, 314, 317, 327, 332, 369, 380, 397, 542, 543, 545, 547, 652, 645, 646 };
            B.Transform = Meshes[Program.Rnd.Next(0, Meshes.Count)];
            C.AddBuff(B);
            C.MyClient.LocalMessage(2005, "You turned into a monster for one minute!");
        }
    }
}