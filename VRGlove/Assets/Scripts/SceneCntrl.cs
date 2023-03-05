using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;


public class SceneCntrl : MonoBehaviour
{
    public Const.SceneMode mode;
    public GameObject ballPrefab;

    //ハンドコントローラ
    public GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        mode = SceneMode.Normal;
    }

    public void ChangeMode(Const.SceneMode nmode){
        this.mode = nmode;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current shene="+mode);
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
                Instantiate(this.ballPrefab);
                this.ballPrefab.transform.position = new Vector3(0f, 0f, 8f);
            }
        }
        //ランダムモード。
         if(mode==Const.SceneMode.RandomBalls){
            //Armのうごきを行う
            hand.GetComponent<HandCntrl>().armRotateFg=true;
         }
    }
}
