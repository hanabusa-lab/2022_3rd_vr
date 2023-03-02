using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BLECntrlWinrt : MonoBehaviour
{
    //ScrollViewオブジェクト
    public GameObject bleScrollView;
    //handオブジェクト
    public GameObject hand;

    // Change this to match your device.
    string targetDeviceName = "";//"BBC micro:bit [geget]";
    string serviceUuid = "{6e400001-b5a3-f393-e0a9-e50e24dcca9e}";
    string[] characteristicUuids = {
         "{6e400002-b5a3-f393-e0a9-e50e24dcca9e}"      // ReceiveCharacteristic
    };

    BLE ble;
    BLE.BLEScan scan;
    bool isScanning = false, isConnected = false;
    string deviceId = null;  
    IDictionary foundDevices = new Dictionary<string, string>();
    int devicesCount = 0;

    // BLE Threads 
    Thread scanningThread, connectionThread, readingThread;

     bool readThreadFg;
    String receiveCmdHandAngle;
    String receiveCmdHandPos;
    String receiveCmdFingure;

     //GloveData 
    GloveData gloveData;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        //receiveCmd=null;
        receiveCmdHandAngle=null;
        receiveCmdHandPos=null;
        receiveCmdFingure=null;

        ble = new BLE();

        gloveData = new GloveData();
     }

    [Serializable]
    public class GloveData
    {
        public string dat1;
        public string dat2;
    }
    // Update is called once per frame
    void Update()
    {  
        //Debug.Log("dicrcount="+foundDevices.Count+" devcount="+ devicesCount);
                    
        //Scan中にmicro:bitを見つけたらbleDDに項目を追加する。
        if (foundDevices.Count > devicesCount){
            Debug.Log("dicrcount="+foundDevices.Count+" devcount="+ devicesCount);
       
            devicesCount = foundDevices.Count;
            foreach(String value in foundDevices.Values){
                Debug.Log("list add micro:bit="+value);
                //Dropdown.OptionData optionData = new Dropdown.OptionData(value);
                bleScrollView.GetComponent<BLEScrollViewCntrl>().addMicrobitButton(value);
                //bleDD.options.Add(optionData);
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
        //TBD receiveCmdを増やして、パフォーマンス向上を行うこと。
        if(ble.isConnected && readingThread.IsAlive){
            if(receiveCmdHandAngle!=null){
                string[] cmd = receiveCmdHandAngle.Split(',');
                if(cmd[0].Substring(0,1)=="p"){
                                        
                    gloveData.dat1 = cmd[0].Substring(2);
                    gloveData.dat2 = cmd[1].Substring(2);
                    string json = JsonUtility.ToJson(gloveData);
                    hand.GetComponent<HandCntrl>().OnGloveAngleChanged(json);
                }
                receiveCmdHandAngle=null;
            };
            if(receiveCmdHandPos!=null){
                string[] cmd = receiveCmdHandPos.Split(',');
                if(cmd[0].Substring(0,1)=="h"){
                    gloveData.dat1 = cmd[0].Substring(3);
                    gloveData.dat2 = cmd[1].Substring(3);
                    string json = JsonUtility.ToJson(gloveData);
                    //hand.GetComponent<HandCntrl>().OnGloveHandPosChanged(json);
                }
                receiveCmdHandPos=null;
            }
            if(receiveCmdFingure!=null){
               string[] cmd = receiveCmdFingure.Split(',');
               if(cmd[0].Substring(0,1)=="f"){
                    gloveData.dat1 = cmd[0].Substring(3);
                    gloveData.dat2 = cmd[1].Substring(3);
                    string json = JsonUtility.ToJson(gloveData);
                    hand.GetComponent<HandCntrl>().OnGloveFingureChanged(json);
                }
                receiveCmdFingure=null;
            }
            /*if(receiveCmd!=null){
                Debug.Log("receiveCmd="+receiveCmd);
                string[] cmd = receiveCmd.Split(',');
                if(cmd[0].Substring(0,1)=="p"){
                                        
                    gloveData.dat1 = cmd[0].Substring(2);
                    gloveData.dat2 = cmd[1].Substring(2);
                    string json = JsonUtility.ToJson(gloveData);
                    hand.GetComponent<HandCntrl>().OnGloveAngleChanged(json);
                }
                if(cmd[0].Substring(0,1)=="f"){
                    gloveData.dat1 = cmd[0].Substring(3);
                    gloveData.dat2 = cmd[1].Substring(3);
                    string json = JsonUtility.ToJson(gloveData);
                    hand.GetComponent<HandCntrl>().OnGloveFingureChanged(json);
                }
                if(cmd[0].Substring(0,1)=="h"){
                    gloveData.dat1 = cmd[0].Substring(3);
                    gloveData.dat2 = cmd[1].Substring(3);
                    string json = JsonUtility.ToJson(gloveData);
                    hand.GetComponent<HandCntrl>().OnGloveHandPosChanged(json);
                }
            }*/
        }
    }

    //Scanの開始。Connectボタンが押された際に実行する
    public void StartScanHandler()
    {
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
    public void ConnectBleDevice(string deviceName)
    {
        //deviceIDの取得
        deviceId = null;
        //foreach (var entry in foundDevices){
        foreach (string key in foundDevices.Keys){
            if(foundDevices[key]==deviceName){
                deviceId = key;
                Debug.Log("selected "+deviceId+" name="+deviceName);

                //スキャンの停止
                scan.Cancel();
  
                //コネクションスレッドの開始
                connectionThread = new Thread(ConnectBleDeviceThread);
                connectionThread.Start();
            }
        }
    }

    private void ConnectBleDeviceThread()
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
            //receiveCmd = rcmd;
            //パフォーマンス向上として内容ごとに別のcmdを設ける
            if(rcmd.Substring(0,1)=="p"){
                receiveCmdHandAngle = rcmd;
            }
            if(rcmd.Substring(0,1)=="f"){
                receiveCmdFingure = rcmd;
            }
            if(rcmd.Substring(0,1)=="h"){
                receiveCmdHandPos = rcmd;
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
