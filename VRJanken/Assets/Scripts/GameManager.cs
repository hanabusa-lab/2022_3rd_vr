using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //シーンをロードする際に必要
 
public class GameManager : MonoBehaviour
{
    public GameObject[] targetArray;
    private int count;
    private float time;
    private int vecY;
    public Text timeUp;
    private float lastTime = 60.0f;
    public GameObject retryButton;  //リトライボタンを入れる変数
 
    void Start()
    {
        count = 0;
        retryButton.SetActive(false); //リトライボタンを非表示
    }
 
    void Update()
    {
        if(count == targetArray.Length)
        {
            lastTime -= Time.deltaTime;
            if(lastTime <= 0 )
            {
                timeUp.text = "TIME UP";
                GetComponent<BallShot>().enabled = false;
                retryButton.SetActive(true); //リトライボタンを表示
            }
        }
        else 
        {
            time -= Time.deltaTime;
            if(time <= 0.0f)
            {
                vecY = Random.Range(0,12);
                //Instantiate(targetArray[count],new Vector3(-20, vecY, -2),Quaternion.identity);
                time = 5.0f;
                count++;
            }
        } 
    }
 
    //シーンを再ロードする（リトライ）
    public void Retry()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
　　}
}