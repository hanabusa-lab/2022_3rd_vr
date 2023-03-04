using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TargetMove : MonoBehaviour
{
    //ターゲットの移動スピード
    [SerializeField] float speed = 10f;
 
    void Start()
    {
        
    }
 
    void Update()
    {
        //X方向へ移動
        transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
        
        //X座標60を越えれば、ターゲットを削除する
        if(transform.position.x >= 60f)
        {
            Destroy(gameObject);
        }
    }
}