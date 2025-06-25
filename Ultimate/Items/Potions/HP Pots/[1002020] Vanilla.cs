using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1002020 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.CurHP += 2000;
            C.RemoveItem(I);
        }
    }
}