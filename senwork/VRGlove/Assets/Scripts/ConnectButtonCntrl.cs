using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButtonCntrl : MonoBehaviour
{
    public GameObject targetMac;
    public GameObject targetWin;
    public BleCntrlWinrt bleCntrlWinrt;
    /*
    [DllImport("__Internal")]
    private static extern bool IsConnected(string targetName);

    [DllImport("__Internal")]
    private static extern void Connect(string targetName);

    [DllImport("__Internal")]
    private static extern void Disconnect(string targetName);
    */

    public void OnClick()
    {
        if (!bleCntrlWinrt.IsConnected()){
            bleCntrlWinrt.StartScanHandler();
            GetComponentInChildren<Text>().text = "Disconnect";
        }else{
            bleCntrlWinrt.Disconnect();
            GetComponentInChildren<Text>().text = "Connect";
        }
        
        /*
        if (!IsConnected(target.name))
        {
            Connect(target.name);
            GetComponentInChildren<Text>().text = "Disconnect";
        }
        else
        {
            Disconnect(target.name);
            GetComponentInChildren<Text>().text = "Connect";
        }*/
    }
}
