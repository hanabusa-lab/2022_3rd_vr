using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;


public class SceneCntrl : MonoBehaviour
{
    public Const.SceneMode mode;
    
    public GameObject ballPrefab;
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
        //ボールモード。ボタンを押すとボールが出る。
        if(mode==Const.SceneMode.SimpleBall){
            //右ボタンを押した場合
            if(Input.GetMouseButtonDown(1)){
                Instantiate(this.ballPrefab);
                this.ballPrefab.transform.position = new Vector3(0f, 0f, 8f);
            }
        }
    }
}
