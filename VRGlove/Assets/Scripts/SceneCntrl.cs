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
    //private GameObject throwCntrl;

    //ハンドコントローラ
    public GameObject hand;

    //block list
    private List<GameObject> blockList;

    // Start is called before the first frame update
    void Start()
    {
        blockList = new List<GameObject>();

        mode = SceneMode.Normal;
        premode = SceneMode.Normal;
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
        //SimpleBallの場合はブロックを作成する。
        if(premode!=mode && mode==Const.SceneMode.SimpleBall){
             CreateBlock();
        }
        //SimpleBall以外に切り替わった場合
        if(premode!=mode && premode==Const.SceneMode.SimpleBall){
             ClearBlock();
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
         if(mode==Const.SceneMode.RandomBalls){
            //Armのうごきを行う
            hand.GetComponent<HandCntrl>().armRotateFg=true;
         }

         premode = mode;
    }


}
