using picpresencelib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PICPresence.Util
{
    public class Attach2
    {
        public SerialPortFlow _serialPort { get; set; }
        public delegate string addRow();
        public delegate void callback();
        public Thread thr { set; get; }
        public int LCDVisibleAreaLength { get; set; }
        public delegate void reactiveFetch();

        public Attach2(SerialPortFlow _serialPort, int LCDVisibleAreaLength = 16)
        {
            this._serialPort = _serialPort;
            this.LCDVisibleAreaLength = LCDVisibleAreaLength;
        }

        public void Run(addRow r1, addRow r2, reactiveFetch reactiveFetch, int delay = 1000)
        {

            thr = new Thread(() =>
            {
                while (true)
                {

                    reactiveFetch();

                    var _r1 = r1();
                    var _r2 = r2();

                    _serialPort.Write(_r1 + _r2);

                    Thread.Sleep(delay);
                }
            });
            thr.IsBackground = true;
            thr.Start();
        }
    }
}
