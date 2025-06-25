using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Game
{
    public class StatusEffect
    {
        public List<StatusEffectEn> Array;
        public StatusEffectEn Value;
        readonly Character Entity;

        public StatusEffect(Character C)
        {
            Entity = C;
            Array = new List<StatusEffectEn>();
            Value = StatusEffectEn.Normal;
        }
        public void Add(StatusEffectEn SE)
        {
            if (!Array.Contains(SE))
            {
                Array.Add(SE);
                Value |= SE;
                World.Spawn(Entity, false);
            }
            if (Entity != null && Entity.MyClient != null)
                Entity.MyClient.AddSend(Packets.Status(Entity.EntityID, Status.Effect, (ulong)Value));
        }
        public bool Contains(StatusEffectEn SE)
        {
            return Array.Contains(SE);
        }
        public void Remove(StatusEffectEn SE)
        {
            if (Array.Contains(SE))
            {
                Array.Remove(SE);
                if (Value != StatusEffectEn.Normal)
                    Value -= SE;
                Entity.MyClient.AddSend(Packets.Status(Entity.EntityID, Status.Effect, (ulong)Value));
                World.Spawn(Entity, false);
            }
        }
        public void Clear()
        {
            Array = new List<StatusEffectEn>();
            Value = StatusEffectEn.Normal;
            Entity.MyClient.AddSend(Packets.Status(Entity.EntityID, Status.Effect, (ulong)Value));
        }
    }
}
