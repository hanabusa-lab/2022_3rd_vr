using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BLECntrlWinrt : MonoBehaviour
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
    IDictionary foundDevices = new Dictionary<string, string>();
    int devicesCount = 0;

    // BLE Threads 
    Thread scanningThread, connectionThread, readingThread;

    // GUI elements
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
        //Debug.Log("dicrcount="+foundDevices.Count+" devcount="+ devicesCount);
                    
        //Scanc中にmicro:bitを見つけたらbleDDに項目を追加する。
        if (foundDevices.Count > devicesCount){
            Debug.Log("dicrcount="+foundDevices.Count+" devcount="+ devicesCount);
       
            devicesCount = foundDevices.Count;
            foreach(String value in foundDevices.Values){
                Debug.Log("list add micro:bit="+value);
                Dropdown.OptionData optionData = new Dropdown.OptionData(value);
                bleDD.options.Add(optionData);
            }
        }

        //if(ble.isConnected && readingThread==null !readingThread.IsAlive){
        //micro:bitとの接続を開始した際
        if(ble.isConnected && readingThread==null){  
            Debug.Log("start readingThread");
            readThreadFg = true;
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
    }

    //Scanの開始。Connectボタンが押された際に実行する
    public void StartScanHandler()
    {
        //clear bleDD list
        bleDD.ClearOptions();
        Dropdown.OptionData optionData = new Dropdown.OptionData("micro:bit");
        bleDD.options.Add(optionData);

        Debug.Log("StartScanHander");
        devicesCount = 0;
        isScanning = true;
        foundDevices.Clear();
        if(scan!=null){
            scan.Cancel();
        }
        if(scanningThread!=null){
            scanningThread.Abort();
        }

        scanningThread = new Thread(ScanBleDevices);
        scanningThread.Start();
    }

    //接続の有無を確認する
    public bool IsConnected(){
        return isConnected; 
    }

    public bool IsScanning(){
        return isScanning; 
    }

    void ScanBleDevices()
    {
        deviceId = null;

        scan = BLE.ScanDevices();
        Debug.Log("BLE.ScanDevices() started.");
        scan.Found = (_deviceId, deviceName) =>
        {
            Debug.Log("found device with name: " + deviceName);
            //add device if device contain micro:bit
            if(deviceName!="" && deviceName.Contains("micro:bit")){
                //discoverDeviceに項目がない場合に追加する。
                //if(!foundDevices.Contains(_deviceId)){
                if(!foundDevices.Contains(_deviceId)){
                    foundDevices.Add(_deviceId, deviceName);
                    Debug.Log("add foundDevices micro:bit" + deviceName);
                }
            }
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
    }

    // Start establish BLE connection with
    // target device in dedicated thread.
    public void StartConHandler()
    {
        connectionThread = new Thread(ConnectBleDevice);
        connectionThread.Start();
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
        if (ble.isConnected){
            Debug.Log("Connected to: " + targetDeviceName);
        }
    }

    private void ReadBleData(object obj)
    {
        //while(readThreadFg){
        while(readThreadFg){
        byte[] packageReceived = BLE.ReadBytes();
        string rcmd = System.Text.Encoding.ASCII.GetString(packageReceived);
            //Debug.Log("packageReceived" + rcmd);
            receiveCmd = rcmd;
        }
    }
    
    //ドロップダウンが選択された際に実行される。
    public void OnDropdownValueSelected(){
        Debug.Log("selected-----="+bleDD.value);
        string deviceName = bleDD.options[bleDD.value].text;

        //deviceIDの取得
        deviceId = null;
        //foreach (var entry in foundDevices){
        foreach (string key in foundDevices.Keys){
            if(foundDevices[key]==deviceName){
                deviceId = key;
                Debug.Log("selected "+deviceId+" name="+deviceName);

                //スキャンの停止
                scan.Cancel();
                //Connectionの開始
                StartConHandler();
            }
        }
    }

    // Prevent threading issues and free BLE stack.
    // Can cause Unity to freeze and lead
    // to errors when omitted.
    private void CleanUp()
    {
        try
        {
            //deviceを切断する
            if(deviceId != null){
                ble.Disconnect(deviceId);
            }

            scan.Cancel();
            ble.Close();
        
            if(scanningThread!=null){
                scanningThread.Abort();
                scanningThread=null;
            }
            if(connectionThread!=null){
                connectionThread.Abort();
                connectionThread=null;
            }

            readThreadFg=false;
            if(readingThread!=null){
                readingThread.Abort();
                readingThread = null;
            }

            foundDevices.Clear();
            deviceId = null;

          
        } catch(NullReferenceException e)
        {
            Debug.Log("Thread or object never initialized.\n" + e);
        }        
    }

    //切断処理
    public void Disconnect(){
         CleanUp();
    }

    private void OnDestroy()
    {
        CleanUp();
        Debug.Log("Exec CleanUp OnDestroy\n");
    }

    private void OnApplicationQuit()
    {
        CleanUp();
        Debug.Log("Exec CleanUp OnApplication \n");
    }
}
