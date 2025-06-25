using System;
using System.Threading;

namespace Ultimate
{
    public delegate void Execute();
    public class MyThread
    {
        public event Execute Execute;
        private Timer T;
        int interval;
        //bool RunOnce = false;
        /// <param name="Interval">Set the interval as milliseconds.</param>
        public void Start(int Interval,int dueTime = 2000)
        {
           // RunOnce = oneTime;
            interval = Interval;
            T = new Timer(Run, this, dueTime, Interval);
        }
        bool closed = false;
        public void Close()
        {
            if (!closed)
            {
                closed = true;
                //T.Change(Timeout.Infinite, Timeout.Infinite);
                T.Dispose();
                T = null;
                Execute = null;
            }
        }
        void Run(object ob)
        {
            if (closed)
            {
                return;
            }
            try
            {
                if (Execute != null)
                    Execute.Invoke();
                else Console.WriteLine("Execute null!");
               /* if (RunOnce)
                    Close();*/

                    //closed = true;
            }
            catch (Exception E) { Game.World.ExcAdd += E.ToString() + "\r\n"; }
        }
    }
}
