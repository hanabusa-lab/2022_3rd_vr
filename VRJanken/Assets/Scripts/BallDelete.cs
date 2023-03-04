using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BallDelete : MonoBehaviour
{
    void Update()
    {
        //Y座標-10まで落下したとき
        if(transform.position.y <= -10f)
        {
            //オブジェクトを削除する
            Destroy(gameObject);
        }
    }
}