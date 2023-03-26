using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BLEScrollViewCntrl : MonoBehaviour
{
    GameObject scanButton;
    //GameObject scrollView;
    public BLECntrlWinrt bleCntrlWinrt;

    // Start is called before the first frame update
    void Start()
    {
        //var scrollView = this.gameObject;//GameObject.Find("Scroll View");
        // スクリプトからScroll Viewのサイズを変更する場合はアンコメント
        //scrollView.transform.localPosition = new Vector3(0, 0, 0);
        //scrollView.transform.localScale = new Vector3(1, 1, 1);
        //scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 1000);

        //var scrollView = GameObject.Find("BLEScrollView");
        var viewPort = this.gameObject.transform.Find("Viewport");
        
        //var scrollView = GameObject.Find("BLEScrollView");

        //Debug.Log("viewPortName="+viewPort.name);
        //var viewPort = GetComponentInChildren<Viewport>();
        //var viewPort = scrollView.transform.Find("Viewport");
        
        Transform content = viewPort.transform.Find("Content");
        //Debug.Log("contentName="+content.name);
        
        //初期状態ではScanButtonを追加する。
        //GameObjectの作成
        scanButton =  createButton("StartScan");
        scanButton.transform.SetParent(content.transform);
        scanButton.GetComponent<Button>().onClick.AddListener(() => { onScanButtonPressed();});
        //Debug.Log("scanButton="+scanButton.GetParent().name);
        
        scanButton.GetComponentInChildren<Text>().text = "スキャン開始";

        Debug.Log("Start BLEScrollViewCntrl");
    }

    //ボタンオブジェクトの作成
    GameObject createButton(string text){
        GameObject Contents = new GameObject(text);

        //Contents.transform.parent = parent.transform;
        var Rect = Contents.AddComponent<RectTransform>();
        Rect.transform.localPosition = new Vector3(0, 0, 0);
        Rect.transform.localScale = new Vector3(1, 1, 1);
        Rect.sizeDelta = new Vector2(200, 30);

        Contents.AddComponent<CanvasRenderer>();
        Contents.AddComponent<Image>();
        Contents.AddComponent<LayoutElement>().preferredHeight = 30;

        var butttonState = Contents.AddComponent<Button>();

        GameObject textObj = new GameObject(text);
        textObj.transform.parent = Contents.transform;
        var rect = textObj.AddComponent<RectTransform>();
        rect.transform.localPosition = new Vector3(0, 0, 0);
        rect.transform.localScale = new Vector3(1, 1, 1);
        rect.sizeDelta = new Vector2(900, 90);

        textObj.AddComponent<CanvasRenderer>();

        var textChild = textObj.AddComponent<Text>();
        textChild.text = text;
        //textChild.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        textChild.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
        
        textChild.fontSize = 20;
        textChild.alignment = TextAnchor.MiddleCenter;
        textChild.font = Resources.GetBuiltinResource (typeof(Font), "LegacyRuntime.ttf") as Font;
        return Contents;
    }

   //ボタンの項目を追加する
    public void addMicrobitButton(string deviceName){
        
        //var scrollView = GameObject.Find("BLEScrollView");
        //var viewPort = scrollView.transform.Find("Viewport");
    
        //var scrollView = GameObject.Find("Scroll View");
        var viewPort = this.gameObject.transform.Find("Viewport");
        Transform content = viewPort.transform.Find("Content");

        //項目が存在していなかったら追加する。
        var children = new Transform[content.childCount];
        var childIndex = 0;
        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in content)
        {
            children[childIndex++] = child;
        }
        for(var i = 0; i<childIndex; i++){
            Debug.Log("Child Name["+i+"]="+children[i].name);
            if(children[i].name==deviceName){
                Debug.Log("already exist "+deviceName);
                return;
            }
        }

        GameObject button = createButton(deviceName);
        button.transform.SetParent(content.transform);
        button.GetComponent<Button>().onClick.AddListener(() => { onMicrobitButtonPressed(deviceName);});
    }

    //ボタンからmicri:bitの項目をクリアする
    void clearMicrobitButton(){
        //var scrollView = GameObject.Find("Scroll View");
        var viewPort = this.gameObject.transform.Find("Viewport");
        Transform content = viewPort.transform.Find("Content");
        
        // 子オブジェクトを格納する配列作成
        var children = new Transform[content.childCount];
        var childIndex = 0;
        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in content)
        {
            children[childIndex++] = child;
        }
        
        for(var i = 0; i<childIndex; i++){
            Debug.Log("Child Name["+i+"]="+children[i].name);
            if(children[i].name!="StartScan"){
                children[i].SetParent(null);
                Destroy(children[i].gameObject);
            }
        }
    }

    //micro:bitへのコネクト処理をリクエストする
    void onMicrobitButtonPressed(string deviceName){
        Debug.Log("DeviceButtonPressed"+deviceName);
        clearMicrobitButton();
        bleCntrlWinrt.ConnectBleDevice(deviceName);
    }
    
    //スキャンボタンを押された時の動作
    void onScanButtonPressed(){
        Debug.Log("Scan Pressed");
        //addMicrobitButton("1");
        //addMicrobitButton("2");
        //clearMicrobitButton();
        //bleCntrlWinrt.Disconnect();
        //スキャンが未実地の場合 スキャンを開始する。
        if (!bleCntrlWinrt.IsScanning()){
            //スキャンごとに状態をクリアする。
            bleCntrlWinrt.Disconnect();
            bleCntrlWinrt.StartScanHandler();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //スキャンが終わったら再度スキャンができるようにする
        if (bleCntrlWinrt.IsScanning()){
            scanButton.GetComponentInChildren<Text>().text = "スキャン中";
        }else{
            scanButton.GetComponentInChildren<Text>().text = "スキャン開始";
        }
    }
}