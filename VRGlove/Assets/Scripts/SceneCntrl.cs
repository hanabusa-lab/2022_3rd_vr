using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;


public class SceneCntrl : MonoBehaviour
{
    public Const.SceneMode mode;
    public Const.SceneMode premode;
    public GameObject ballPrefab;
    public GameObject BlockPrefab;
    public GameObject coinPrefab;
    //private GameObject throwCntrl;
    public GameObject coinPlane;

    //ハンドコントローラ
    public GameObject hand;

    //block list
    private List<GameObject> blockList;

    private int coinThrowTiming;

    // Start is called before the first frame update
    void Start()
    {
        blockList = new List<GameObject>();

        mode = SceneMode.Normal;
        premode = SceneMode.Normal;

        coinThrowTiming = 0;
    }

    //Blockを作成する
    private void CreateBlock(){
        for(float x=-30f; x<=30f; x+=2f){
            for(float y=-4f; y<=10f; y+=2f){
                GameObject ball = Instantiate(BlockPrefab, new Vector3(x, y, 40f), Quaternion.identity);
                blockList.Add(ball);
            }
        }
        for(float x=-30f; x<=30f; x+=4f){
                GameObject ball = Instantiate(BlockPrefab, new Vector3(x, 12f, 40f), Quaternion.identity);
                blockList.Add(ball);
        }
    }

    //Blockを削除する
    private void ClearBlock(){
        blockList.ForEach(i => {
        Destroy(i); 
        });
        blockList.Clear();
    }

    public void ChangeMode(Const.SceneMode nmode){
        this.mode = nmode;
     }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current shene="+mode);
        //モード切りかえ
        //Normalの場合
        if(premode!=mode && mode==Const.SceneMode.Normal){
             //カメラの座標も変える
            GameObject camera = GameObject.Find("CameraParent");
            camera.transform.position = new Vector3(0f, 0.35f, -0.4f);
        }
        //SimpleBallの場合
        if(premode!=mode && mode==Const.SceneMode.SimpleBall){
             CreateBlock();
             //カメラの座標も変える
            GameObject camera = GameObject.Find("CameraParent");
             //Transform tmpTransform = camera.transform;
            camera.transform.position = new Vector3(0f, 0.35f, -0.4f);

        }
        //SimpleBall以外に切り替わった場合
        if(premode!=mode && premode==Const.SceneMode.SimpleBall){
             ClearBlock();
        }
        //Randomに切り替わった場合
        if(premode!=mode && mode==Const.SceneMode.RandomCoin){
           //カメラの座標も変える
            GameObject camera = GameObject.Find("CameraParent");
             //Transform tmpTransform = camera.transform;
            camera.transform.position = new Vector3(0f, 0.2f, -0.45f);
            hand.GetComponent<HandCntrl>().armRotateFg=true;
            //GameObject.Find("CoinPlane").enabled = true;//SetActive(true);
            coinPlane.SetActive(true);
        }
        //Randomモード以外に切り替わった場合
        if(premode!=mode && premode==Const.SceneMode.RandomCoin){
            hand.GetComponent<HandCntrl>().armRotateFg=false;
            //GameObject.Find("CoinPlane").enabled = false;//SetActive(false);
            coinPlane.SetActive(false);
        }

        //ノーマルモード
        if(mode==Const.SceneMode.Normal){
            //Armのうごきを行はない
            hand.GetComponent<HandCntrl>().armRotateFg=false;
            hand.GetComponent<HandCntrl>().OnResetArmAgnel();
            //Debug.Log("scene normal armRotateFG="+);
        }
        //ボールモード。ボタンを押すとボールが出る。
        if(mode==Const.SceneMode.SimpleBall){
            //Armの動きは行わない
            hand.GetComponent<HandCntrl>().armRotateFg=false;
            hand.GetComponent<HandCntrl>().OnResetArmAgnel();
            
            //右ボタンを押した場合
            if(Input.GetMouseButtonDown(1)){
                this.gameObject.GetComponent<ThrowingCntrl>().ThrowingBall();
                //Instantiate(this.ballPrefab);
                //this.ballPrefab.transform.position = new Vector3(0f, 0f, 8f);
            }
        }
        //ランダムモード。
         if(mode==Const.SceneMode.RandomCoin){
            //常時コインがでてくる
            int posx = Random.Range(-5, 5);
            int posy = Random.Range(0, 5);
            //コインを出す量を調整
            if(coinThrowTiming>10){
                this.gameObject.GetComponent<ThrowingCntrl>().ThrowingCoin(new Vector3(posx/10f,posy/10f,0f));
                coinThrowTiming = 0;
            }
            coinThrowTiming +=1;

            //キーボードにより動作をかえる Z:guu, x:choki, c:paa, v:clear
            //GUU
            if(Input.GetKey(KeyCode.Z)){
                hand.GetComponent<HandCntrl>().fingureAngle1=90f;
                hand.GetComponent<HandCntrl>().fingureAngle2=90f;
                Debug.Log("GUU");     
            }
            //PAA
            if(Input.GetKey(KeyCode.C)){
                hand.GetComponent<HandCntrl>().fingureAngle1=0f;
                hand.GetComponent<HandCntrl>().fingureAngle2=0f;
                Debug.Log("PAA");     
            }

            //手の状態が変わって、ものを握っていたら、ものを飛ばす
            Const.JankenType jankenType=hand.GetComponent<HandCntrl>().checkJankenType();
            Debug.Log("JankenType="+jankenType+"having Object="+hand.GetComponent<HandCntrl>().havingObject);     
            
            //if(hand.GetComponent<HandCntrl>().havingObject!=null && hand.GetComponent<HandCntrl>().checkJankenType()!=Const.JankenType.Guu){
              if(hand.GetComponent<HandCntrl>().havingObject!=null&& jankenType!=Const.JankenType.Guu){
                
                //コインを手放した状態にする。
                hand.GetComponent<HandCntrl>().havingObject.transform.GetComponent<CoinCntrl>().catchedFg=false;

                Debug.Log("Throw object");
                //VR Gloveの角度に応じて跳ね返る方向を変える
                Transform tmpTransform = hand.transform;
                Vector3 worldAngle = tmpTransform.eulerAngles;
                worldAngle.y = Mathf.Repeat(worldAngle.y + 180, 360) - 180;
                worldAngle.x = Mathf.Repeat(worldAngle.x + 180, 360) - 180;
                worldAngle.x = 90f-1f*worldAngle.x+20f;
                
                worldAngle = worldAngle.normalized;
                //Debug.Log("hand angle="+worldAngle.x+" "+worldAngle.y+" "+worldAngle.z);

                var scale = 1f;
                Rigidbody rb =hand.GetComponent<HandCntrl>().havingObject.transform.GetComponent<Rigidbody> ();//.set_velocityvelocity();
                //rb.AddForce(scale*worldAngle.y, scale*worldAngle.x,  scale, ForceMode.Impulse);
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
                rb.AddForce(0, -2f,  2f, ForceMode.Impulse);
                
                //Destroy(hand.GetComponent<HandCntrl>().havingObject);
                hand.GetComponent<HandCntrl>().havingObject=null;
            }
         }

         premode = mode;
    }


}
