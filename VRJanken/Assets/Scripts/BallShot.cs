using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BallShot : MonoBehaviour
{
    public GameObject prefab; //ボールのPrefabデータを入れる
    public float power; //ボールに加える力
    private float time; //連射防止
 
    void Start()
    {
        time = 2.0f; //タイムを2秒で始めて、最初の1回は発射OK
    }
 
    void Update()
    {
        time += Time.deltaTime;　//連射防止 カウントアップタイマー
        
        if(time >= 0.5f)　//連射防止 2秒たてば実行
        {
            if(Input.GetMouseButtonDown(0)) //マウス左クリックしたとき
            {
            
            //Prefabのボールを生成
            GameObject ball = GameObject.Instantiate(prefab) as GameObject;
 
            //ボールの発射位置をControllerの位置にする
            ball.transform.position = this.transform.position;
 
            //マウスクリックした位置にRayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            //rayの方向を取得し、targetに格納
            Vector3 target = ray.direction;
 
            //クリックしたところに向けて、ボールに加速度をつける
            ball.GetComponent<Rigidbody>().velocity = target * power;
 
            time = 0.0f;　//連射防止 タイマーを0に戻す
            }
        }
    }
}