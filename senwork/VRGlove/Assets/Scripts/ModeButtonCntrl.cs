using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

//モード管理クラス
//ボタンを押した際に、SceneCntrlのモードを変える。
public class ModeButtonCntrl : MonoBehaviour
{
    public GameObject sceneCntrl;

    void Start()
    {
        sceneCntrl = GameObject.Find("SceneCntrl");
    }

    public void OnClickNormal()
    {
        sceneCntrl.GetComponent<SceneCntrl>().mode = Const.SceneMode.Normal;
    }

    public void OnClickSimpleBall()
    {
        sceneCntrl.GetComponent<SceneCntrl>().mode = Const.SceneMode.SimpleBall;
    }

    public void OnClickRandomBall()
    {
        sceneCntrl.GetComponent<SceneCntrl>().mode = Const.SceneMode.RandomBalls;
    }
}
