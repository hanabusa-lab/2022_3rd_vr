using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

//ハンドモデルのコントロールクラス
public class HandCntrl : MonoBehaviour
{
    private Animator animator;
    private bool fg;
    private float f1;
    private float f2; 
    //腕の回転の有無フラグ
    public bool armRotateFg;

    //hand arm
    private GameObject handArm;
    
    //手のモデル
    private GameObject handModel;

    //マテリアルセット
    public Material[] ColorSet = new Material[7];
    private int colorIndex;

    //指の値
    public float fingureAngle1, fingureAngle2;

    //持っているかいなか
    //public bool catchingFg;

    public GameObject havingObject;

    // Start is called before the first frame update
    void Start()
    {
         animator = GetComponent<Animator>();
         //親アームを取得する。
         handArm = GameObject.Find("HandArm");
         fg = true;
         float f1=0;

         //handモデルの色を変える
         handModel=GameObject.Find("HandModel");
         colorIndex = 0;
         handModel.GetComponent<Renderer>().material = ColorSet[colorIndex];

         //指の値を初期化
        fingureAngle1 = 0; 
        fingureAngle2 = 0;

        //持っているかフラグ
        //catchingFg=false; 

        havingObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButtonDown(1)){
            float value = animator.GetFloat("param1");
            Debug.Log("value="+f1.ToString());
            f1+=-0.1f;
            if(f1<0){
                f1=0.0f;
            }
            animator.SetFloat("f1",f1);
            animator.SetFloat("f2",f1);
            animator.SetFloat("f3",f1);
            animator.SetFloat("f4",f1);
            animator.SetFloat("f5",f1);
        }
        if(Input.GetMouseButtonDown(0)){
            float value = animator.GetFloat("param1");
            Debug.Log("value="+f1.ToString());
            f1+=0.1f;
            if(f1>1.0f){
                f1=1.0f;
            }
            animator.SetFloat("f1",f1);
            animator.SetFloat("f2",f1);
            animator.SetFloat("f3",f1);
            animator.SetFloat("f4",f1);
            animator.SetFloat("f5",f1);
        }*/
    }

    //micro:bitから取得情報クラス
     [Serializable]
    public class GloveData
    {
        public string dat1;
        public string dat2;
    }

    //GloveのAngle更新
    public void OnGloveAngleChanged(String json)
    {
        GloveData angle = JsonUtility.FromJson<GloveData>(json);
        //Debug.Log("onGloveAngleChanged json="+json+" value="+angle.dat1+" "+angle.dat1);

        //handの移動
        Transform tmpTransform = this.transform;
        Vector3 localAngle = tmpTransform.localEulerAngles;
        localAngle.x = -1*int.Parse(angle.dat1)+90.0f; // ローカル座標を基準に、x軸を軸にした回転を10度に変更
        localAngle.y = -1*int.Parse(angle.dat2); // ローカル座標を基準に、y軸を軸にした回転を10度に変更
        //localAngle.z = 10.0f; // ローカル座標を基準に、z軸を軸にした回転を10度に変更
        tmpTransform.localEulerAngles = localAngle; // 回転角度を設定
        
        //armの移動
        if(armRotateFg){
            tmpTransform = handArm.transform;
            localAngle = tmpTransform.localEulerAngles;
            localAngle.x = -1*int.Parse(angle.dat1)+90.0f; // ローカル座標を基準に、x軸を軸にした回転を10度に変更
            localAngle.y = -1*int.Parse(angle.dat2); // ローカル座標を基準に、y軸を軸にした回転を10度に変更     
            tmpTransform.localEulerAngles = localAngle; // 回転角度を設定
        }
    }

    //Gloveの指の更新
    public void OnGloveFingureChanged(String json)
    {
        GloveData fingure = JsonUtility.FromJson<GloveData>(json);
       // Debug.Log("OnGloveFingureChanged "+json+""+fingure.dat1+" "+fingure.dat2);

        float f1 = Mathf.Abs(int.Parse(fingure.dat1));
        if(f1>90){
            f1=90;
        }
        //0-90の傾きを指の角度として保持しておく
        fingureAngle1 = f1;

        //アニメーションのために0-1に正規化
        f1 = f1/90.0f;
        animator.SetFloat("f1",f1);

        float f2 = Mathf.Abs(int.Parse(fingure.dat2));
        if(f2>90){
            f2=90;
        }
        fingureAngle2 = f2;

        //アニメーションのために0-1に正規化
        f2 = f2/90f;
        animator.SetFloat("f2",f2);

        //チョキ対応 f1の値がみの値が大きい場合には、f3-f5は握ったままにする。
        float f3 = f2;
        float of = f2;
        if(f1 > 0.5f){
            of = 1.0f;
            f3 = f2;
        }
        animator.SetFloat("f3",f3);
        animator.SetFloat("f4",of);
        animator.SetFloat("f5",of);
    }
    
    //GloveのHand位置の更新
    public void OnGloveHandPosChanged(String json)
    {
        GloveData pos = JsonUtility.FromJson<GloveData>(json);
        //Debug.Log("OnGloveHandPosChanged "+json+" "+pos.dat1+" "+pos.dat2);

        //handの位置移動
        Transform tmpTransform = this.transform;
        /*
        Vector3 localAngle = tmpTransform.localEulerAngles;
        localAngle.x = -1*int.Parse(angle.p)+90.0f; // ローカル座標を基準に、x軸を軸にした回転を10度に変更
        localAngle.y = -1*int.Parse(angle.r); // ローカル座標を基準に、y軸を軸にした回転を10度に変更
        localAngle.z = 10.0f; // ローカル座標を基準に、z軸を軸にした回転を10度に変更
        tmpTransform.localEulerAngles = localAngle; // 回転角度を設定
        */
        this.transform.position = new Vector3(int.Parse(pos.dat1)/100.0f, int.Parse(pos.dat2)/100.0f, tmpTransform.position.z);
 
    }

    //Reset ArmAngle
    //腕の角度を初期化する。
    public void OnResetArmAgnel()
    {
        //armの移動
        Transform tmpTransform = handArm.transform;
        Vector3 localAngle = tmpTransform.localEulerAngles;
        localAngle.x = 0;
        localAngle.y = 0;
        localAngle.z = 0;     
        tmpTransform.localEulerAngles = localAngle;
    }

    public void ChangeGloveColoe(){
        colorIndex +=1;
        Debug.Log(ColorSet.Length);
        if(colorIndex>=ColorSet.Length){
            colorIndex = 0;
        }
         handModel.GetComponent<Renderer>().material = ColorSet[colorIndex];

    }

    //手の状況を確認を行う
    public Const.JankenType checkJankenType(){
        //今のところ、ぐーとぱーだけ
        //デフォルトはパー
        Const.JankenType type = Const.JankenType.Paa;

        if(fingureAngle1>45 || fingureAngle2>45){
            type = Const.JankenType.Guu;
        }
        return type;
    }

}
