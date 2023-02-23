using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BleCntrlWinrt : MonoBehaviour
{
    //bleSelectionDropDown
    [SerializeField] public Dropdown bleDD;

    // Change this to match your device.
    string targetDeviceName = "BBC micro:bit [geget]";
    string serviceUuid = "{6e400001-b5a3-f393-e0a9-e50e24dcca9e}";
    string[] characteristicUuids = {
         "{6e400002-b5a3-f393-e0a9-e50e24dcca9e}"      // CUUID 1
    };

    BLE ble;
    BLE.BLEScan scan;
    bool isScanning = false, isConnected = false;
    string deviceId = null;  
    IDictionary discoveredDevices = new Dictionary<string, string>();
    int devicesCount = 0;

    // BLE Threads 
    Thread scanningThread, connectionThread, readingThread;

    // GUI elements
    //private Text TextDiscoveredDevices, TextIsScanning, TextTargetDeviceConnection, TextTargetDeviceData;
    //private Button ButtonEstablishConnection, ButtonStartScan;
    int remoteAngle, lastRemoteAngle;
    bool readThreadFg;
    String receiveCmd;

    //handオブジェクト
    public GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        receiveCmd=null;

        ble = new BLE();
        //ButtonEstablishConnection.enabled = false;
        //TextTargetDeviceConnection.text = targetDeviceName + " not found.";
        readingThread = new Thread(ReadBleData);
        readThreadFg = false;
     }


    [Serializable]
    public class GloveAngle
    {
        public string p;
        public string r;
    }
    
    [Serializable]
    public class GloveFingure
    {
        public string f1;
        public string f2;
    }
    // Update is called once per frame
    void Update()
    {  
        //Debug.Log("dicrcount="+discoveredDevices.Count+" devcount="+ devicesCount);
                    
        //セレクションを更新する
        if (discoveredDevices.Count > devicesCount){
            devicesCount = discoveredDevices.Count;
            foreach(String value in discoveredDevices.Values){
                Debug.Log("list micro:bit="+value);
                Dropdown.OptionData optionData = new Dropdown.OptionData(value);
                bleDD.options.Add(optionData);
            }
            //KeyValuePair<string, string> deviceName =  discoveredDevices[discoveredDevices.Count];
            //String deviceName =  discoveredDevices(x => x.Key).Last();
            //Debug.Log("deviceName2="+deviceName.Value);
            //Dropdown.OptionData optionData = new Dropdown.OptionData(deviceName);
            //bleDD.options.Add(optionData);
        }

        //connection threadが有効な際に受信を行う
        //Debug.Log("ble connect="+ble.isConnected+" readingThread="+readingThread.IsAlive);
        if(ble.isConnected && !readingThread.IsAlive){
            Debug.Log("start readingThread");
            readingThread = new Thread(ReadBleData);
            readingThread.Start();
        }

        //受信した情報をもとにhandの更新を行う
        if(ble.isConnected && readingThread.IsAlive){
            if(receiveCmd!=null){
                string[] cmd = receiveCmd.Split(',');
                if(cmd[0].Substring(0,1)=="p"){
                                        
                    GloveAngle angle = new GloveAngle();
                    angle.p = cmd[0].Substring(2);
                    angle.r = cmd[1].Substring(2);
                    string json = JsonUtility.ToJson(angle);
                    hand.GetComponent<HandCntrl>().OnGloveAngleChanged(json);
                }
                if(cmd[0].Substring(0,1)=="f"){
                    GloveFingure fingure = new GloveFingure();
                    fingure.f1 = cmd[0].Substring(3);
                    fingure.f2 = cmd[1].Substring(3);
                    string json = JsonUtility.ToJson(fingure);
                    hand.GetComponent<HandCntrl>().OnGloveFingureChanged(json);
                }
            }
        }

        /*
        if (isScanning)
        {
            //if (ButtonStartScan.enabled)
            //    ButtonStartScan.enabled = false;

            if (discoveredDevices.Count > devicesCount)
            {
                //UpdateGuiText("scan");
                devicesCount = discoveredDevices.Count;
            }                
        } else
        {
             Restart scan in same play session not supported yet.
            if (!ButtonStartScan.enabled)
                ButtonStartScan.enabled = true;
            
            if (TextIsScanning.text != "Not scanning.")
            {
                TextIsScanning.color = Color.white;
                TextIsScanning.text = "Not scanning.";
            }
        }*/

        // The target device was found.
        if (deviceId != null && deviceId != "-1")
        {
            // Target device is connected and GUI knows.
            if (ble.isConnected && isConnected)
            {
                //UpdateGuiText("writeData");
            }
            // Target device is connected, but GUI hasn't updated yet.
            else if (ble.isConnected && !isConnected)
            {
                //UpdateGuiText("connected");
                isConnected = true;
            // Device was found, but not connected yet. 
            } 
            /*else if (!ButtonEstablishConnection.enabled && !isConnected)
            {
                ButtonEstablishConnection.enabled = true;
                TextTargetDeviceConnection.text = "Found target device:\n" + targetDeviceName;
            } */
            
        } 

        //update data
        //TextTargetDeviceData.text = receiveCmd;
    }

    private void OnDestroy()
    {
        CleanUp();
        Debug.Log("Exec CleanUp\n");
    }

    private void OnApplicationQuit()
    {
        CleanUp();
        Debug.Log("Exec CleanUp\n");
    }

    //Scanの開始。Connectボタンが押された際に実行する
    public void StartScanHandler()
    {
        devicesCount = 0;
        isScanning = true;
        discoveredDevices.Clear();
        scanningThread = new Thread(ScanBleDevices);
        scanningThread.Start();
        //TextIsScanning.color = new Color(244, 180, 26);
        //TextIsScanning.text = "Scanning...";
        //TextDiscoveredDevices.text = "";
    }

    //接続の有無を確認する
    public bool IsConnected(){
        return isConnected; 
    }

    // Prevent threading issues and free BLE stack.
    // Can cause Unity to freeze and lead
    // to errors when omitted.
    private void CleanUp()
    {
        try
        {
            scan.Cancel();
            ble.Close();
            scanningThread.Abort();
            connectionThread.Abort();
            readingThread.Abort();
        } catch(NullReferenceException e)
        {
            Debug.Log("Thread or object never initialized.\n" + e);
        }        
    }

    //切断処理
    public void Disconnect(){
        ResetHandler();
    }

    private void ResetHandler()
    {
        readThreadFg=false;
        //TextTargetDeviceData.text = "";
        //TextTargetDeviceConnection.text = targetDeviceName + " not found.";
        // Reset previous discovered devices
        discoveredDevices.Clear();
        //TextDiscoveredDevices.text = "No devices.";
        deviceId = null;
        CleanUp();
    }

    private void ReadBleData(object obj)
    {
        //while(readThreadFg){
        bool fg=true;
        while(fg){
        byte[] packageReceived = BLE.ReadBytes();
        //BLE.ReadPackage();
        // Convert little Endian.
        // In this example we're interested about an angle
        // value on the first field of our package.
        //remoteAngle = packageReceived[0];
         string rcmd = System.Text.Encoding.ASCII.GetString(packageReceived);
            Debug.Log("packageReceived" + rcmd);
            //TextTargetDeviceData.text = rcmd;
            receiveCmd = rcmd;
        }
        //Debug.Log("Angle: " + remoteAngle);
        //TextTargetDeviceData.text = remoteAngle.ToString(); 
        //Debug.Log("get data");
        //Thread.Sleep(10);
        //}
        /*BLE.Impl.BLEData res = new BLE.Impl.BLEData();
        bool fg = true;
        int cnt = 0;
        while( fg && BLE.Impl.PollData(out res, true))
        {
            string rcmd = System.Text.Encoding.ASCII.GetString(res.buf, 0, res.size);
            Console.WriteLine("rcmd="+rcmd);
            cnt++;
            if (cnt > 50) fg = false;
        }*/

    }

    void UpdateGuiText(string action)
    {
        switch(action) {
            case "scan":
                //TextDiscoveredDevices.text = "";
                foreach (KeyValuePair<string, string> entry in discoveredDevices)
                {
                    //TextDiscoveredDevices.text += "DeviceID: " + entry.Key + "\nDeviceName: " + entry.Value + "\n\n";
                    Debug.Log("Added device: " + entry.Key);
                }
                break;
            case "connected":
                //ButtonEstablishConnection.enabled = false;
                //TextTargetDeviceConnection.text = "Connected to target device:\n" + targetDeviceName;
                break;
            case "writeData":
                if (!readingThread.IsAlive)
                //if (!readThreadFg)
                {
                    Debug.Log("start readingThread");
                    readThreadFg = true;
                    readingThread = new Thread(ReadBleData);
                    readingThread.Start();
                }
                if (remoteAngle != lastRemoteAngle)
                {
                    //TextTargetDeviceData.text = "Remote angle: " + remoteAngle;
                    lastRemoteAngle = remoteAngle;
                }
                break;
        }
    }

    void ScanBleDevices()
    {
        //clear bleDD list
        //bleDD = GetComponent<Dropdown>();
        //bleDD.ClearOptions();

        scan = BLE.ScanDevices();
        Debug.Log("BLE.ScanDevices() started.");
        scan.Found = (_deviceId, deviceName) =>
        {
            Debug.Log("found device with name: " + deviceName);
            //add device if device contain micro:bit
            if(deviceName!="" && deviceName.Contains("micro:bit")){
                //discoverDeviceに項目がない場合に追加する。
                //if(!discoveredDevices.Contains(_deviceId)){
                    if(!discoveredDevices.Contains(_deviceId)){
                        discoveredDevices.Add(_deviceId, deviceName);
                        Debug.Log("found micro:bit" + deviceName);
                    }
            }
            //if (deviceId == null && deviceName == targetDeviceName)
            //    deviceId = _deviceId;
        };

        scan.Finished = () =>
        {
            isScanning = false;
            Debug.Log("scan finished");
            if (deviceId == null)
                deviceId = "-1";
        };
        while (deviceId == null)
            Thread.Sleep(500);
        scan.Cancel();
        scanningThread = null;
        isScanning = false;

        if (deviceId == "-1")
        {
            Debug.Log("no device found!");
            return;
        }
    }

    // Start establish BLE connection with
    // target device in dedicated thread.
    public void StartConHandler()
    {
        connectionThread = new Thread(ConnectBleDevice);
        connectionThread.Start();
    }

    
    //ドロップダウンが選択された際に実行される。
    public void OnDropdownValueSelected(){
        Debug.Log("selected-----="+bleDD.value);
        string deviceName = bleDD.options[bleDD.value].text;

        //deviceIDの取得
        deviceId = null;
        //foreach (var entry in discoveredDevices){
        foreach (string key in discoveredDevices.Keys){
            if(discoveredDevices[key]==deviceName){
                deviceId = key;
                Debug.Log("selected "+deviceId+" name="+deviceName);

                //スキャンの停止
                scan.Cancel();
                //Connectionの開始
                StartConHandler();
            }
        }
    }

    private void ConnectBleDevice()
    {
        Debug.Log("Start Connecting to: " + targetDeviceName);

        if (deviceId != null)
        {
            try
            {
                ble.Connect(deviceId,
                serviceUuid,
                characteristicUuids);
            } catch(Exception e)
            {
                Debug.Log("Could not establish connection to device with ID " + deviceId + "\n" + e);
            }
        }
        if (ble.isConnected)
            Debug.Log("Connected to: " + targetDeviceName);
    }

    /*ulong ConvertLittleEndian(byte[] array)
    {
        int pos = 0;
        ulong result = 0;
        foreach (byte by in array)
        {
            result |= ((ulong)by) << pos;
            pos += 8;
        }
        return result;
    }*/
}
