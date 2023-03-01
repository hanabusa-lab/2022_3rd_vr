using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ScanButtonCntrl : MonoBehaviour
{
    //public GameObject targetMac;
    //public GameObject targetWin;
    public BLECntrlWinrt bleCntrlWinrt;
    [SerializeField] public GameObject connectBtn;
    /*
    [DllImport("__Internal")]
    private static extern bool IsConnected(string targetName);

    [DllImport("__Internal")]
    private static extern void Connect(string targetName);

    [DllImport("__Internal")]
    private static extern void Disconnect(string targetName);
    */

    void Start()
    {
        GetComponentInChildren<Text>().text = "スキャン開始";
    }

    // Update is called once per frame
    void Update()
    {
        //スキャンが終わったら再度スキャンができるようにする
        if (bleCntrlWinrt.IsScanning()){
            GetComponentInChildren<Text>().text = "スキャン中";
        }else{
            GetComponentInChildren<Text>().text = "スキャン開始";
        }
    }

    public void OnClick()
    {
        //bleCntrlWinrt.Disconnect();
        //スキャンが未実地の場合 スキャンを開始する。
        if (!bleCntrlWinrt.IsScanning()){
            //スキャンごとに状態をクリアする。
            bleCntrlWinrt.Disconnect();
            bleCntrlWinrt.StartScanHandler();
        }
    }
}
