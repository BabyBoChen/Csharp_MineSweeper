using System;
using System.Timers;
using System.Threading;

namespace Mine
{
    class StopWatch
    {

        public event EventHandler SendTime;
        
        private Thread TimeThread;
        private DateTime now = DateTime.Now;
        private DateTime now2 = DateTime.Now;
        private TimeSpan ts;
        private bool _ticking = true;

        public bool Ticking 
        {
            get
            {
                return this._ticking;
            }
            set
            {
                _ticking = value;
            }
        }

        public TimeSpan Ts
        {
            get
            {
                return this.ts;
            }
        }

        public StopWatch()
        {
            this.TimeThread = new Thread(()=> {
                Tick(this, EventArgs.Empty);
            });
            this.now = DateTime.Now;
            this.TimeThread.IsBackground = true;
            this.TimeThread.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            while (true)
            {
                Thread.Sleep(500);
                this.now2 = DateTime.Now;
                if (Ticking == true)
                {
                    this.ts = this.ts.Add(now2.Subtract(now));
                    if (this.SendTime != null) SendTime(this,EventArgs.Empty);
                }
                this.now = this.now2;
            }
        }
    }
}
