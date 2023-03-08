using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class CoinCntrl : MonoBehaviour
{
    private GameObject hand;
    public bool catchedFg;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody rb =this.transform.GetComponent<Rigidbody> ();//.set_velocityvelocity();
        //rb.velocity = new Vector3(0f, 0f, -2f);
        //this.transform.position = new Vector3(0f, 0f, 8f);
        hand = GameObject.Find("VRGlove");
        catchedFg = false;
    }
   
    void OnBecameInvisible(){
        Destroy(this.gameObject);
        //Debug.Log("destroyed"); 
    }

    void Update(){
        if(catchedFg){
            this.gameObject.transform.position =  hand.GetComponentInChildren<Transform>().position;
            this.gameObject.transform.position = new Vector3(hand.GetComponentInChildren<Transform>().position.x, 
                                                            hand.GetComponentInChildren<Transform>().position.y+0.0f, 
                                                            hand.GetComponentInChildren<Transform>().position.z+0.2f);
                
            this.gameObject.transform.localEulerAngles =  new Vector3(0f,0f,0f);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
	{
		// 当たった相手のRigidbodyのx軸方向に力を加える。
		// 今回はx軸のマイナス方向に力を加えて跳ね返す。
        //VRGloveに当たった場合には、落ちる。
        if(collision.gameObject.tag =="VRGlove"){
            Const.JankenType type =  hand.GetComponent<HandCntrl>().checkJankenType();
            //握ったら手の位置においておく。ただ、握るのは、一つしかにぎれない。
            //if(type == Const.JankenType.Guu && hand.GetComponent<HandCntrl>().havingObject==null){
            if(type == Const.JankenType.Guu && hand.GetComponent<HandCntrl>().havingObject==null){
                
                //hand.GetComponent<HandCntrl>().havingObject=this.gameObject;
                //this.gameObject.transform.position = hand.transform.position;
                //Vector3 tmpposition = new Vector3(hand.transform.position.x, hand.transform.position.y, hand.transform.position.z);
                //transform.Translate(hand.transform.position);
                catchedFg = true;
                //hand.GetComponent<HandCntrl>().catchingFg = true;
                hand.GetComponent<HandCntrl>().havingObject = this.gameObject;
                Debug.Log("Catch "+hand.GetComponent<HandCntrl>().havingObject.name);

                //hand.GetComponent<HandCntrl>().catchingFg
            }
        }
    }
}
