namespace DeathWish.Game.MsgLoader
{
    using System;

    public class SpeedProtect
    {
        public static void JumpProdect(Client.GameClient pClient)
        {
            lock (pClient.JumpLock)
            {
                double mill = (pClient.Player.JumpingStamp - pClient.Player.PreviousJump).TotalMilliseconds;
                if (mill > 100 && mill <= 550)
                {
                    if (!pClient.Player.OnSpeedFlags() && !pClient.Player.OnTransform && !pClient.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.Ride) && pClient.Player.TransformationID != 207 && pClient.Player.TransformationID != 267)
                    {
                        pClient.Player.CountJumpHack++;
                        if (pClient.Player.CountJumpHack >= 5)
                        {
                            pClient.Player.CountJumpHack = 0;
                            CheatPacket.Detected(pClient, "Speed Hack");
                        }
                    }
                }
                else
                {
                    if (pClient.Player.CountJumpHack > 0)
                        pClient.Player.CountJumpHack--;
                }
                pClient.Player.PreviousJump = pClient.Player.JumpingStamp;
                pClient.Player.JumpingStamp = DateTime.Now;
            }
        }
        public static void MovementProdect(Client.GameClient pClient)
        {
            lock (pClient.JumpLock)
            {
                double mill = (pClient.Player.MoveingStamp - pClient.Player.PreviousMove).TotalMilliseconds;
                if (mill > 0 && mill < 150)
                {
                    if (!pClient.Player.OnSpeedFlags() && !pClient.Player.OnTransform && !pClient.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.Ride) && pClient.Player.TransformationID != 207 && pClient.Player.TransformationID != 267)
                    {
                        pClient.Player.CountMoveHack++;
                        if (pClient.Player.CountMoveHack >= 5)
                        {
                            pClient.Player.CountMoveHack = 0;
                            CheatPacket.Detected(pClient, "Speed Hack");
                        }
                    }
                }
                else
                {
                    if (pClient.Player.CountMoveHack > 0)
                        pClient.Player.CountMoveHack--;
                }
                pClient.Player.PreviousMove = pClient.Player.MoveingStamp;
                pClient.Player.MoveingStamp = DateTime.Now;
            }
        }
    }
}
