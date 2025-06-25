using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using Ultimate.Structures;

namespace Ultimate.Items
{
    public class Item_722109 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.RemoveItem(I.UID);
            Buff B = new Buff();
            B.Eff = Features.SkillsClass.ExtraEffect.Transform;
            B.Lasts = 180;
            B.Value = 180;
            B.Started = DateTime.Now;
            B.StEff = Game.StatusEffectEn.Normal;
            B.skillID = 8272;
            C.TimeBuff = B.Lasts;

            B.Transform = 132;
            C.AddBuff(B);
            C.MyClient.LocalMessage(2005, "You turned into a Robin for 3 minutes!");
        }
    }
}
