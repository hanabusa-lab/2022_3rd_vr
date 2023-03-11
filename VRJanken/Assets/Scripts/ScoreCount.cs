using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //テキストを扱う場合に必要
 
public class ScoreCount : MonoBehaviour
{
    public int score = 0; //スコアを入れる変数
    public Text ScoreText; //表示するテキスト
    public Material[] ColorSet;
    private int jankenHand = 0;
    public HandCntrl handCntrl;
    public GameManager gameManager;
    private int jankenHandPre = 0;

    [SerializeField]  AudioSource source1;
    [SerializeField]  AudioSource source2;
    [SerializeField]  AudioClip clip1;
    [SerializeField]  AudioClip clip2;
 
    void Update()
    {
        //scoreのデータをテキスト形式に変換。スコアテキストを表示する。
        ScoreText.text = "ポイント: " + score.ToString();
        if (Input.GetKey (KeyCode.Z)) {
            jankenHand = 1;
        }
        if (Input.GetKey (KeyCode.X)) {
            jankenHand = 2;
        }
        if (Input.GetKey (KeyCode.C)) {
            jankenHand = 3;
        }
        if (Input.GetKey (KeyCode.V)) {
            jankenHand = 0;
        }
        jankenHand = handCntrl.janken;
        if(jankenHandPre != jankenHand){
            gameObject.GetComponent<Renderer>().material = ColorSet[jankenHand];
        }
        jankenHandPre = jankenHand;
        

    }
 
    //オブジェクトがぶつかったときの処理
    public void OnCollisionEnter(Collision collision)
    {
        //ぶつかった相手にScorePointタグがついているとき
        if (collision.gameObject.CompareTag("JankenChoki"))
        {
            if(jankenHand == 1){
                source1.PlayOneShot(clip1);
                // Destroy(collision.gameObject); //オブジェクトを消す
                collision.gameObject.SetActive(false);
                score = score + 5;
            }else{
                source2.PlayOneShot(clip2);
                score = score + 1;
            }
        }
        if (collision.gameObject.CompareTag("JankenGu"))
        {
            if(jankenHand == 3){
                source1.PlayOneShot(clip1);
                // Destroy(collision.gameObject); //オブジェクトを消す
                collision.gameObject.SetActive(false);
                score = score + 5;
            }else{
                source2.PlayOneShot(clip2);
                score = score + 1;
            }
        }
        if (collision.gameObject.CompareTag("JankenPa"))
        {
            if(jankenHand == 2){
                source1.PlayOneShot(clip1);
                // Destroy(collision.gameObject); //オブジェクトを消す
                collision.gameObject.SetActive(false);
                score = score + 5;
            }else{
                source2.PlayOneShot(clip2);
                score = score + 1;
            }
        }
 
        //ぶつかった相手にDeleteタグがついているとき
        if (collision.gameObject.CompareTag("Delete"))
        {
            // Destroy(collision.gameObject); //オブジェクトを消す
            collision.gameObject.SetActive(false);
                
        }
    }

    public void ResetScore(){
        score = 0;
    }
}