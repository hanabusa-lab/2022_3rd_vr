using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class JankenShot : MonoBehaviour
{
    public GameObject[] prefab; //ボールのPrefabデータを入れる
    private float power; //ボールに加える力
    private float time; //連射防止
    [SerializeField] GameObject player;
    // private float ballSpeed = 10.0f;
    private float timeTotal = 0f;
    public GameManager gameManager;
 
    void Start()
    {
        time = 6.0f; //タイムを2秒で始めて、最初の1回は発射OK
        timeTotal = 0f;
    }
 
    void Update()
    {
        time += Time.deltaTime;　//連射防止 カウントアップタイマー
        timeTotal += Time.deltaTime;

        // Transform originTransform = transform;
        // transform.Translate(0, Random.Range(-3, 3), Random.Range(-3, 3), Space.World); // ワールドのX軸方向に1移動
        transform.position = new Vector3(-20, Random.Range(5, 15), Random.Range(-15, 15));
        transform.LookAt(player.transform);
        float timeSpan = 4f;
        power = 3;
        if(timeTotal > 20f){
            timeSpan = 3f;
            power = 6;
        }
        if(timeTotal > 45f){
            timeSpan = 2f;
            power = 10;
        }
        if(timeTotal > 55f){
            timeSpan = 1f;
            power = 14;
        }
        if(time >= timeSpan)　//連射防止 2秒たてば実行
        {
            if(gameManager.isPlayingGame){
                shot();
            }else{
                timeTotal = 0;
            }
            
            time = 0.0f;　//連射防止 タイマーを0に戻す
            //}
        }
        // transform.position = originTransform.position;
    }

    void shot()
    {
        // マテリアル配列の添え字をランダムで設定（0～4）
        int jankenNo = Random.Range(0, 3);
        GameObject shotObj = Instantiate(prefab[jankenNo], transform.position, Quaternion.identity);
        shotObj.GetComponent<Rigidbody>().velocity = transform.forward * power;
        // Rigidbody shotRigidbody = shotObj.GetComponent<Rigidbody>();
        // shotRigidbody.AddForce(transform.forward * power);
    }
}