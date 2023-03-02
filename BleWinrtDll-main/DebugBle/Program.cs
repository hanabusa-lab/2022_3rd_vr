using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Console.WriteLine("found device with name: " + deviceName);
                if (deviceId == null && deviceName == "BBC micro:bit [geget]") { 
                    deviceId = _deviceId;
                }

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
           
            Console.WriteLine("Start Connect...");
            ble.Connect(deviceId,
               "{6e400001-b5a3-f393-e0a9-e50e24dcca9e}",
               new string[] { "{6e400002-b5a3-f393-e0a9-e50e24dcca9e}" });
            
            for (int guard = 0; guard < 10; guard++)
            {
                BLE.ReadPackage();
            }
            Thread.Sleep(1000);
           
            ble.Disconnect(deviceId);
            Thread.Sleep(1000);
            //ble.Close();
            
            //BLE ble2 = new BLE();


            /*BLE.Impl.BLEData res = new BLE.Impl.BLEData();
                bool fg = true;
                int cnt = 0;
                while( fg && BLE.Impl.PollData(out res, true))
                {
                    string rcmd = System.Text.Encoding.ASCII.GetString(res.buf, 0, res.size);
                    Console.WriteLine("rcmd="+rcmd);
                    cnt++;
                    if (cnt > 20) fg = false;
                }*/



            Console.WriteLine("Rescan...");
            scan = BLE.ScanDevices();
            deviceId = null;
            scan.Found = (_deviceId, deviceName) =>
            {
                Console.WriteLine("found device with name: " + deviceName);
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
            

            Thread.Sleep(500);

            Console.WriteLine("Start Connect...");
            ble.Connect(deviceId,
               "{6e400001-b5a3-f393-e0a9-e50e24dcca9e}",
               new string[] { "{6e400002-b5a3-f393-e0a9-e50e24dcca9e}" });
            for (int guard = 0; guard < 10; guard++)
            {
                BLE.ReadPackage();
            }
            ble.Disconnect(deviceId);

            ble.Close();

            Console.WriteLine("Press enter to exit the program...");
            Console.ReadLine();


        }



    }
}
