using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DebugBle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("You can use this program to test the BleWinrtDll.dll. Make sure your Computer has Bluetooth enabled.");

            BLE ble = new BLE();
            string deviceId = null;

            BLE.BLEScan scan = BLE.ScanDevices();
            scan.Found = (_deviceId, deviceName) =>
            {
                Console.WriteLine("found device with name: " + deviceName+ "devicde id"+_deviceId);
                if (deviceId == null && deviceName == "BBC micro:bit [geget]")
                    deviceId = _deviceId;
            };
            scan.Finished = () =>
            {
                Console.WriteLine("scan finished");
                if (deviceId == null)
                    deviceId = "-1";
            };
            while (deviceId == null)
                Thread.Sleep(500);

            scan.Cancel();
            if (deviceId == "-1")
            {
                Console.WriteLine("no device found!");
                return;
            }

            /*ble.Connect(deviceId,
                "{f6f04ffa-9a61-11e9-a2a3-2a2ae2dbcce4}", 
                new string[] { "{f6f07c3c-9a61-11e9-a2a3-2a2ae2dbcce4}",
                    "{f6f07da4-9a61-11e9-a2a3-2a2ae2dbcce4}",
                    "{f6f07ed0-9a61-11e9-a2a3-2a2ae2dbcce4}" });
            */
            //uart
             ble.Connect(deviceId,
                "{6e400001-b5a3-f393-e0a9-e50e24dcca9e}",
                new string[] { "{6e400002-b5a3-f393-e0a9-e50e24dcca9e}" });
            
            /*
             //button
            ble.Connect(deviceId,
                "{e95d9882-251d-470a-a062-fa1922dfa9a8}",
                new string[] { "{e95dda91-251d-470a-a062-fa1922dfa9a8}" });
           */


            Console.WriteLine("connected");
            //BLE.Subscribe(deviceId, "{6e400001-b5a3-f393-e0a9-e50e24dcca9e}", new string[] { "{6e400002-b5a3-f393-e0a9-e50e24dcca9e}" });



            //for (int guard = 0; guard < 50; guard++)
            //{
                /*Console.WriteLine("waiting");
                Console.WriteLine("erro:"+BLE.GetError());
                */
                BLE.Impl.BLEData res = new BLE.Impl.BLEData();
                bool fg = true;
                int cnt = 0;
                while( fg && BLE.Impl.PollData(out res, true))
                {
                    string rcmd = System.Text.Encoding.ASCII.GetString(res.buf, 0, res.size);
                    Console.WriteLine("rcmd="+rcmd);
                    cnt++;
                    if (cnt > 50) fg = false;
                }
                //BLE.ReadPackage();
                /*BLE.WritePackage(deviceId,
                    "{f6f04ffa-9a61-11e9-a2a3-2a2ae2dbcce4}",
                    "{f6f07ffc-9a61-11e9-a2a3-2a2ae2dbcce4}",
                    new byte[] { 0, 1, 2 });
                Console.WriteLine(BLE.GetError());*/
                //Thread.Sleep(5);
                //Console.WriteLine("guard="+guard);
            //}

            Console.WriteLine("Press enter to exit the program...");
            Console.ReadLine();

            ble.Close();
        }
    }
}
