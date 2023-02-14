using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ハンドモデルのコントロールクラス
public class HandCntrl : MonoBehaviour
{
    public Animator animator;
    private bool fg;
    private float f1;
    private float f2; 
    
    // Start is called before the first frame update
    void Start()
    {
         animator = GetComponent<Animator>();
         fg = true;
         float f1=0;
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

    //micro:bitからの情報取得
    [Serializable]
    public class GloveAngle
    {
        public string p;
        public string r;
    }

    // micro:bitの加速度計のx軸方向の値を受け取ります
    public void OnGloveAngleChanged(String json)
    {
        GloveAngle angle = JsonUtility.FromJson<GloveAngle>(json);
        Debug.Log(json+" "+angle.p+" "+angle.r);

        //角度回転
        Transform tmpTransform = this.transform;
 
        // ローカル座標を基準に、回転を取得
        Vector3 localAngle = tmpTransform.localEulerAngles;
        localAngle.x = -1*int.Parse(angle.p)+90.0f; // ローカル座標を基準に、x軸を軸にした回転を10度に変更
        localAngle.y = -1*int.Parse(angle.r); // ローカル座標を基準に、y軸を軸にした回転を10度に変更
        localAngle.z = 10.0f; // ローカル座標を基準に、z軸を軸にした回転を10度に変更
        tmpTransform.localEulerAngles = localAngle; // 回転角度を設定
 
    }

    [Serializable]
    public class GloveFingure
    {
        public string f1;
        public string f2;
    }

    public void OnGloveFingureChanged(String json)
    {
        GloveFingure fingure = JsonUtility.FromJson<GloveFingure>(json);
        Debug.Log(json+""+fingure.f1+" "+fingure.f2);

        float f1 = Mathf.Abs(int.Parse(fingure.f1));
        if(f1>90){
            f1=90;
        }
        f1 = f1/90.0f;
        animator.SetFloat("f1",f1);

        float f2 = Mathf.Abs(int.Parse(fingure.f2));
        if(f2>90){
            f2=90;
        }
        f2 = f2/90.1f;
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

}
