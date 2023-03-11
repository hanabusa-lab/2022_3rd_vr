using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //シーンをロードする際に必要
 
public class GameManager : MonoBehaviour
{
    private float time;
    private int vecY;
    public Text timeUp;
    private float lastTime = 0f;
    public GameObject retryButton;  //リトライボタンを入れる変数
    public bool isPlayingGame = false;
    public AudioSource audioSource;
    public AudioSource audioSourceEnd;
    private bool enbEndSound = false;
    public GameObject DestroyJanken;
 
    void Start()
    {
       Application.targetFrameRate = 30;
        retryButton.SetActive(true); //リトライボタンを非表示
        DestroyJanken.SetActive(false);
        isPlayingGame = false;
    }
 
    void Update()
    {
        //Debug.Log(lastTime);
            
            if(lastTime <= 0 )
            {
                
                retryButton.SetActive(true); //リトライボタンを表示
                audioSource.Stop(); 
                if(isPlayingGame == true){
                    audioSourceEnd.Play(); 
                    timeUp.text = "タイムアップ！";
                }
                isPlayingGame = false;
                DestroyJanken.SetActive(true);
                
            
        }
        else 
        {
            lastTime -= Time.deltaTime;
            timeUp.text = "のこりじかん：" + lastTime.ToString("000");
            
            // audioSource.Play(); 
            
        } 
    }
 
    //シーンを再ロードする（リトライ）
    public void Retry()
    { 
        isPlayingGame = true;
        lastTime = 80.0f;
        timeUp.text = "";
        retryButton.SetActive(false); 
        audioSource.Play(); 
        enbEndSound = true;
        DestroyJanken.SetActive(false);
　　}
}