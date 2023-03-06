using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCntrl : MonoBehaviour
{
    private GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody rb =this.transform.GetComponent<Rigidbody> ();//.set_velocityvelocity();
        //rb.velocity = new Vector3(0f, 0f, -2f);
        //this.transform.position = new Vector3(0f, 0f, 8f);
        hand = GameObject.Find("VRGlove");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible(){
        Destroy(this.gameObject);
        Debug.Log("destroyed"); 
    }

    private void OnCollisionEnter(Collision collision)
	{
		// 当たった相手のRigidbodyのx軸方向に力を加える。
		// 今回はx軸のマイナス方向に力を加えて跳ね返す。
        //VRGloveに当たった場合には、落ちる。
        if(collision.gameObject.tag =="VRGlove"){
            //VR Gloveの角度に応じて跳ね返る方向を変える
            Transform tmpTransform = hand.transform;
            Vector3 worldAngle = tmpTransform.eulerAngles;
            worldAngle.y = Mathf.Repeat(worldAngle.y + 180, 360) - 180;
            worldAngle.x = Mathf.Repeat(worldAngle.x + 180, 360) - 180;
            worldAngle.x = 90f-1f*worldAngle.x+20f;
            
            
            worldAngle = worldAngle.normalized;
            Debug.Log("hand angle="+worldAngle.x+" "+worldAngle.y+" "+worldAngle.z);

            var scale = 15f;
	        Rigidbody rb =this.transform.GetComponent<Rigidbody> ();//.set_velocityvelocity();
    	    rb.AddForce(scale*worldAngle.y, scale*worldAngle.x,  scale, ForceMode.Impulse);
            rb.useGravity = true;
        }
    }
}
