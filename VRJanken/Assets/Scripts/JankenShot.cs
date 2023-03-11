using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class JankenShot : MonoBehaviour
{
    public GameObject[] prefab; //ボールのPrefabデータを入れる
    private float power = 3.0f; //ボールに加える力
    private float time = 10.0f; //連射防止
    [SerializeField] GameObject player;
    // private float ballSpeed = 10.0f;
    private float timeTotal = 0f;
    private int game = 0;
    private GameObject[] jankenStocked = new GameObject[50];
    private int count = 0;
 
    void Start()
    {
        time = 6.0f; //タイムを2秒で始めて、最初の1回は発射OK
        timeTotal = 0f;
        Debug.Log("JankenShootStart");
        for(int i = 0; i < 50; i++){
            
            int jankenNo = UnityEngine.Random.Range(0, 3);
            transform.position = new Vector3(-20f, Random.Range(5f, 15f), Random.Range(-15f, 15f));
            //transform.LookAt(player.transform);
            GameObject shotObj = Instantiate(prefab[jankenNo], transform.position, Quaternion.identity);
            shotObj.transform.LookAt(player.transform);
            shotObj.SetActive(false);
            jankenStocked[i] = shotObj;
            
        }
        
        
    }
 
    void Update()
    {
        time = time + Time.deltaTime;　//連射防止 カウントアップタイマー
        timeTotal = timeTotal + Time.deltaTime;

        transform.position = new Vector3(-20f, Random.Range(5f, 15f), Random.Range(-15f, 15f));
        transform.LookAt(player.transform);
        
        
        if(timeTotal >= 80f){
            game = 0;
        }

        // Transform originTransform = transform;
        // transform.Translate(0, Random.Range(-3, 3), Random.Range(-3, 3), Space.World); // ワールドのX軸方向に1移動
        transform.position = new Vector3(-20f, Random.Range(5f, 15f), Random.Range(-10f, 10f));
        transform.LookAt(player.transform);
        float timeSpan = 4f;
        power = 3f;
        if(timeTotal > 20f){
            timeSpan = 3f;
            power = 6f;
        }
        if(timeTotal > 45f){
            timeSpan = 2f;
            power = 10f;
        }
        if(timeTotal > 55f){
            timeSpan = 1f;
            power = 14f;
        }
        if(time >= timeSpan)　//連射防止 2秒たてば実行
        {
            // if(game == true){
            if(game == 1){
                shot();
            }else{
                timeTotal = 0f;
            }
            
            time = 0.0f;　//連射防止 タイマーを0に戻す
            //}
        }
        // transform.position = originTransform.position;
        
    }

    void shot()
    {
        // マテリアル配列の添え字をランダムで設定（0～4）
        // int jankenNo = UnityEngine.Random.Range(0, 3);
        // GameObject shotObj = Instantiate(prefab[jankenNo], transform.position, Quaternion.identity);
        // shotObj.GetComponent<Rigidbody>().velocity = transform.forward * power;
        // Rigidbody shotRigidbody = shotObj.GetComponent<Rigidbody>();
        // shotRigidbody.AddForce(transform.forward * power);
        
        jankenStocked[count].SetActive(true);
        //jankenStocked[count].transform.LookAt(player.transform);
        jankenStocked[count].GetComponent<Rigidbody>().velocity = jankenStocked[count].transform.forward * power;
        count = count + 1;

    }

    public void setGameStart(){
        game = 1;
        timeTotal = 0f;
        count = 0;
        for(int i = 0; i < 50; i++){
            
            int jankenNo = UnityEngine.Random.Range(0, 3);
            transform.position = new Vector3(-20f, Random.Range(5f, 15f), Random.Range(-15f, 15f));
            //transform.LookAt(player.transform);
            //GameObject shotObj = Instantiate(prefab[jankenNo], transform.position, Quaternion.identity);
            jankenStocked[i].transform.position = transform.position;
            jankenStocked[i].transform.rotation = Quaternion.identity;
            jankenStocked[i].transform.LookAt(player.transform);
            jankenStocked[i].SetActive(false);
            
            
        }
        
    }
}